<template>
  <div>
    <section class="hero">
      <h1>Discover great films</h1>
      <p>Personalised recommendations powered by AI</p>
      <RouterLink to="/search" class="cta">Browse all movies →</RouterLink>
    </section>

    <section>
      <h2 class="section-title">All Movies</h2>
      <div v-if="loading" class="loading">Loading movies...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else class="grid">
        <MovieCard v-for="movie in movies" :key="movie.id" :movie="movie" />
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import MovieCard from '../components/MovieCard.vue'
import { movieService } from '../services/api.js'

const movies = ref([])
const loading = ref(true)
const error = ref(null)

onMounted(async () => {
  try {
    const res = await movieService.getAll()
    movies.value = res.data
  } catch {
    error.value = 'Could not load movies. Is the API running?'
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.hero { text-align: center; padding: 64px 24px 48px; }
.hero h1 { font-size: 48px; font-weight: 800; letter-spacing: -1px; margin-bottom: 12px; }
.hero p { font-size: 18px; color: var(--text-muted); margin-bottom: 24px; }
.cta {
  display: inline-block; background: var(--accent);
  color: white; padding: 12px 28px;
  border-radius: 6px; font-weight: 600; font-size: 15px;
}
.cta:hover { opacity: 0.85; }
.section-title { font-size: 22px; font-weight: 700; margin-bottom: 20px; }
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 16px;
}
.loading, .error { text-align: center; color: var(--text-muted); padding: 48px; }
.error { color: var(--accent); }
</style>