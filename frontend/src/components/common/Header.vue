<template>
  <header class="bg-white shadow-sm">
    <div class="container mx-auto px-4 py-6">
      <div class="flex items-center justify-between">
        <div class="flex items-center space-x-4">
          <div class="w-10 h-10 bg-indigo-600 rounded-lg flex items-center justify-center">
            <i class="fas fa-chart-line text-white"></i>
          </div>
          <div>
            <h1 class="text-2xl font-bold text-gray-800">多平台做题情况监测</h1>
            <p class="text-gray-600 text-sm">实时追踪你的编程练习进度</p>
          </div>
        </div>
        <div class="flex items-center space-x-4">
          <button class="px-4 py-2 text-gray-600 hover:text-indigo-600 transition-colors">
            <i class="fas fa-cog mr-2"></i>设置
          </button>
          <button class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors">
            <i class="fas fa-user mr-2"></i>登录
          </button>
        </div>
      </div>
    </div>
  </header>
</template>

<script>
export default {
  name: 'Header'
}
</script>