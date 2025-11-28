<template>
  <div class="watchlist-container p-6 bg-white rounded-lg shadow-lg">
    <h2 class="text-2xl font-bold mb-6 text-gray-800">🎯 监视列表管理</h2>

    <!-- 创建新列表 -->
    <div class="mb-8 p-4 bg-gray-50 rounded-lg">
      <h3 class="text-lg font-semibold mb-4">创建新的监视列表</h3>
      <div class="space-y-4">
        <input
          v-model="newListName"
          type="text"
          placeholder="输入监视列表名称"
          class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <div class="flex gap-2">
          <input
            v-model="newUsername"
            type="text"
            placeholder="输入用户名"
            class="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            @keyup.enter="addNewUsername"
          />
          <button
            @click="addNewUsername"
            class="px-4 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition"
          >
            添加用户
          </button>
        </div>
        <div class="flex flex-wrap gap-2">
          <span
            v-for="(user, idx) in tempUsernames"
            :key="idx"
            class="px-3 py-1 bg-blue-100 text-blue-800 rounded-full text-sm flex items-center gap-2"
          >
            {{ user }}
            <button @click="removeTempUser(idx)" class="text-red-500 font-bold">×</button>
          </span>
        </div>
        <button
          @click="createNewWatchlist"
          :disabled="!newListName || tempUsernames.length === 0"
          class="w-full px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600 transition disabled:bg-gray-400"
        >
          创建监视列表
        </button>
      </div>
    </div>

    <!-- 监视列表列表 -->
    <div class="space-y-4">
      <div
        v-for="(watchlist, id) in watchlists"
        :key="id"
        class="p-4 border border-gray-300 rounded-lg bg-gray-50"
      >
        <div class="flex justify-between items-start mb-4">
          <div>
            <h4 class="text-lg font-semibold text-gray-800">{{ watchlist.name }}</h4>
            <p class="text-sm text-gray-500">
              创建时间: {{ new Date(watchlist.createdAt).toLocaleString() }}
            </p>
          </div>
          <div class="flex gap-2">
            <button
              v-if="!watchlist.isRunning"
              @click="startWatchlist(id, watchlist.usernames)"
              class="px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600 transition"
            >
              ▶️ 启动
            </button>
            <button
              v-else
              @click="stopWatchlist(id)"
              class="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
            >
              ⏹️ 停止
            </button>
            <button
              @click="deleteWatchlistConfirm(id)"
              class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
            >
              🗑️ 删除
            </button>
          </div>
        </div>

        <!-- 用户列表 -->
        <div class="mb-4">
          <p class="text-sm font-semibold text-gray-700 mb-2">监视用户 ({{ watchlist.usernames.length }})</p>
          <div class="flex flex-wrap gap-2">
            <span
              v-for="(user, idx) in watchlist.usernames"
              :key="idx"
              class="px-3 py-1 bg-blue-100 text-blue-800 rounded-full text-sm flex items-center gap-2"
            >
              {{ user }}
              <button
                @click="removeUser(id, user)"
                class="text-red-500 font-bold hover:text-red-700"
              >
                ×
              </button>
            </span>
            <input
              :id="`user-input-${id}`"
              type="text"
              placeholder="输入新用户名"
              class="px-3 py-1 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              @keyup.enter="addUser(id, $event)"
            />
          </div>
        </div>

        <!-- 状态指示 -->
        <div class="text-sm">
          <span v-if="watchlist.isRunning" class="text-green-600 font-semibold">
            ✓ 正在运行中...
          </span>
          <span v-else class="text-gray-500">
            已停止
          </span>
        </div>
      </div>

      <p v-if="Object.keys(watchlists).length === 0" class="text-gray-500 text-center py-8">
        还没有创建任何监视列表，创建一个开始监视吧！
      </p>
    </div>

    <!-- 通知面板 -->
    <div v-if="notifications.length > 0" class="mt-8 p-4 bg-yellow-50 border border-yellow-300 rounded-lg">
      <h3 class="text-lg font-semibold mb-4 text-yellow-800">🔔 新的提交通知</h3>
      <div class="space-y-3">
        <div
          v-for="(notification, idx) in notifications"
          :key="idx"
          class="p-3 bg-white border border-yellow-200 rounded-lg"
        >
          <p class="font-semibold text-gray-800">
            {{ notification.username }} 
            <span
              :style="{ color: getDifficultyColor(notification.difficulty) }"
              class="font-bold"
            >
              卷了{{ difficultyNames[notification.difficulty] }}题
            </span>
          </p>
          <p class="text-gray-700">
            <a
              :href="`https://www.luogu.com.cn/problem/${notification.problemId}`"
              target="_blank"
              class="text-blue-500 hover:underline"
            >
              {{ notification.problemId }}: {{ notification.problemTitle }}
            </a>
          </p>
          <p class="text-sm text-gray-500">
            {{ new Date(notification.submitTime).toLocaleString() }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, onUnmounted } from 'vue'
import { useWatchlistStorage } from '@/composables/useWatchlistStorage'
import { watchlistService } from '@/services/watchlistService'

const storage = useWatchlistStorage()

const watchlists = reactive(storage.getWatchlists())
const notifications = ref([])
const newListName = ref('')
const newUsername = ref('')
const tempUsernames = ref([])

let notificationInterval = null
let activeWatchlistId = null

// 颜色映射
const difficultyColors = [
  'rgb(191, 191, 191)', // 暂无评定 - 灰
  'rgb(254, 76, 97)',   // 入门 - 红
  'rgb(243, 156, 17)',  // 普及− - 橙
  'rgb(255, 193, 22)',  // 普及/提高− - 黄
  'rgb(82, 196, 26)',   // 普及+/提高 - 绿
  'rgb(52, 152, 219)',  // 提高+/省选− - 蓝
  'rgb(157, 61, 207)',  // 省选/NOI− - 紫
  'rgb(14, 29, 105)'    // NOI/NOI+/CTSC - 黑
]

const difficultyNames = [
  '暂无评定',
  '入门',
  '普及−',
  '普及/提高−',
  '普及+/提高',
  '提高+/省选−',
  '省选/NOI−',
  'NOI/NOI+/CTSC'
]

const getDifficultyColor = (difficulty) => {
  return difficultyColors[difficulty] || 'gray'
}

// 创建新的监视列表
const createNewWatchlist = () => {
  if (!newListName.value || tempUsernames.value.length === 0) {
    alert('请输入列表名称和至少一个用户名')
    return
  }

  const watchlist = storage.createWatchlist(newListName.value, [...tempUsernames.value])
  watchlists[watchlist.id] = watchlist

  newListName.value = ''
  tempUsernames.value = []
  alert('监视列表创建成功！')
}

// 添加临时用户
const addNewUsername = () => {
  const username = newUsername.value.trim()
  if (username && !tempUsernames.value.includes(username)) {
    tempUsernames.value.push(username)
    newUsername.value = ''
  }
}

// 移除临时用户
const removeTempUser = (idx) => {
  tempUsernames.value.splice(idx, 1)
}

// 启动监视列表
const startWatchlist = async (id, usernames) => {
  try {
    await watchlistService.startWatchlist(usernames, 5)
    storage.updateWatchlist(id, { isRunning: true })
    watchlists[id].isRunning = true
    activeWatchlistId = id

    // 开始轮询通知
    notificationInterval = setInterval(fetchNotifications, 3000)
    alert('监视列表已启动！')
  } catch (error) {
    alert('启动失败: ' + error.message)
  }
}

// 停止监视列表
const stopWatchlist = async (id) => {
  try {
    await watchlistService.stopWatchlist()
    storage.updateWatchlist(id, { isRunning: false })
    watchlists[id].isRunning = false
    activeWatchlistId = null

    if (notificationInterval) {
      clearInterval(notificationInterval)
      notificationInterval = null
    }
    alert('监视列表已停止')
  } catch (error) {
    alert('停止失败: ' + error.message)
  }
}

// 删除监视列表
const deleteWatchlistConfirm = (id) => {
  if (confirm('确定要删除这个监视列表吗？')) {
    storage.deleteWatchlist(id)
    delete watchlists[id]
  }
}

// 添加用户
const addUser = async (id, event) => {
  const input = event.target
  const username = input.value.trim()

  if (username) {
    storage.addUserToWatchlist(id, username)
    watchlists[id].usernames.push(username)
    input.value = ''
  }
}

// 移除用户
const removeUser = (id, username) => {
  storage.removeUserFromWatchlist(id, username)
  watchlists[id].usernames = watchlists[id].usernames.filter(u => u !== username)
}

// 获取通知
const fetchNotifications = async () => {
  try {
    const newNotifications = await watchlistService.getNotifications()
    if (newNotifications.length > 0) {
      notifications.value = [...newNotifications, ...notifications.value].slice(0, 50)

      // 浏览器通知
      newNotifications.forEach(notif => {
        if (Notification.permission === 'granted') {
          new Notification(`${notif.username} 卷了${notif.difficultyName}题！`, {
            body: `${notif.problemId}: ${notif.problemTitle}`,
            icon: '🎯'
          })
        }
      })
    }
  } catch (error) {
    console.error('Failed to fetch notifications:', error)
  }
}

onMounted(() => {
  // 请求浏览器通知权限
  if ('Notification' in window && Notification.permission === 'default') {
    Notification.requestPermission()
  }
})

onUnmounted(() => {
  if (notificationInterval) {
    clearInterval(notificationInterval)
  }
})
</script>

<style scoped>
.watchlist-container {
  max-width: 1000px;
  margin: 0 auto;
}
</style>
