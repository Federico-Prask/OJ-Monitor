// src/stores/platformStore.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { platformService } from '../services/api'

export const usePlatformStore = defineStore('platform', () => {
  const platforms = ref([])
  const activePlatformIndex = ref(0)
  const userData = ref({}) // 存储用户数据 { [platformId]: { stats, recentRecords } }

  const activePlatform = computed(() => {
    return platforms.value[activePlatformIndex.value] || platforms.value[0] || getDefaultPlatform()
  })

  const initializePlatforms = async () => {
    try {
      platforms.value = await platformService.getPlatforms()
    } catch (error) {
      console.error('Failed to fetch platforms:', error)
      // 使用默认平台作为后备
      platforms.value = getDefaultPlatforms()
    }
  }

  const setActivePlatform = (index) => {
    if (index >= 0 && index < platforms.value.length) {
      activePlatformIndex.value = index
    }
  }

  const fetchPlatformData = async (platformId, username) => {
    const platform = platforms.value.find(p => p.id === platformId)
    if (!platform) return

    platform.loading = true
    
    try {
      const [stats, records] = await Promise.all([
        platformService.getUserStats(platformId, username),
        platformService.getRecentSubmissions(platformId, username, 10)
      ])
      
      // 更新用户数据
      if (!userData.value[platformId]) {
        userData.value[platformId] = {}
      }
      userData.value[platformId].stats = stats
      userData.value[platformId].recentRecords = records
      
      // 更新平台数据
      platform.stats = stats
      platform.recentRecords = records
    } catch (error) {
      console.error(`Failed to fetch data for ${platformId}:`, error)
      // 使用模拟数据作为后备
      platform.stats = getMockStats(platformId)
      platform.recentRecords = getMockRecords(platformId)
    } finally {
      platform.loading = false
    }
  }

  // 其他辅助函数保持不变...
  const getMockStats = (platformId) => {
    const baseStats = {
      luogu: { solved: 156, submissions: 423, accuracy: 78 },
      lsyoi: { solved: 89, submissions: 201, accuracy: 82 },
      oiclass: { solved: 124, submissions: 298, accuracy: 75 }
    }
    return baseStats[platformId] || { solved: 0, submissions: 0, accuracy: 0 }
  }

  const getMockRecords = (platformId) => {
    const baseRecords = {
      luogu: [
        { problem: 'A+B Problem', id: 'P1001', difficulty: '简单', time: '2023-07-15 14:23', status: '通过' },
        // ... 其他记录
      ],
      lsyoi: [
        { problem: '两数之和', id: 'T1001', difficulty: '简单', time: '2023-07-15 13:15', status: '通过' },
        // ... 其他记录
      ],
      oiclass: [
        { problem: '冒泡排序', id: 'C1001', difficulty: '简单', time: '2023-07-15 16:45', status: '通过' },
        // ... 其他记录
      ]
    }
    return baseRecords[platformId] || []
  }

  const getDefaultPlatforms = () => {
    return [
      {
        id: 'luogu',
        name: '洛谷',
        icon: 'fas fa-leaf',
        color: '#1a9c55',
        description: '国内知名的算法竞赛社区',
        baseUrl: 'https://www.luogu.com.cn',
        stats: getMockStats('luogu'),
        recentRecords: getMockRecords('luogu')
      },
      // ... 其他平台
    ]
  }

  const getDefaultPlatform = () => {
    return {
      id: 'default',
      name: '默认平台',
      icon: 'fas fa-question',
      color: '#6b7280',
      description: '请选择平台',
      stats: { solved: 0, submissions: 0, accuracy: 0 },
      recentRecords: []
    }
  }

  // 立即初始化平台数据
  initializePlatforms()

  return {
    platforms,
    activePlatformIndex,
    activePlatform,
    userData,
    initializePlatforms,
    setActivePlatform,
    fetchPlatformData
  }
})