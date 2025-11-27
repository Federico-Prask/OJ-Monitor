import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import './assets/styles/global.css'

// 添加调试信息
console.log('Vue app starting...')

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)

// 确保挂载元素存在
const appElement = document.getElementById('app')
if (appElement) {
  app.mount('#app')
  console.log('Vue app mounted successfully')
} else {
  console.error('Mount element #app not found')
  document.write('<h1>Mount element not found. Check index.html</h1>')
}