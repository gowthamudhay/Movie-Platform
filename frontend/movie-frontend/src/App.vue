<template>
  <div id="app">
    <NavBar />
    <main>
      <RouterView />
    </main>
  </div>
</template>

<script setup>
import NavBar from './components/NavBar.vue'
</script>

<style>
* { margin: 0; padding: 0; box-sizing: border-box; }

:root {
  --bg: #0f0f0f;
  --surface: #1a1a1a;
  --surface2: #252525;
  --accent: #e50914;
  --text: #ffffff;
  --text-muted: #aaaaaa;
  --radius: 8px;
}

body {
  background: var(--bg);
  color: var(--text);
  font-family: -apple-system, sans-serif;
  min-height: 100vh;
}

main {
  max-width: 1200px;
  margin: 0 auto;
  padding: 24px 16px;
}

a { color: inherit; text-decoration: none; }
</style>