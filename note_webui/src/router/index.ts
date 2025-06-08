import LoginView from '@/views/LoginView.vue'
import NoteView from '@/views/NoteView.vue'
import RegisterView from '@/views/RegisterView.vue'
import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: NoteView,
      meta: { title: 'Notes', requiresAuth: true },
    },
    {
      path: '/notes',
      name: 'note',
      component: NoteView,
      meta: { title: 'Notes', requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      meta: { title: 'Login' },
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
      meta: { title: 'Register' },
    },
  ],
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()

  // Initialize auth state if token exists
  if (authStore.accessToken && !authStore.userId) {
    try {
      await authStore.initializeAuth()
    } catch (error) {
      console.error('Auth initialization failed:', error)
      return '/login'
    }
  }

  // Redirect to login if auth is required but user is not authenticated
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return '/login'
  }
})

export default router
