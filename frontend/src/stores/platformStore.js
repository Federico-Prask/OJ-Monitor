import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const usePlatformStore = defineStore('platform', () => {
  const platforms = ref([])
  const activePlatformIndex = ref(0)
  const loading = ref(false)

  const activePlatform = computed(() => {
    return platforms.value[activePlatformIndex.value] || getDefaultPlatform()
  })

  const initializePlatforms = () => {
    // 使用模拟数据，不调用API
    platforms.value = getDefaultPlatforms()
  }

  const setActivePlatform = (index) => {
    if (index >= 0 && index < platforms.value.length) {
      activePlatformIndex.value = index
    }
  }

  const fetchPlatformData = async (platformId, username = 'demo') => {
    const platform = platforms.value.find(p => p.id === platformId)
    if (!platform) return

    platform.loading = true
    
    // 模拟API调用延迟
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    // 使用模拟数据
    platform.stats = getMockStats(platformId)
    platform.recentRecords = getMockRecords(platformId)
    platform.loading = false
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
    const baseRecords = {
      luogu: [
        { problem: 'A+B Problem', id: 'P1001', difficulty: '简单', time: '2023-07-15 14:23', status: '通过' },
        { problem: '斐波那契数列', id: 'P1002', difficulty: '简单', time: '2023-07-14 09:45', status: '通过' }
      ],
      lsyoi: [
        { problem: '两数之和', id: 'T1001', difficulty: '简单', time: '2023-07-15 13:15', status: '通过' },
        { problem: '字符串反转', id: 'T1002', difficulty: '简单', time: '2023-07-14 10:22', status: '通过' }
      ],
      oiclass: [
        { problem: '冒泡排序', id: 'C1001', difficulty: '简单', time: '2023-07-15 16:45', status: '通过' },
        { problem: '二分查找', id: 'C1002', difficulty: '简单', time: '2023-07-14 12:33', status: '通过' }
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
      {
        id: 'lsyoi',
        name: 'LSYOJ',
        icon: 'fas fa-laptop-code',
        color: '#3b82f6',
        description: '立山中学在线评测系统',
        baseUrl: 'http://lsyoi.top:81',
        stats: getMockStats('lsyoi'),
        recentRecords: getMockRecords('lsyoi')
      },
      {
        id: 'oiclass',
        name: 'OI Class',
        icon: 'fas fa-graduation-cap',
        color: '#8b5cf6',
        description: '信息学竞赛在线学习平台',
        baseUrl: 'https://oiclass.com',
        stats: getMockStats('oiclass'),
        recentRecords: getMockRecords('oiclass')
      }
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

  // 立即初始化
  initializePlatforms()

  return {
    platforms,
    activePlatformIndex,
    activePlatform,
    loading,
    initializePlatforms,
    setActivePlatform,
    fetchPlatformData
  }
})