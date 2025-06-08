import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import Button from 'primevue/button'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Message from 'primevue/message'
import ToastService from 'primevue/toastservice'
import Toast from 'primevue/toast'
import Textarea from 'primevue/textarea'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'
import Fieldset from 'primevue/fieldset'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import AnimateOnScroll from 'primevue/animateonscroll'
import Divider from 'primevue/divider'
import ConfirmationService from 'primevue/confirmationservice'

import { Form } from '@primevue/forms'

import axios from 'axios'
import VueAxios from 'vue-axios'

import App from './App.vue'
import router from './router'

import 'primeicons/primeicons.css'

const app = createApp(App)

app.use(PrimeVue, {
  theme: {
    preset: Aura,
  },
})
app.component('Button', Button)
app.component('FloatLabel', FloatLabel)
app.component('InputText', InputText)
app.component('Form', Form)
app.component('Fieldset', Fieldset)
app.component('Message', Message)
app.component('Divider', Divider)
app.component('Toast', Toast)
app.component('Textarea', Textarea)
app.component('IconField', IconField)
app.component('InputIcon', InputIcon)
app.component('DataTable', DataTable)
app.component('Column', Column)
app.directive('animateonscroll', AnimateOnScroll)
app.use(ConfirmationService)
app.use(ToastService)
app.use(createPinia())
app.use(router)
app.use(VueAxios, axios)
app.provide('axios', app.config.globalProperties.axios)

app.mount('#app')
