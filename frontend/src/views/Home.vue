<template>
  <div class="home">
    <!-- 主菜单标签页 -->
    <div class="mb-6 border-b border-gray-200">
      <div class="flex gap-4">
        <button
          @click="activeTab = 'platforms'"
          :class="[
            'px-4 py-2 font-semibold border-b-2 transition',
            activeTab === 'platforms'
              ? 'border-blue-500 text-blue-600'
              : 'border-transparent text-gray-600 hover:text-gray-800'
          ]"
        >
          📊 平台查询
        </button>
        <button
          @click="activeTab = 'watchlist'"
          :class="[
            'px-4 py-2 font-semibold border-b-2 transition',
            activeTab === 'watchlist'
              ? 'border-blue-500 text-blue-600'
              : 'border-transparent text-gray-600 hover:text-gray-800'
          ]"
        >
          🎯 监视列表
        </button>
      </div>
    </div>

    <!-- 平台查询内容 -->
    <div v-show="activeTab === 'platforms'">
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

    <!-- 监视列表内容 -->
    <div v-show="activeTab === 'watchlist'">
      <WatchlistManager />
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { usePlatformStore } from '../stores/platformStore'
import PlatformTabs from '../components/platform/PlatformTabs.vue'
import PlatformPanel from '../components/platform/PlatformPanel.vue'
import WatchlistManager from '../components/watchlist/WatchlistManager.vue'

export default {
  name: 'Home',
  components: {
    PlatformTabs,
    PlatformPanel,
    WatchlistManager
  },
  setup() {
    const platformStore = usePlatformStore()
    const loading = ref(false)
    const activeTab = ref('platforms')

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
      activeTab,
      handlePlatformChange,
      refreshData
    }
  }
}
</script>