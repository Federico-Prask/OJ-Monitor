import axios from 'axios'

const LUOGU_API_BASE = 'https://www.luogu.com.cn'

export const luoguService = {
  async getUserStats(userId) {
    // 这里实现洛谷数据获取逻辑
    // 由于跨域限制，可能需要后端代理
    try {
      // 模拟API调用
      const response = await axios.get(`${LUOGU_API_BASE}/api/user/${userId}`)
      return this.parseStats(response.data)
    } catch (error) {
      console.error('Failed to fetch Luogu stats:', error)
      throw error
    }
  },

  async getRecentRecords(userId, limit = 10) {
    try {
      // 模拟API调用
      const response = await axios.get(`${LUOGU_API_BASE}/api/record/${userId}?limit=${limit}`)
      return this.parseRecords(response.data)
    } catch (error) {
      console.error('Failed to fetch Luogu records:', error)
      throw error
    }
  },

  parseStats(data) {
    // 解析洛谷API返回的数据
    return {
      solved: data.passedProblemCount || 0,
      submissions: data.submittedProblemCount || 0,
      accuracy: data.acRate || 0
    }
  },

  parseRecords(data) {
    // 解析做题记录
    return data.records?.map(record => ({
      problem: record.problem.title,
      id: record.problem.pid,
      difficulty: this.mapDifficulty(record.problem.difficulty),
      time: new Date(record.submitTime * 1000).toLocaleString('zh-CN'),
      status: record.status === 12 ? '通过' : '未通过'
    })) || []
  },

  mapDifficulty(difficulty) {
    const difficultyMap = {
      0: '入门',
      1: '普及-',
      2: '普及/提高-',
      3: '普及+/提高',
      4: '提高+/省选-',
      5: '省选/NOI-',
      6: 'NOI/NOI+/CTSC'
    }
    return difficultyMap[difficulty] || '未知'
  }
}