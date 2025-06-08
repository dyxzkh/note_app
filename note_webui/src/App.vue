<script setup lang="ts">
import { RouterView } from 'vue-router'
import ConfirmPopup from 'primevue/confirmpopup'
</script>

<template>
  <Toast />
  <ConfirmPopup></ConfirmPopup>
  <RouterView v-slot="{ Component }">
    <transition name="fade" mode="out-in">
      <component class="w-full" :is="Component" />
    </transition>
  </RouterView>
</template>

<style>
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-to,
.fade-leave-from {
  opacity: 1;
}
</style>
