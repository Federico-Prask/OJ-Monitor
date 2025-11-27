import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { PLATFORMS } from '../utils/constants'

export const usePlatformStore = defineStore('platform', () => {
  const platforms = ref([])
  const activePlatformIndex = ref(0)

  // 添加 activePlatform 计算属性
  const activePlatform = computed(() => {
    return platforms.value[activePlatformIndex.value] || platforms.value[0] || getDefaultPlatform()
  })

  const initializePlatforms = () => {
    platforms.value = PLATFORMS.map(platform => ({
      ...platform,
      stats: getMockStats(platform.id),
      recentRecords: getMockRecords(platform.id),
      loading: false
    }))
  }

  const setActivePlatform = (index) => {
    if (index >= 0 && index < platforms.value.length) {
      activePlatformIndex.value = index
    }
  }

  const fetchPlatformData = async (platformId) => {
    const platform = platforms.value.find(p => p.id === platformId)
    if (!platform) return

    platform.loading = true
    
    // 模拟API调用延迟
    await new Promise(resolve => setTimeout(resolve, 1000))
    
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
        { problem: '斐波那契数列', id: 'P1002', difficulty: '简单', time: '2023-07-14 09:45', status: '通过' },
        { problem: '最大子段和', id: 'P1003', difficulty: '中等', time: '2023-07-13 16:12', status: '通过' },
        { problem: '最短路径', id: 'P1004', difficulty: '困难', time: '2023-07-12 20:33', status: '未通过' },
        { problem: '快速排序', id: 'P1005', difficulty: '中等', time: '2023-07-11 11:27', status: '通过' }
      ],
      lsyoi: [
        { problem: '两数之和', id: 'T1001', difficulty: '简单', time: '2023-07-15 13:15', status: '通过' },
        { problem: '字符串反转', id: 'T1002', difficulty: '简单', time: '2023-07-14 10:22', status: '通过' },
        { problem: '二叉树遍历', id: 'T1003', difficulty: '中等', time: '2023-07-13 15:41', status: '通过' },
        { problem: '动态规划入门', id: 'T1004', difficulty: '中等', time: '2023-07-12 19:05', status: '未通过' },
        { problem: '图论基础', id: 'T1005', difficulty: '困难', time: '2023-07-11 14:18', status: '通过' }
      ],
      oiclass: [
        { problem: '冒泡排序', id: 'C1001', difficulty: '简单', time: '2023-07-15 16:45', status: '通过' },
        { problem: '二分查找', id: 'C1002', difficulty: '简单', time: '2023-07-14 12:33', status: '通过' },
        { problem: '背包问题', id: 'C1003', difficulty: '中等', time: '2023-07-13 18:22', status: '未通过' },
        { problem: '最短路径算法', id: 'C1004', difficulty: '困难', time: '2023-07-12 21:15', status: '通过' },
        { problem: '线段树应用', id: 'C1005', difficulty: '困难', time: '2023-07-11 17:09', status: '未通过' }
      ]
    }
    return baseRecords[platformId] || []
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
    initializePlatforms,
    setActivePlatform,
    fetchPlatformData
  }
})