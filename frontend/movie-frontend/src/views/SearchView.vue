<script setup>
import { ref, onMounted } from 'vue'
import MovieCard from '../components/MovieCard.vue'
import { movieService, eventService } from '../services/api.js'

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

  // Fire search event when user types more than 2 characters
  if (query.value.length > 2) {
    eventService.trackSearch(query.value)
  }
}

function filterByGenre(genre) {
  activeGenre.value = genre
  onSearch()
}
</script>