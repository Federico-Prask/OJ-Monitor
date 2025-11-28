# 监视列表问题诊断指南

## 检查清单

### 1. 后端爬虫是否工作

在后端日志中查找以下信息（启动爬虫后 5-10 秒内应该出现）：

```
[Information] Fetching https://www.luogu.com.cn/record/list?user=ricky_lin&status=12&page=1
[Information] Fetched XXXX bytes for ricky_lin page 1
[Information] Parsed X submissions from page 1
[Information] Successfully parsed X submissions for user ricky_lin
```

如果看到以下错误：
- `Failed to find JSON data in HTML` - 说明洛谷页面格式变化，需要更新解析逻辑
- `HttpRequestException` - 网络连接问题
- 没有任何日志 - 爬虫没有启动或崩溃

### 2. 测试后端 API

```bash
# 启动爬虫
curl -X POST http://localhost:5000/api/watchlist/start \
  -H "Content-Type: application/json" \
  -d '{"usernames":["ricky_lin"],"intervalSeconds":5}'

# 等待 10 秒

# 获取通知
curl http://localhost:5000/api/watchlist/notifications

# 停止爬虫
curl -X POST http://localhost:5000/api/watchlist/stop
```

### 3. 前端轮询是否工作

打开浏览器开发者工具 (F12)，查看:
- **Network 标签**: 是否有 `/api/watchlist/notifications` 请求？
- **Console 标签**: 是否有错误信息？ 是否看到 "收到新通知:" 的日志？

### 4. 数据流检查

```
用户在前端点击 "启动" 
  ↓
前端发送 POST /api/watchlist/start
  ↓
后端 WatchlistCrawlerService 启动爬虫循环
  ↓
爬虫开始爬取洛谷数据 (每 5 秒)
  ↓
如果检测到新提交，添加到 _unnotifiedSubmissions
  ↓
前端轮询 GET /api/watchlist/notifications (每 5 秒，延迟 10 秒启动)
  ↓
后端返回 _unnotifiedSubmissions，然后清空队列
  ↓
前端显示通知
```

## 常见问题

### 问题 1: 获取通知为空

**原因可能:**
1. 爬虫还未启动或还未爬取数据（需要等待至少 5-10 秒）
2. 用户名拼写错误 - 检查洛谷是否有该用户
3. 用户没有最近的 AC 提交 - 爬虫只爬取 AC 的提交 (status=12)

**解决方案:**
- 确认用户在洛谷上存在
- 在洛谷网站手动搜索该用户，查看是否有最近的 AC 提交
- 查看后端日志是否有错误

### 问题 2: 前端无法启动爬虫

**原因可能:**
1. 后端没有运行 - 检查 `http://localhost:5000` 是否可达
2. 代理配置错误 - Vite 的 /api 代理没有正确转发到后端

**解决方案:**
```bash
# 检查后端
curl http://localhost:5000/

# 检查代理
curl http://localhost:3001/api/platforms  # (假设前端在 3001 端口)
```

### 问题 3: 爬虫崩溃或停止

**原因可能:**
1. 洛谷网站 HTML 格式变化，解析失败
2. 网络超时
3. 内存不足

**解决方案:**
- 查看后端日志中的具体错误信息
- 如果是 HTML 解析错误，需要更新 ParseLuoguRecords 方法

## 调试技巧

### 启用详细日志

在 `Program.cs` 中修改日志级别：

```csharp
.ConfigureLogging(logging =>
{
    logging.SetMinimumLevel(LogLevel.Debug); // 改为 Debug
})
```

### 模拟数据测试

如果洛谷爬虫有问题，可以在 WatchlistCrawlerService 中添加模拟数据：

```csharp
if (username == "test_user")
{
    // 返回测试数据
    submissions.Add(new { problem = "测试题目", id = "P1001", difficulty = 3, time = DateTime.Now });
}
```

### 使用 curl 模拟前端

```bash
# 启动
curl -X POST http://localhost:5000/api/watchlist/start \
  -H "Content-Type: application/json" \
  -d '{"usernames":["ricky_lin"]}'

# 轮询（每 2 秒）
for i in {1..5}; do
  echo "轮询 $i:"
  curl http://localhost:5000/api/watchlist/notifications
  sleep 2
done

# 停止
curl -X POST http://localhost:5000/api/watchlist/stop
```

## 更新日志

已添加以下改进：

1. ✅ 后端爬虫添加详细日志
2. ✅ 改进 JSON 解析错误处理
3. ✅ 前端轮询延迟 10 秒启动（给爬虫时间收集数据）
4. ✅ 前端改为每 5 秒轮询一次（与爬虫间隔同步）
5. ✅ 添加通知数据的 difficultyNames 映射
