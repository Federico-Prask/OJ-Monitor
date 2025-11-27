<template>
  <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
    <div class="bg-gradient-to-r from-blue-50 to-indigo-50 p-6 rounded-xl border border-blue-100">
      <div class="flex justify-between items-center">
        <div>
          <p class="text-gray-600">已解决题目</p>
          <h3 class="text-3xl font-bold text-gray-800 mt-2">{{ stats.solved }}</h3>
        </div>
        <div class="w-12 h-12 rounded-full bg-blue-100 flex items-center justify-center">
          <i class="fas fa-check text-blue-600 text-xl"></i>
        </div>
      </div>
      <p class="text-sm text-gray-500 mt-4">较上周 <span class="text-green-500 font-medium">+12</span></p>
    </div>

    <div class="bg-gradient-to-r from-green-50 to-emerald-50 p-6 rounded-xl border border-green-100">
      <div class="flex justify-between items-center">
        <div>
          <p class="text-gray-600">提交次数</p>
          <h3 class="text-3xl font-bold text-gray-800 mt-2">{{ stats.submissions }}</h3>
        </div>
        <div class="w-12 h-12 rounded-full bg-green-100 flex items-center justify-center">
          <i class="fas fa-upload text-green-600 text-xl"></i>
        </div>
      </div>
      <p class="text-sm text-gray-500 mt-4">较上周 <span class="text-green-500 font-medium">+24</span></p>
    </div>

    <div class="bg-gradient-to-r from-purple-50 to-pink-50 p-6 rounded-xl border border-purple-100">
      <div class="flex justify-between items-center">
        <div>
          <p class="text-gray-600">正确率</p>
          <h3 class="text-3xl font-bold text-gray-800 mt-2">{{ stats.accuracy }}%</h3>
        </div>
        <div class="w-12 h-12 rounded-full bg-purple-100 flex items-center justify-center">
          <i class="fas fa-chart-line text-purple-600 text-xl"></i>
        </div>
      </div>
      <p class="text-sm text-gray-500 mt-4">较上周 <span class="text-green-500 font-medium">+3%</span></p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'StatsCards',
  props: {
    stats: {
      type: Object,
      required: true,
      default: () => ({
        solved: 0,
        submissions: 0,
        accuracy: 0
      })
    }
  }
}
</script>