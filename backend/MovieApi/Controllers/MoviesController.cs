using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        return await _context.Movies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();
        return movie;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return await _context.Movies.ToListAsync();

        return await _context.Movies
            .Where(m => m.Title.Contains(q) || m.Genre.Contains(q) || m.Director.Contains(q))
            .ToListAsync();
    }

    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<string>>> GetGenres()
    {
        return await _context.Movies
            .Select(m => m.Genre)
            .Distinct()
            .ToListAsync();
    }
}