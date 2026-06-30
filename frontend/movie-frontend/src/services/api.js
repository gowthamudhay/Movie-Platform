import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5146/api'
})

export const movieService = {
  getAll: () => api.get('/movies'),
  getById: (id) => api.get(`/movies/${id}`),
  search: (q) => api.get(`/movies/search?q=${q}`),
  getGenres: () => api.get('/movies/genres')
}