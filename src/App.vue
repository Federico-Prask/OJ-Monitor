<template>
  <div id="app" class="min-h-screen bg-gray-50">
    <MainLayout>
      <Home />
    </MainLayout>
    <DebugInfo />
  </div>
</template>

<script>
import MainLayout from './components/layout/MainLayout.vue'
import Home from './views/Home.vue'
import DebugInfo from './components/common/DebugInfo.vue'

export default {
  name: 'App',
  components: {
    MainLayout,
    Home,
    DebugInfo
  }
}
</script>