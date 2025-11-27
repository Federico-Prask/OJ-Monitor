<template>
  <div class="home">
    <!-- 平台切换标签 -->
    <PlatformTabs 
      :platforms="platforms"
      :active-platform-index="activePlatformIndex"
      @platform-change="handlePlatformChange"
    />

    <!-- 平台内容区域 -->
    <PlatformPanel 
      :platform="activePlatform"
      :loading="loading"
      @refresh="refreshData"
    />
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { usePlatformStore } from '../stores/platformStore'
import PlatformTabs from '../components/platform/PlatformTabs.vue'
import PlatformPanel from '../components/platform/PlatformPanel.vue'

export default {
  name: 'Home',
  components: {
    PlatformTabs,
    PlatformPanel
  },
  setup() {
    const platformStore = usePlatformStore()
    const activePlatformIndex = ref(0)
    const loading = ref(false)

    const platforms = computed(() => platformStore.platforms)
    const activePlatform = computed(() => platformStore.platforms[activePlatformIndex.value])

    const handlePlatformChange = (index) => {
      activePlatformIndex.value = index
      platformStore.setActivePlatform(index)
    }

    const refreshData = async () => {
      loading.value = true
      await platformStore.fetchPlatformData(activePlatform.value.id)
      loading.value = false
    }

    onMounted(() => {
      platformStore.initializePlatforms()
    })

    return {
      platforms,
      activePlatformIndex,
      activePlatform,
      loading,
      handlePlatformChange,
      refreshData
    }
  }
}
</script>