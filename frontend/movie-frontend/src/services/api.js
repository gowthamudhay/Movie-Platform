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