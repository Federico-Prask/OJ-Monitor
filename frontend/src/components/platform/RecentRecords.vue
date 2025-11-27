<template>
  <div>
    <h3 class="text-xl font-bold text-gray-800 mb-4">最近做题记录</h3>
    <div class="overflow-x-auto">
      <table class="min-w-full bg-white rounded-lg overflow-hidden">
        <thead class="bg-gray-100">
          <tr>
            <th class="py-3 px-4 text-left text-gray-600 font-medium">题目</th>
            <th class="py-3 px-4 text-left text-gray-600 font-medium">难度</th>
            <th class="py-3 px-4 text-left text-gray-600 font-medium">提交时间</th>
            <th class="py-3 px-4 text-left text-gray-600 font-medium">状态</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-200">
          <tr v-for="(record, index) in records" :key="index">
            <td class="py-3 px-4">
              <div class="flex items-center">
                <div class="w-8 h-8 rounded-md flex items-center justify-center text-white mr-3 bg-indigo-500">
                  <i class="fas fa-code text-xs"></i>
                </div>
                <div>
                  <p class="font-medium">{{ record.problem }}</p>
                  <p class="text-xs text-gray-500">{{ record.id }}</p>
                </div>
              </div>
            </td>
            <td class="py-3 px-4">
              <span :class="[
                'px-2 py-1 rounded-full text-xs font-medium',
                record.difficulty === '简单' ? 'bg-green-100 text-green-800' :
                record.difficulty === '中等' ? 'bg-yellow-100 text-yellow-800' :
                'bg-red-100 text-red-800'
              ]">
                {{ record.difficulty }}
              </span>
            </td>
            <td class="py-3 px-4 text-gray-600">{{ record.time }}</td>
            <td class="py-3 px-4">
              <span :class="[
                'px-2 py-1 rounded-full text-xs font-medium',
                record.status === '通过' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
              ]">
                {{ record.status }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: 'RecentRecords',
  props: {
    records: {
      type: Array,
      default: () => []
    }
  }
}
</script>