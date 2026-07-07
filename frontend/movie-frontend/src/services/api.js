import axios from 'axios'

const api = axios.create({
  baseURL: 'https://movie-platform-production-96b2.up.railway.app/api'
})

export const movieService = {
  getAll: () => api.get('/movies'),
  getById: (id) => api.get(`/movies/${id}`),
  search: (q) => api.get(`/movies/search?q=${q}`),
  getGenres: () => api.get('/movies/genres')
}

// Generate a random session ID for this browser session
const sessionId = Math.random().toString(36).substring(2, 15)
const userId = 'user_' + Math.random().toString(36).substring(2, 8)

export const eventService = {
  trackMovieView: (movie) => api.post('/events', {
    eventType: 'MOVIE_VIEW',
    movieId: movie.id,
    movieTitle: movie.title,
    userId: userId,
    sessionId: sessionId,
    genre: movie.genre,
    timestamp: new Date().toISOString()
  }),

  trackSearch: (query) => api.post('/events', {
    eventType: 'SEARCH',
    movieId: 0,
    movieTitle: query,
    userId: userId,
    sessionId: sessionId,
    genre: '',
    timestamp: new Date().toISOString()
  })
}