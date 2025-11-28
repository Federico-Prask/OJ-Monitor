import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
  server: {
    host: '0.0.0.0', // 监听所有接口以支持远程访问（Codespaces）
    open: false,
    // 将开发服务器的 /api 请求代理到后端 API
    proxy: {
      '/api': {
        // 后端地址：优先使用环境变量 VITE_API_TARGET，否则默认使用 localhost:5000
        target: process.env.VITE_API_TARGET || 'http://localhost:5000',
        changeOrigin: true,
        secure: false,
        // 保留 /api 前缀，使前端代码可继续使用相对路径
        rewrite: (path) => path
      }
    }
  }
})