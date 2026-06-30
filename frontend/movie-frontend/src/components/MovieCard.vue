<template>
  <RouterLink :to="`/movies/${movie.id}`" class="card">
    <div class="poster-wrap">
      <img :src="movie.posterUrl" :alt="movie.title" class="poster" @error="onError" />
      <div class="rating">⭐ {{ movie.rating }}</div>
    </div>
    <div class="info">
      <h3 class="title">{{ movie.title }}</h3>
      <div class="meta">
        <span class="genre">{{ movie.genre }}</span>
        <span class="year">{{ movie.year }}</span>
      </div>
    </div>
  </RouterLink>
</template>

<script setup>
defineProps({ movie: Object })
function onError(e) {
  e.target.src = 'https://via.placeholder.com/300x450/1a1a1a/666?text=No+Poster'
}
</script>

<style scoped>
.card {
  display: block;
  background: var(--surface);
  border-radius: var(--radius);
  overflow: hidden;
  transition: transform 0.2s;
  cursor: pointer;
}
.card:hover { transform: translateY(-4px); }
.poster-wrap { position: relative; aspect-ratio: 2/3; background: var(--surface2); }
.poster { width: 100%; height: 100%; object-fit: cover; display: block; }
.rating {
  position: absolute; top: 8px; right: 8px;
  background: rgba(0,0,0,0.8); color: #ffd700;
  font-size: 12px; font-weight: 600;
  padding: 4px 8px; border-radius: 20px;
}
.info { padding: 12px; }
.title {
  font-size: 14px; font-weight: 600; margin-bottom: 6px;
  white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
}
.meta { display: flex; justify-content: space-between; }
.genre {
  font-size: 12px; color: var(--accent);
  background: rgba(229,9,20,0.15);
  padding: 2px 8px; border-radius: 12px;
}
.year { font-size: 12px; color: var(--text-muted); }
</style>