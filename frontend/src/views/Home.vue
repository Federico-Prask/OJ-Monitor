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
    const loading = ref(false)

    // 使用 store 中的 platforms 和 activePlatform
    const platforms = computed(() => platformStore.platforms)
    const activePlatform = computed(() => platformStore.activePlatform)
    const activePlatformIndex = computed(() => platformStore.activePlatformIndex)

    const handlePlatformChange = (index) => {
      platformStore.setActivePlatform(index)
    }

    const refreshData = async () => {
      loading.value = true
      await platformStore.fetchPlatformData(activePlatform.value.id)
      loading.value = false
    }

    onMounted(() => {
      // 确保平台数据已初始化
      if (platformStore.platforms.length === 0) {
        platformStore.initializePlatforms()
      }
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