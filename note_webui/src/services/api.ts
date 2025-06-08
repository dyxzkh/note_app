import { useAuthStore } from '@/stores/auth'
import axios, {
  type AxiosInstance,
  type AxiosResponse,
  type InternalAxiosRequestConfig,
  AxiosError,
} from 'axios'

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5225/api/',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request interceptor
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const authStore = useAuthStore()
    const token = authStore.accessToken

    if (token) {
      config.headers = config.headers || {}
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  },
  (error: AxiosError) => {
    return Promise.reject(error)
  },
)

// Response interceptor
api.interceptors.response.use(
  (response: AxiosResponse) => {
    return response
  },
  async (error: AxiosError) => {
    const authStore = useAuthStore()
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean }

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      try {
        const refreshToken = authStore.refreshToken
        if (refreshToken) {
          const response = await axios.post('/auth/refresh-token', { refreshToken })
          const { accessToken } = response.data

          authStore.setAccessToken(accessToken)

          // Update headers
          originalRequest.headers = originalRequest.headers || {}
          originalRequest.headers.Authorization = `Bearer ${accessToken}`

          return api(originalRequest)
        }
      } catch (refreshError) {
        authStore.logout()
        return Promise.reject(refreshError)
      }
    }

    return Promise.reject(error)
  },
)

export default api
