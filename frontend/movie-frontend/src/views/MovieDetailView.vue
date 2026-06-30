<template>
  <div>
    <button class="back" @click="$router.back()">← Back</button>
    <div v-if="loading" class="loading">Loading...</div>
    <div v-else-if="movie" class="detail">
      <img :src="movie.posterUrl" :alt="movie.title" class="poster" />
      <div class="info">
        <span class="genre-badge">{{ movie.genre }}</span>
        <h1 class="title">{{ movie.title }}</h1>
        <div class="meta">
          <span class="rating">⭐ {{ movie.rating }} / 10</span>
          <span>{{ movie.year }}</span>
          <span>{{ movie.director }}</span>
        </div>
        <p class="desc">{{ movie.description }}</p>
        <div class="actions">
          <button class="btn-primary">▶ Watch now</button>
          <button class="btn-secondary">+ Watchlist</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { movieService } from '../services/api.js'

const route = useRoute()
const movie = ref(null)
const loading = ref(true)

onMounted(async () => {
  try {
    const res = await movieService.getById(route.params.id)
    movie.value = res.data
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.back {
  background: transparent; border: 1px solid #333;
  color: var(--text-muted); padding: 8px 16px;
  border-radius: 6px; cursor: pointer; margin-bottom: 32px;
}
.back:hover { color: var(--text); }
.detail { display: grid; grid-template-columns: 280px 1fr; gap: 40px; align-items: start; }
@media (max-width: 640px) { .detail { grid-template-columns: 1fr; } }
.poster { width: 100%; border-radius: 12px; }
.genre-badge {
  display: inline-block; background: rgba(229,9,20,0.15);
  color: var(--accent); font-size: 12px; font-weight: 600;
  padding: 4px 12px; border-radius: 20px; margin-bottom: 12px;
}
.title { font-size: 36px; font-weight: 800; margin-bottom: 16px; line-height: 1.1; }
.meta { display: flex; gap: 16px; flex-wrap: wrap; margin-bottom: 24px; color: var(--text-muted); font-size: 14px; }
.rating { color: #ffd700; font-weight: 600; }
.desc { font-size: 16px; line-height: 1.7; color: #ccc; margin-bottom: 32px; }
.actions { display: flex; gap: 12px; }
.btn-primary {
  background: var(--accent); color: white; border: none;
  padding: 12px 28px; border-radius: 6px;
  font-size: 15px; font-weight: 600; cursor: pointer;
}
.btn-secondary {
  background: transparent; color: var(--text);
  border: 1px solid #444; padding: 12px 24px;
  border-radius: 6px; font-size: 15px; cursor: pointer;
}
</style>