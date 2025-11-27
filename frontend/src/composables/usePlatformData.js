import { ref } from 'vue'
import { 
  luoguService,
  lsyoiService, 
  oiclassService 
} from '../services/api'

export function usePlatformData() {
  const loading = ref(false)
  const error = ref(null)

  const fetchPlatformStats = async (platformId) => {
    loading.value = true
    error.value = null

    try {
      switch (platformId) {
        case 'luogu':
          return await luoguService.getUserStats()
        case 'lsyoi':
          return await lsyoiService.getUserStats()
        case 'oiclass':
          return await oiclassService.getUserStats()
        default:
          throw new Error(`Unknown platform: ${platformId}`)
      }
    } catch (err) {
      error.value = err.message
      // 返回模拟数据作为fallback
      return getMockStats(platformId)
    } finally {
      loading.value = false
    }
  }

  const fetchRecentRecords = async (platformId) => {
    try {
      switch (platformId) {
        case 'luogu':
          return await luoguService.getRecentRecords()
        case 'lsyoi':
          return await lsyoiService.getRecentRecords()
        case 'oiclass':
          return await oiclassService.getRecentRecords()
        default:
          throw new Error(`Unknown platform: ${platformId}`)
      }
    } catch (err) {
      error.value = err.message
      // 返回模拟数据作为fallback
      return getMockRecords(platformId)
    }
  }

  const getMockStats = (platformId) => {
    const baseStats = {
      luogu: { solved: 156, submissions: 423, accuracy: 78 },
      lsyoi: { solved: 89, submissions: 201, accuracy: 82 },
      oiclass: { solved: 124, submissions: 298, accuracy: 75 }
    }
    return baseStats[platformId] || { solved: 0, submissions: 0, accuracy: 0 }
  }

  const getMockRecords = (platformId) => {
    // 返回模拟的做题记录
    return []
  }

  return {
    loading,
    error,
    fetchPlatformStats,
    fetchRecentRecords
  }
}