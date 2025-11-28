#!/bin/bash

# OJ-Monitor 监视列表测试脚本

API_BASE="http://localhost:5000/api/watchlist"

echo "=== OJ-Monitor 监视列表测试 ==="
echo ""

# 测试启动监视列表
echo "1. 启动监视列表 (测试用户: ricky_lin)"
curl -X POST "$API_BASE/start" \
  -H "Content-Type: application/json" \
  -d '{
    "usernames": ["ricky_lin"],
    "intervalSeconds": 5
  }' \
  -v

echo -e "\n\n等待 15 秒让爬虫收集数据...\n"
sleep 15

# 获取通知
echo "2. 获取未通知的新提交"
curl -X GET "$API_BASE/notifications" \
  -H "Content-Type: application/json" \
  -v

echo -e "\n\n3. 停止监视列表"
curl -X POST "$API_BASE/stop" \
  -H "Content-Type: application/json" \
  -v

echo -e "\n\n测试完成！"
