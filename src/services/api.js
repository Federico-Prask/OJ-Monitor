// src/services/api.js
import axios from 'axios'

const API_BASE_URL = 'http://localhost:5000/api'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const platformService = {
  async getPlatforms() {
    const response = await apiClient.get('/platforms')
    return response.data
  },

  async getUserStats(platformId, username) {
    const response = await apiClient.get(`/platforms/${platformId}/users/${username}/stats`)
    return response.data
  },

  async getRecentSubmissions(platformId, username, limit = 10) {
    const response = await apiClient.get(`/platforms/${platformId}/users/${username}/submissions?limit=${limit}`)
    return response.data
  },

  async validateUser(platformId, username) {
    const response = await apiClient.get(`/platforms/${platformId}/users/${username}/validate`)
    return response.data
  }
}