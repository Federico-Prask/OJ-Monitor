<template>
  <footer class="bg-white border-t border-gray-200 mt-12">
    <div class="container mx-auto px-4 py-6">
      <div class="text-center text-gray-600">
        <p>数据更新于 {{ currentTime }}</p>
        <p class="mt-2 text-sm">本工具仅用于学习目的，数据获取请遵守各平台相关规定</p>
      </div>
    </div>
  </footer>
</template>

<script>
import { ref, onMounted } from 'vue'

export default {
  name: 'Footer',
  setup() {
    const currentTime = ref('')

    const updateTime = () => {
      const now = new Date()
      currentTime.value = now.toLocaleString('zh-CN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      })
    }

    onMounted(() => {
      updateTime()
      setInterval(updateTime, 60000) // 每分钟更新一次
    })

    return {
      currentTime
    }
  }
}
</script>