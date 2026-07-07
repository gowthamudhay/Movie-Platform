<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { movieService, eventService } from '../services/api.js'

const route = useRoute()
const movie = ref(null)
const loading = ref(true)

onMounted(async () => {
  try {
    const res = await movieService.getById(route.params.id)
    movie.value = res.data

    // FIRE EVENT TO KINESIS PIPELINE
    await eventService.trackMovieView(res.data)
    console.log('Event tracked for movie:', res.data.title)

  } finally {
    loading.value = false
  }
})
</script>