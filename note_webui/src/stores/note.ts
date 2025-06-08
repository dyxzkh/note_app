import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/services/api'
import type { Note, CreateNoteDto, UpdateNoteDto, ApiResponse } from '@/models/note'

export const useNoteStore = defineStore('note', () => {
  const notes = ref<Note[]>([])
  const currentNote = ref<Note | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  // Fetch all notes
  async function fetchAllNotes() {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.get<Note[]>('/note')
      notes.value = response.data
    } catch (err) {
      error.value = 'Failed to fetch notes'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Get note by ID
  async function fetchNoteById(id: number) {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.get<Note>(`/note/${id}`)
      currentNote.value = response.data
    } catch (err) {
      error.value = 'Failed to fetch note'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Get notes by user ID
  async function fetchNotesByUserId(userId: number) {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.get<Note[]>(`/note/user/${userId}`)
      notes.value = response.data
    } catch (err) {
      error.value = 'Failed to fetch user notes'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Create new note
  async function createNote(noteData: CreateNoteDto) {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.post<ApiResponse<Note>>('/note', noteData)

      if (!response.data.success) {
        throw new Error(response.data.errorMessage)
      }

      // if (response.data.data) {
      //   notes.value.push(response.data.data)
      //   return response.data.data
      // }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to create note'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Update existing note
  async function updateNote(id: number, noteData: UpdateNoteDto) {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.put<ApiResponse<boolean>>(`/note/${id}`, noteData)
      //console.log(response)

      if (!response.data.success) {
        throw new Error(response.data.errorMessage)
      }

      if (response.data.data) {
        // Refresh the note in the store
        await fetchNoteById(id)
        // Also refresh the list if needed
        await fetchAllNotes()
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to update note'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Delete note
  async function deleteNote(id: number) {
    try {
      isLoading.value = true
      error.value = null
      const response = await api.delete<boolean>(`/note/${id}`)

      if (response.data) {
        // Remove from local state
        notes.value = notes.value.filter((note) => note.id !== id)
        if (currentNote.value?.id === id) {
          currentNote.value = null
        }
      }
    } catch (err) {
      error.value = 'Failed to delete note'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  // Clear current note
  function clearCurrentNote() {
    currentNote.value = null
  }

  // Clear all notes
  function clearNotes() {
    notes.value = []
  }

  return {
    notes,
    currentNote,
    isLoading,
    error,
    fetchAllNotes,
    fetchNoteById,
    fetchNotesByUserId,
    createNote,
    updateNote,
    deleteNote,
    clearCurrentNote,
    clearNotes,
  }
})
