import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    port: 3000,
    host: true, // 允许外部访问
    open: true
  },
  build: {
    outDir: 'dist',
    sourcemap: true // 确保源代码映射启用
  },
  // 优化依赖预构建
  optimizeDeps: {
    include: ['vue', 'pinia']
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
})