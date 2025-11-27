import { defineStore } from 'pinia'
import { ref } from 'vue'
import { PLATFORMS } from '../utils/constants'
import { usePlatformData } from '../composables/usePlatformData'

export const usePlatformStore = defineStore('platform', () => {
  const platforms = ref([])
  const activePlatformIndex = ref(0)

  const { fetchPlatformStats, fetchRecentRecords } = usePlatformData()

  const initializePlatforms = () => {
    platforms.value = PLATFORMS.map(platform => ({
      ...platform,
      stats: {
        solved: 0,
        submissions: 0,
        accuracy: 0
      },
      recentRecords: [],
      loading: false
    }))
  }

  const setActivePlatform = (index) => {
    activePlatformIndex.value = index
  }

  const fetchPlatformData = async (platformId) => {
    const platform = platforms.value.find(p => p.id === platformId)
    if (!platform) return

    platform.loading = true
    
    try {
      const [stats, records] = await Promise.all([
        fetchPlatformStats(platformId),
        fetchRecentRecords(platformId)
      ])
      
      platform.stats = stats
      platform.recentRecords = records
    } catch (error) {
      console.error(`Failed to fetch data for ${platformId}:`, error)
    } finally {
      platform.loading = false
    }
  }

  return {
    platforms,
    activePlatformIndex,
    initializePlatforms,
    setActivePlatform,
    fetchPlatformData
  }
})