// src/stores/auth.ts
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import router from '@/router'
import api from '@/services/api'
import type { CreateUserDto } from '@/models/create_user.dto'
import { jwtDecode } from 'jwt-decode'
import type { GetUserDto } from '@/models/getuser'

export const useAuthStore = defineStore('auth', () => {
  const accessToken = ref<string | null>(localStorage.getItem('accessToken'))
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))
  const user = ref<GetUserDto | null>(null)

  const isAuthenticated = computed(() => !!accessToken.value)
  const userId = computed(() => user.value?.id || null)

  function setAccessToken(token: string) {
    accessToken.value = token
    localStorage.setItem('accessToken', token)
  }

  function setRefreshToken(token: string) {
    refreshToken.value = token
    localStorage.setItem('refreshToken', token)
  }

  function setUser(userData: GetUserDto) {
    user.value = userData
    // Store user data in localStorage if needed
    localStorage.setItem('user', JSON.stringify(userData))
  }

  const initializeAuth = async () => {
    if (accessToken.value) {
      const decodedToken = decodeToken(accessToken.value)
      if (decodedToken?.id) {
        await fetchUser(Number(decodedToken.id))
      }
    }
  }

  async function login(credentials: { email: string; password: string }) {
    try {
      const response = await api.post('auth/login', credentials)
      setAccessToken(response.data.accessToken)
      setRefreshToken(response.data.refreshToken)

      const decodedToken = decodeToken(response.data.accessToken)
      if (decodedToken?.id) {
        await fetchUser(Number(decodedToken.id))
      }
      router.push('/notes')
    } catch (error) {
      throw error
    }
  }

  async function fetchUser(userId: number) {
    try {
      const response = await api.get<GetUserDto>(`/user/${userId}`)
      setUser(response.data)
    } catch (error) {
      console.error('Failed to fetch user:', error)
      throw error
    }
  }

  async function registerUser(dto: CreateUserDto) {
    try {
      await api.post('/user/register', dto)
      router.push('/login')
    } catch (error) {
      console.error('Registration error:', error)
      throw error
    }
  }

  function logout() {
    accessToken.value = null
    refreshToken.value = null
    user.value = null
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    router.push('/login')
  }

  function decodeToken(token: string): any {
    try {
      return jwtDecode(token)
    } catch (Error) {
      return null
    }
  }

  return {
    accessToken,
    refreshToken,
    user,
    isAuthenticated,
    setAccessToken,
    setRefreshToken,
    registerUser,
    login,
    logout,
    initializeAuth,
    userId,
  }
})
