<template>
  <div class="flex flex-wrap justify-center mb-8">
    <button 
      v-for="(platform, index) in platforms" 
      :key="platform.id"
      @click="$emit('platform-change', index)"
      :class="[
        'px-6 py-3 rounded-t-lg font-medium transition-all duration-300 flex items-center',
        activePlatformIndex === index 
          ? 'bg-white text-indigo-600 shadow-md border-t border-l border-r border-gray-200' 
          : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
      ]"
    >
      <div class="w-4 h-4 rounded-full mr-2" :style="{ backgroundColor: platform.color }"></div>
      {{ platform.name }}
    </button>
  </div>
</template>

<script>
export default {
  name: 'PlatformTabs',
  props: {
    platforms: {
      type: Array,
      required: true
    },
    activePlatformIndex: {
      type: Number,
      required: true
    }
  },
  emits: ['platform-change']
}
</script>