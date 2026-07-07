import { eventService } from '../services/api.js'

function onSearch() {
  const q = query.value.toLowerCase()
  results.value = allMovies.filter(m =>
    (m.title.toLowerCase().includes(q) ||
     m.genre.toLowerCase().includes(q) ||
     m.director.toLowerCase().includes(q)) &&
    (activeGenre.value === '' || m.genre === activeGenre.value)
  )

  // Track search event
  if (query.value.length > 2) {
    eventService.trackSearch(query.value)
  }
}