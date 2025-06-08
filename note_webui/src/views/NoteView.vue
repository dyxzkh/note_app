<template>
  <div class="flex justify-center w-full">
    <div class="container p-5">
      <h1 class="text-[2rem] text-center">Note Management</h1>
      <!-- Create Note Form -->
      <Fieldset :legend="isEditing ? 'Edit Note' : 'Create Note'" class="mb-5">
        <div class="flex flex-col gap-4">
          <FloatLabel variant="on">
            <InputText :invalid="titleError" v-model="currentNote.title" fluid />
            <label>Title</label>
          </FloatLabel>
          <Message v-if="titleError" severity="error" size="small" variant="simple">
            Title is required
          </Message>

          <FloatLabel variant="on">
            <Textarea v-model="currentNote.content" rows="5" cols="30" fluid />
            <label>Content</label>
          </FloatLabel>

          <div class="flex gap-2">
            <Button
              v-if="isEditing"
              label="Cancel"
              severity="secondary"
              size="small"
              outlined
              @click="resetForm"
            />
            <Button
              :label="isEditing ? 'Update Note' : 'Create Note'"
              :icon="isEditing ? 'pi pi-check' : 'pi pi-plus'"
              severity="primary"
              size="small"
              outlined
              @click="isEditing ? handleUpdateNote() : handleCreateNote()"
            />
          </div>
        </div>
      </Fieldset>

      <!-- Notes Table -->
      <DataTable
        v-model:filters="filters"
        :value="noteStore.notes"
        paginator
        :rows="5"
        :rowsPerPageOptions="[5, 10, 20, 50]"
        tableStyle="min-width: 50rem"
        class="mt-5"
      >
        <template #header>
          <div class="flex justify-end">
            <IconField>
              <InputIcon>
                <i class="pi pi-search" />
              </InputIcon>
              <InputText
                v-model="filters['global'].value"
                size="small"
                placeholder="Keyword Search"
              />
            </IconField>
          </div>
        </template>
        <Column field="id" header="Id" sortable style="width: 20%"></Column>
        <Column field="title" header="Title" sortable style="width: 25%"></Column>
        <Column field="content" header="Content" sortable style="width: 35%"></Column>
        <Column header="Actions" style="width: 20%; white-space: nowrap">
          <template #body="{ data }">
            <Button
              label="Edit"
              icon="pi pi-pencil"
              severity="secondary"
              size="small"
              @click="editNote(data.id)"
              outlined
              class="mr-2"
            />
            <Button
              label="Delete"
              icon="pi pi-trash"
              severity="danger"
              size="small"
              outlined
              @click="confirmDelete($event, data.id)"
            />
          </template>
        </Column>
      </DataTable>

      <Button label="Logout" @click="logout" class="mt-6 bg-red-500 hover:bg-red-600" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useNoteStore } from '@/stores/note'
import { useConfirm } from 'primevue/useconfirm'
import { FilterMatchMode } from '@primevue/core/api'
import { useToast } from 'primevue'

const authStore = useAuthStore()
const noteStore = useNoteStore()
const toast = useToast()
const confirm = useConfirm()

const isEditing = ref(false)
const titleError = ref(false)

const currentNote = ref({
  id: 0,
  title: '',
  content: '',
  createdBy: authStore.userId || 0,
})

const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  id: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  title: { value: null, matchMode: FilterMatchMode.IN },
  content: { value: null, matchMode: FilterMatchMode.EQUALS },
})

// Watch for user changes to set the createdBy field
watch(
  () => authStore.user,
  (user) => {
    if (user?.id) {
      currentNote.value.createdBy = user.id
    }
  },
  { immediate: true },
)

const confirmDelete = (event: PointerEvent | any, id: number) => {
  confirm.require({
    target: event.currentTarget,
    message: 'Do you want to delete this record?',
    icon: 'pi pi-info-circle',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true,
    },
    acceptProps: {
      label: 'Delete',
      severity: 'danger',
    },
    accept: async () => {
      await deleteNote(id)
      // Reset form if deleting the currently edited note
      if (isEditing.value && currentNote.value.id === id) {
        resetForm()
      }
      toast.add({
        severity: 'success',
        summary: 'Confirmed',
        detail: 'Record deleted',
        life: 3000,
      })
    },
  })
}

function resetForm() {
  isEditing.value = false
  currentNote.value = {
    id: 0,
    title: '',
    content: '',
    createdBy: authStore.userId || 0,
  }
  titleError.value = false
}

async function editNote(id: number) {
  try {
    await noteStore.fetchNoteById(id)

    if (noteStore.currentNote) {
      currentNote.value = {
        id: noteStore.currentNote.id,
        title: noteStore.currentNote.title,
        content: noteStore.currentNote.content,
        createdBy: noteStore.currentNote.createdBy,
      }
      isEditing.value = true
    }
  } catch (error) {
    console.error('Error fetching note for edit:', error)
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: 'Failed to load note for editing',
      life: 3000,
    })
  }
}

async function handleCreateNote() {
  try {
    if (!authStore.userId) {
      toast.add({
        severity: 'error',
        summary: 'Create Note Failed',
        detail: 'You must be logged in to create a note.',
        life: 3000,
      })
      return
    }

    if (!currentNote.value.title) {
      titleError.value = true
      toast.add({
        severity: 'error',
        summary: 'Create Note Failed',
        detail: 'Title is required to create a note.',
        life: 3000,
      })
      return
    }

    await noteStore.createNote({
      title: currentNote.value.title,
      content: currentNote.value.content,
      createdBy: authStore.userId,
    })

    // Reset form and refresh notes
    resetForm()
    await noteStore.fetchNotesByUserId(authStore.userId)

    toast.add({
      severity: 'success',
      summary: 'Note Created Successfully',
      life: 3000,
    })
  } catch (error) {
    console.error('Error creating note:', error)
    titleError.value = false
  }
}

async function handleUpdateNote() {
  try {
    if (!currentNote.value.title) {
      titleError.value = true
      toast.add({
        severity: 'error',
        summary: 'Update Failed',
        detail: 'Title is required to update a note.',
        life: 3000,
      })
      return
    }

    await noteStore.updateNote(currentNote.value.id, {
      title: currentNote.value.title,
      content: currentNote.value.content,
    })

    // Reset form and refresh notes
    resetForm()
    if (authStore.userId) {
      await noteStore.fetchNotesByUserId(authStore.userId)
    }

    toast.add({
      severity: 'success',
      summary: 'Success',
      detail: 'Note updated successfully',
      life: 3000,
    })
  } catch (error) {
    console.error('Error updating note:', error)
    toast.add({
      severity: 'error',
      summary: 'Update Failed',
      detail: 'Failed to update note',
      life: 3000,
    })
  }
}

async function deleteNote(id: number) {
  try {
    await noteStore.deleteNote(id)
    if (authStore.userId) {
      await noteStore.fetchNotesByUserId(authStore.userId)
    }
  } catch (error) {
    console.error('Error deleting note:', error)
  }
}

function logout() {
  authStore.logout()
}

onMounted(async () => {
  if (authStore.userId) {
    await noteStore.fetchNotesByUserId(authStore.userId)
    console.log('Notes fetched:', noteStore.notes)
  } else {
    console.error('No user ID available when mounting component')
  }
})
</script>
