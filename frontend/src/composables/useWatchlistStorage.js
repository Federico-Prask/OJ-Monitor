/**
 * 监视列表本地存储管理（基于 Cookie）
 */

export const useWatchlistStorage = () => {
  const STORAGE_KEY = 'oj_monitor_watchlists'

  // 获取所有监视列表
  const getWatchlists = () => {
    try {
      const stored = localStorage.getItem(STORAGE_KEY)
      return stored ? JSON.parse(stored) : {}
    } catch (error) {
      console.error('Error reading watchlists from storage:', error)
      return {}
    }
  }

  // 创建新的监视列表
  const createWatchlist = (name, usernames = []) => {
    const watchlists = getWatchlists()
    const id = Date.now().toString()
    
    watchlists[id] = {
      id,
      name,
      usernames,
      createdAt: new Date().toISOString(),
      isRunning: false
    }
    
    localStorage.setItem(STORAGE_KEY, JSON.stringify(watchlists))
    return watchlists[id]
  }

  // 删除监视列表
  const deleteWatchlist = (id) => {
    const watchlists = getWatchlists()
    delete watchlists[id]
    localStorage.setItem(STORAGE_KEY, JSON.stringify(watchlists))
  }

  // 更新监视列表
  const updateWatchlist = (id, updates) => {
    const watchlists = getWatchlists()
    if (watchlists[id]) {
      watchlists[id] = { ...watchlists[id], ...updates }
      localStorage.setItem(STORAGE_KEY, JSON.stringify(watchlists))
      return watchlists[id]
    }
    return null
  }

  // 添加用户到监视列表
  const addUserToWatchlist = (id, username) => {
    const watchlists = getWatchlists()
    if (watchlists[id] && !watchlists[id].usernames.includes(username)) {
      watchlists[id].usernames.push(username)
      localStorage.setItem(STORAGE_KEY, JSON.stringify(watchlists))
    }
  }

  // 从监视列表移除用户
  const removeUserFromWatchlist = (id, username) => {
    const watchlists = getWatchlists()
    if (watchlists[id]) {
      watchlists[id].usernames = watchlists[id].usernames.filter(u => u !== username)
      localStorage.setItem(STORAGE_KEY, JSON.stringify(watchlists))
    }
  }

  return {
    getWatchlists,
    createWatchlist,
    deleteWatchlist,
    updateWatchlist,
    addUserToWatchlist,
    removeUserFromWatchlist
  }
}
