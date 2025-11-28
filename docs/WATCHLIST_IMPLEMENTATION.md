# OJ-Monitor 监视列表功能实现总结

## 📋 实现清单

### 后端 (C# .NET 8)

#### 1. 数据模型
- ✅ `Models/Entities/WatchlistSubmission.cs` - 提交记录实体
- ✅ `Models/DTOs/WatchlistSubmissionDto.cs` - 数据传输对象

#### 2. 爬虫服务
- ✅ `Services/Crawlers/LuoguWatchlistCrawler.cs` - 真实洛谷爬虫
  - 爬取用户最新提交
  - 解析 HTML 中的 JSON 数据
  - 难度等级识别
  - 用户验证

- ✅ `Services/Interfaces/IWatchlistCrawlerService.cs` - 爬虫服务接口

- ✅ `Services/WatchlistCrawlerService.cs` - 爬虫服务实现
  - 定期爬虫循环（可配置间隔，默认 5 秒）
  - 新提交检测和通知队列
  - 提交状态管理

#### 3. 控制器
- ✅ `Controllers/WatchlistController.cs` - API 控制器
  - `POST /api/watchlist/start` - 启动爬虫
  - `POST /api/watchlist/stop` - 停止爬虫
  - `GET /api/watchlist/notifications` - 获取未通知提交
  - `POST /api/watchlist/mark-notified` - 标记已通知

#### 4. 配置更新
- ✅ `Program.cs` - 注册爬虫服务

### 前端 (Vue 3 + Tailwind CSS)

#### 1. 组合函数
- ✅ `composables/useWatchlistStorage.js` - LocalStorage 管理
  - CRUD 监视列表
  - 用户管理（添加/删除）

#### 2. 服务
- ✅ `services/watchlistService.js` - API 调用封装
  - `startWatchlist(usernames, intervalSeconds)`
  - `stopWatchlist()`
  - `getNotifications()`
  - `markNotified(username, problemId)`

#### 3. 组件
- ✅ `components/watchlist/WatchlistManager.vue` - 完整管理界面
  - 创建/删除监视列表
  - 管理用户列表
  - 启动/停止爬虫
  - 实时通知显示
  - 桌面通知集成

#### 4. 视图更新
- ✅ `views/Home.vue` - 添加监视列表标签页

#### 5. 文档
- ✅ `docs/WATCHLIST.md` - 完整使用指南

## 🚀 快速开始

### 启动应用

```bash
# 终端 1: 启动后端
ASPNETCORE_URLS=http://localhost:5000 dotnet run --project backend/OJMonitor.API

# 终端 2: 启动前端
cd frontend
npm install
npm run dev
```

### 使用流程

1. 打开浏览器访问 `http://localhost:3000` (或 Codespaces 公网地址)
2. 切换到"🎯 监视列表"标签页
3. 创建新列表并添加要监视的洛谷用户
4. 点击"▶️ 启动"开始监视
5. 新提交时会在通知面板显示并可触发桌面通知

## 📡 API 流程

```
前端 (Home.vue + WatchlistManager.vue)
  ↓
  → POST /api/watchlist/start (启动爬虫)
  ↓
后端爬虫循环 (WatchlistCrawlerService)
  ↓
  → 定期爬取洛谷 (LuoguWatchlistCrawler)
  ↓
  → 检测新提交 → 加入通知队列
  ↓
前端轮询 (3 秒一次)
  → GET /api/watchlist/notifications (获取新提交)
  ↓
显示通知 + 桌面通知
```

## 🎨 UI 特性

- 选项卡式导航（平台查询 / 监视列表）
- 创建和管理多个监视列表
- 灵活的用户列表管理
- 实时通知面板
- 颜色编码难度等级（灰→黑）
- 桌面通知集成
- 洛谷链接快捷打开

## 💾 数据存储

- **监视列表配置**：LocalStorage (Cookie)
  - 列表名称
  - 用户列表
  - 创建时间
  - 运行状态

- **提交记录**：内存 (ConcurrentDictionary)
  - 最新提交追踪
  - 通知队列

## ⚙️ 可配置选项

在启动爬虫时可以指定：
- `usernames` - 要监视的用户列表
- `intervalSeconds` - 轮询间隔（默认 5 秒）

前端轮询间隔：3 秒（可在 WatchlistManager.vue 中修改）

## 🔧 技术栈

### 后端
- .NET 8 (C#)
- ASP.NET Core
- HttpClient
- Regex (HTML 解析)
- JsonDocument (JSON 解析)

### 前端
- Vue 3 (Composition API)
- Tailwind CSS
- Axios
- Browser Notifications API

## ✨ 改进方向

未来可以添加：
1. WebSocket 实时推送（替代轮询）
2. 数据库持久化
3. 更多平台支持（LSYOJ、OI Class）
4. 提交历史记录
5. 用户排行榜
6. 难度统计图表
7. 邮件/Telegram 通知
