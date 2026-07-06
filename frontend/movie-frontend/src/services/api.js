import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api'
})

export const movieService = {
  getAll: () => api.get('/movies'),
  getById: (id) => api.get(`/movies/${id}`),
  search: (q) => api.get(`/movies/search?q=${q}`),
  getGenres: () => api.get('/movies/genres')
}