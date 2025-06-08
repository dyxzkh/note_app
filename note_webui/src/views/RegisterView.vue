<template>
  <div
    class="flex justify-center items-center flex-col w-full min-h-screen p-4 sm:p-8 md:p-12 lg:p-16"
  >
    <div class="mb-8">
      <img
        oncontextmenu="return false;"
        draggable="false"
        src="/images/service-desk.png"
        alt="Logo"
        class="max-w-[400px] min-w-[100px] w-full z-[100]"
      />
    </div>

    <h3 style="color: var(--p-primary-color)" class="text-lg sm:text-xl font-bold mb-4 text-center">
      Register
    </h3>

    <Form
      v-slot="$form"
      :initialValues="initialValues"
      :resolver="resolver"
      :validateOnValueUpdate="false"
      :validateOnBlur="true"
      @submit="onFormSubmit"
      class="flex flex-col gap-4 w-full sm:w-[350px] md:w-[400px]"
    >
      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <InputText id="fullname" type="text" name="fullname" fluid />
          <label for="fullname">Fullname</label>
        </FloatLabel>
        <Message v-if="$form.fullname?.invalid" severity="error" size="small" variant="simple">
          {{ $form.fullname.error.message }}
        </Message>
      </div>

      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <InputText id="email" type="email" name="email" fluid autocomplete="username" />
          <label for="email">Email</label>
        </FloatLabel>
        <Message v-if="$form.email?.invalid" severity="error" size="small" variant="simple">
          {{ $form.email.error.message }}
        </Message>
      </div>

      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <InputText
            id="password"
            type="password"
            name="password"
            autocomplete="current-password"
            fluid
          />
          <label for="password">Password</label>
        </FloatLabel>
        <Message v-if="$form.password?.invalid" severity="error" size="small" variant="simple">
          {{ $form.password.error.message }}
        </Message>
      </div>

      <Button type="submit" icon="pi pi-sign-in" severity="primary" size="small" label="Register" />
      <Divider style="margin: 0" />
      <router-link :to="'/login'">
        <Button
          icon="pi pi-arrow-left"
          severity="secondary"
          size="small"
          label="Back to Login"
          class="w-full"
        />
      </router-link>
    </Form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useToast } from 'primevue/usetoast'
import type { FormSubmitEvent } from '@primevue/forms'
import { useAuthStore } from '@/stores/auth'
import type { AxiosError } from 'axios'

interface FormValues {
  fullname: string
  email: string
  password: string
}

const toast = useToast()

const initialValues = ref<FormValues>({
  fullname: '',
  email: '',
  password: '',
})

// Resolver to validate fields manually
const resolver = ({
  values,
}: {
  values: Record<string, any>
}): { values: Record<string, any>; errors: Record<string, any> } => {
  const errors: Record<string, any> = {}

  const fullname = values.fullname as string
  const email = values.email as string
  const password = values.password as string

  if (!fullname) {
    errors.fullname = [{ message: 'Fullname is required.' }]
  }

  if (!email) {
    errors.email = [{ message: 'Email is required.' }]
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
    errors.email = [{ message: 'Please enter a valid email address.' }]
  }

  if (!password) {
    errors.password = [{ message: 'Password is required.' }]
  } else if (password.length < 6) {
    errors.password = [{ message: 'Password must be at least 6 characters.' }]
  }

  return {
    values,
    errors,
  }
}

// Submit handler
const onFormSubmit = async (event: FormSubmitEvent<Record<string, any>>) => {
  const { valid, values } = event

  if (valid) {
    try {
      const authStore = useAuthStore()
      await authStore.registerUser(values as FormValues).then(() => {
        toast.add({
          severity: 'success',
          summary: 'Reister Successful',
          detail: 'You are now back to login page.',
          life: 3000,
        })
      })
    } catch (err: AxiosError | any) {
      console.error('Login error:', err)
      toast.add({
        severity: 'error',
        summary: 'Login Failed',
        detail: err.response.data.message,
        life: 3000,
      })
    }
  } else {
    toast.add({
      severity: 'error',
      summary: 'Validation Error',
      detail: 'Please validate all errors before submitting.',
      life: 3000,
    })
  }
}
</script>
