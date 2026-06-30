<template>
  <nav class="navbar">
    <RouterLink to="/" class="logo">🎬 MoviePlatform</RouterLink>
    <div class="links">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/search">Search</RouterLink>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 32px;
  background: rgba(15,15,15,0.95);
  border-bottom: 1px solid #2a2a2a;
  position: sticky;
  top: 0;
  z-index: 100;
}
.logo { font-size: 20px; font-weight: 700; color: var(--accent); }
.links { display: flex; gap: 24px; }
.links a { color: var(--text-muted); font-size: 14px; font-weight: 500; }
.links a:hover, .links a.router-link-active { color: var(--text); }
</style>