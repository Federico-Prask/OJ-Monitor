<template>
  <div class="bg-gray-50 p-6 rounded-xl mb-8">
    <div class="flex justify-between items-center mb-4">
      <h3 class="text-xl font-bold text-gray-800">做题趋势</h3>
      <div class="flex space-x-2">
        <button class="px-3 py-1 bg-white rounded-lg text-sm">周</button>
        <button class="px-3 py-1 bg-indigo-600 text-white rounded-lg text-sm">月</button>
        <button class="px-3 py-1 bg-white rounded-lg text-sm">年</button>
      </div>
    </div>
    <div class="h-64 bg-white rounded-lg p-4 flex items-end justify-between">
      <div v-for="(item, index) in chartData" :key="index" class="flex flex-col items-center">
        <div class="text-xs text-gray-500 mb-1">{{ item.day }}</div>
        <div 
          class="w-8 rounded-t-md transition-all duration-500" 
          :style="{ height: item.value * 2 + 'px', backgroundColor: platform.color }"
        ></div>
        <div class="text-xs mt-1">{{ item.value }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'

export default {
  name: 'TrendChart',
  props: {
    platform: {
      type: Object,
      required: true
    }
  },
  setup() {
    const chartData = ref([
      { day: '周一', value: 12 },
      { day: '周二', value: 19 },
      { day: '周三', value: 8 },
      { day: '周四', value: 15 },
      { day: '周五', value: 22 },
      { day: '周六', value: 18 },
      { day: '周日', value: 14 }
    ])

    return {
      chartData
    }
  }
}
</script>