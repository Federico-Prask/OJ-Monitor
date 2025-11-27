import axios from 'axios'

// 动态获取当前主机名来构建API URL
const getApiBaseUrl = () => {
  if (import.meta.env.DEV) {
    // 开发环境：使用代理或当前域名的5000端口
    const hostname = window.location.hostname;
    if (hostname.includes('github.dev')) {
      // 在 GitHub Codespaces 中
      const codespaceName = hostname.split('.')[0];
      return `https://${codespaceName}-5000.app.github.dev/api`;
    }
    return '/api'; // 使用代理
  }
  // 生产环境：使用相对路径或配置的URL
  return import.meta.env.VITE_API_BASE_URL || '/api';
}

const API_BASE_URL = getApiBaseUrl();

console.log('API Base URL:', API_BASE_URL);

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 15000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// 请求拦截器
apiClient.interceptors.request.use(
  (config) => {
    console.log(`Making ${config.method?.toUpperCase()} request to: ${config.url}`)
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// 响应拦截器
apiClient.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    console.error('API Error:', error)
    // 如果API调用失败，返回模拟数据
    if (error.config.url.includes('/api/platforms')) {
      return Promise.resolve({
        data: getMockPlatforms()
      })
    }
    return Promise.reject(error)
  }
)

// 模拟平台数据
function getMockPlatforms() {
  return [
    {
      id: 'luogu',
      name: '洛谷',
      icon: 'fas fa-leaf',
      color: '#1a9c55',
      description: '国内知名的算法竞赛社区',
      baseUrl: 'https://www.luogu.com.cn',
      isEnabled: true
    },
    {
      id: 'lsyoi',
      name: 'LSYOJ',
      icon: 'fas fa-laptop-code',
      color: '#3b82f6',
      description: 'LSYOI在线评测系统',
      baseUrl: 'http://lsyoi.top:81',
      isEnabled: true
    },
    {
      id: 'oiclass',
      name: 'OI Class',
      icon: 'fas fa-graduation-cap',
      color: '#8b5cf6',
      description: '信息学竞赛在线学习平台',
      baseUrl: 'https://oiclass.com',
      isEnabled: true
    }
  ]
}

export const platformService = {
  async getPlatforms() {
    try {
      const response = await apiClient.get('/platforms')
      return response.data
    } catch (error) {
      console.error('Failed to fetch platforms, using mock data:', error)
      return getMockPlatforms()
    }
  },

  async getUserStats(platformId, username) {
    try {
      const response = await apiClient.get(`/platforms/${platformId}/users/${username}/stats`)
      return response.data
    } catch (error) {
      console.error(`Failed to fetch stats for ${platformId}, using mock data:`, error)
      return getMockStats(platformId)
    }
  },

  async getRecentSubmissions(platformId, username, limit = 10) {
    try {
      const response = await apiClient.get(`/platforms/${platformId}/users/${username}/submissions?limit=${limit}`)
      return response.data
    } catch (error) {
      console.error(`Failed to fetch submissions for ${platformId}, using mock data:`, error)
      return getMockSubmissions(platformId)
    }
  },

  async validateUser(platformId, username) {
    try {
      const response = await apiClient.get(`/platforms/${platformId}/users/${username}/validate`)
      return response.data
    } catch (error) {
      console.error(`Failed to validate user for ${platformId}:`, error)
      return true // 默认返回true
    }
  }
}

// 模拟数据函数
function getMockStats(platformId) {
  const baseStats = {
    luogu: { solved: 156, submissions: 423, accuracy: 78 },
    lsyoi: { solved: 89, submissions: 201, accuracy: 82 },
    oiclass: { solved: 124, submissions: 298, accuracy: 75 }
  }
  return baseStats[platformId] || { solved: 0, submissions: 0, accuracy: 0 }
}

function getMockSubmissions(platformId) {
  const baseSubmissions = {
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
  return baseSubmissions[platformId] || []
}

export default apiClient