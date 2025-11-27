<template>
  <div v-if="showDebug" class="fixed bottom-4 right-4 bg-black bg-opacity-80 text-white p-4 rounded-lg text-xs z-50 max-w-sm">
    <div class="font-bold mb-2">调试信息</div>
    <div>当前平台: {{ currentPlatform }}</div>
    <div>平台数量: {{ platformCount }}</div>
    <div>CSS 加载: {{ cssLoaded ? '成功' : '失败' }}</div>
    <div>字体加载: {{ fontsLoaded ? '成功' : '失败' }}</div>
    <button @click="showDebug = false" class="mt-2 text-red-400">关闭</button>
  </div>
  <button 
    v-else
    @click="showDebug = true"
    class="fixed bottom-4 right-4 bg-gray-800 text-white p-2 rounded-full text-xs z-50"
  >
    调试
  </button>
</template>

<script>
import { ref, onMounted, computed } from 'vue'
import { usePlatformStore } from '../../stores/platformStore'

export default {
  name: 'DebugInfo',
  setup() {
    const showDebug = ref(false)
    const cssLoaded = ref(false)
    const fontsLoaded = ref(false)
    const platformStore = usePlatformStore()

    const currentPlatform = computed(() => platformStore.activePlatform?.name || '未知')
    const platformCount = computed(() => platformStore.platforms.length)

    onMounted(() => {
      // 检查CSS是否加载
      cssLoaded.value = document.styleSheets.length > 0
      
      // 检查字体是否加载
      const checkFonts = () => {
        const testElement = document.createElement('span')
        testElement.style.fontFamily = 'FontAwesome'
        testElement.style.position = 'absolute'
        testElement.style.left = '-9999px'
        testElement.innerHTML = '&#xf021;' // Font Awesome sync icon
        document.body.appendChild(testElement)
        
        setTimeout(() => {
          const width1 = testElement.offsetWidth
          testElement.style.fontFamily = 'Arial'
          const width2 = testElement.offsetWidth
          fontsLoaded.value = width1 !== width2
          document.body.removeChild(testElement)
        }, 100)
      }
      
      setTimeout(checkFonts, 1000)
    })

    return {
      showDebug,
      cssLoaded,
      fontsLoaded,
      currentPlatform,
      platformCount
    }
  }
}
</script>