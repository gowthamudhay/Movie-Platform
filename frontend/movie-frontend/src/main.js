import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import HomeView from './views/HomeView.vue'
import SearchView from './views/SearchView.vue'
import MovieDetailView from './views/MovieDetailView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: HomeView },
    { path: '/search', component: SearchView },
    { path: '/movies/:id', component: MovieDetailView }
  ]
})

const app = createApp(App)
app.use(router)
app.mount('#app')