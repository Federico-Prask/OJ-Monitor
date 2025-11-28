import apiClient from '../api'

/**
 * 监视列表 API 服务
 */
export const watchlistService = {
  /**
   * 启动监视列表爬虫
   * @param {Array<string>} usernames 用户名列表
   * @param {number} intervalSeconds 轮询间隔（秒）
   */
  async startWatchlist(usernames, intervalSeconds = 5) {
    try {
      const response = await apiClient.post('/watchlist/start', {
        usernames,
        intervalSeconds
      })
      return response.data
    } catch (error) {
      console.error('Failed to start watchlist:', error)
      throw error
    }
  },

  /**
   * 停止监视列表爬虫
   */
  async stopWatchlist() {
    try {
      const response = await apiClient.post('/watchlist/stop')
      return response.data
    } catch (error) {
      console.error('Failed to stop watchlist:', error)
      throw error
    }
  },

  /**
   * 获取未通知的新提交
   */
  async getNotifications() {
    try {
      const response = await apiClient.get('/watchlist/notifications')
      return response.data || []
    } catch (error) {
      console.error('Failed to get notifications:', error)
      return []
    }
  },

  /**
   * 标记提交为已通知
   */
  async markNotified(username, problemId) {
    try {
      const response = await apiClient.post('/watchlist/mark-notified', {
        username,
        problemId
      })
      return response.data
    } catch (error) {
      console.error('Failed to mark as notified:', error)
      throw error
    }
  }
}
