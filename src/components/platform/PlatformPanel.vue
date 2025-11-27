<template>
  <div v-if="platform" class="bg-white rounded-lg shadow-lg p-6 mb-8 transition-all duration-300">
    <!-- 平台头部信息 -->
    <div class="flex flex-col md:flex-row justify-between items-center mb-6">
      <div class="flex items-center mb-4 md:mb-0">
        <div class="w-12 h-12 rounded-full flex items-center justify-center text-white text-xl" 
             :style="{ backgroundColor: platform.color || '#6b7280' }">
          <i class="fas fa-laptop-code"></i>
        </div>
        <div class="ml-4">
          <h2 class="text-2xl font-bold text-gray-800">{{ platform.name }}</h2>
          <p class="text-gray-600">{{ platform.description }}</p>
        </div>
      </div>
      <div class="flex space-x-4">
        <button 
          @click="$emit('refresh')"
          class="bg-indigo-100 text-indigo-700 px-4 py-2 rounded-lg flex items-center hover:bg-indigo-200 transition-colors"
          :disabled="loading"
        >
          <i class="fas fa-sync-alt mr-2" :class="{ 'animate-spin': loading }"></i> 
          {{ loading ? '刷新中...' : '刷新数据' }}
        </button>
        <button class="bg-indigo-600 text-white px-4 py-2 rounded-lg flex items-center hover:bg-indigo-700 transition-colors">
          <i class="fas fa-cog mr-2"></i> 设置
        </button>
      </div>
    </div>

    <!-- 数据统计卡片 -->
    <StatsCards :stats="platform.stats || { solved: 0, submissions: 0, accuracy: 0 }" />

    <!-- 做题趋势图 -->
    <TrendChart :platform="platform" />

    <!-- 最近做题记录 -->
    <RecentRecords :records="platform.recentRecords || []" />
  </div>
  <div v-else class="bg-white rounded-lg shadow-lg p-6 mb-8 text-center">
    <p class="text-gray-500">加载中...</p>
  </div>
</template>

<script>
import StatsCards from './StatsCards.vue'
import TrendChart from './TrendChart.vue'
import RecentRecords from './RecentRecords.vue'

export default {
  name: 'PlatformPanel',
  components: {
    StatsCards,
    TrendChart,
    RecentRecords
  },
  props: {
    platform: {
      type: Object,
      default: null
    },
    loading: {
      type: Boolean,
      default: false
    }
  },
  emits: ['refresh']
}
</script>