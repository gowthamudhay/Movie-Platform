<template>
  <div>
    <input
      v-model="query"
      @input="onSearch"
      type="text"
      placeholder="Search by title, genre, or director..."
      class="input"
      autofocus
    />
    <div class="filters">
      <button class="btn" :class="{ active: activeGenre === '' }" @click="filterByGenre('')">All</button>
      <button
        v-for="genre in genres" :key="genre"
        class="btn" :class="{ active: activeGenre === genre }"
        @click="filterByGenre(genre)"
      >{{ genre }}</button>
    </div>
    <div v-if="results.length === 0" class="empty">No movies found</div>
    <div v-else class="grid">
      <MovieCard v-for="movie in results" :key="movie.id" :movie="movie" />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import MovieCard from '../components/MovieCard.vue'
import { movieService } from '../services/api.js'

const query = ref('')
const results = ref([])
const genres = ref([])
const activeGenre = ref('')
let allMovies = []

onMounted(async () => {
  const [moviesRes, genresRes] = await Promise.all([
    movieService.getAll(),
    movieService.getGenres()
  ])
  allMovies = moviesRes.data
  results.value = allMovies
  genres.value = genresRes.data
})

function onSearch() {
  const q = query.value.toLowerCase()
  results.value = allMovies.filter(m =>
    (m.title.toLowerCase().includes(q) ||
     m.genre.toLowerCase().includes(q) ||
     m.director.toLowerCase().includes(q)) &&
    (activeGenre.value === '' || m.genre === activeGenre.value)
  )
}

function filterByGenre(genre) {
  activeGenre.value = genre
  onSearch()
}
</script>

<style scoped>
.input {
  width: 100%; padding: 14px 18px; font-size: 16px;
  background: var(--surface); border: 1px solid #333;
  border-radius: var(--radius); color: var(--text);
  outline: none; margin-bottom: 16px; display: block;
}
.input:focus { border-color: var(--accent); }
.filters { display: flex; flex-wrap: wrap; gap: 8px; margin-bottom: 24px; }
.btn {
  padding: 6px 14px; border-radius: 20px;
  border: 1px solid #333; background: transparent;
  color: var(--text-muted); font-size: 13px; cursor: pointer;
}
.btn:hover { border-color: #666; color: var(--text); }
.btn.active { background: var(--accent); border-color: var(--accent); color: white; }
.grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(160px, 1fr)); gap: 16px; }
.empty { text-align: center; color: var(--text-muted); padding: 64px; }
</style>