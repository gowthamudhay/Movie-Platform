using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = 1, Title = "The Shawshank Redemption", Genre = "Drama", Year = 1994, Rating = 9.3, Director = "Frank Darabont", Description = "Two imprisoned men bond over years, finding solace and redemption.", PosterUrl = "https://placehold.co/300x450/8b0000/ffffff?text=Shawshank" },
            new Movie { Id = 2, Title = "The Godfather", Genre = "Crime", Year = 1972, Rating = 9.2, Director = "Francis Ford Coppola", Description = "The aging patriarch of an organized crime dynasty transfers control to his son.", PosterUrl = "https://placehold.co/300x450/333333/ffffff?text=Godfather" },
            new Movie { Id = 3, Title = "The Dark Knight", Genre = "Action", Year = 2008, Rating = 9.0, Director = "Christopher Nolan", Description = "Batman faces the Joker, who plunges Gotham into anarchy.", PosterUrl = "https://placehold.co/300x450/1a1a1a/ffffff?text=Dark+Knight" },
            new Movie { Id = 4, Title = "Pulp Fiction", Genre = "Crime", Year = 1994, Rating = 8.9, Director = "Quentin Tarantino", Description = "The lives of two mob hitmen intertwine in Los Angeles.", PosterUrl = "https://placehold.co/300x450/d4a017/000000?text=Pulp+Fiction" },
            new Movie { Id = 5, Title = "Inception", Genre = "Sci-Fi", Year = 2010, Rating = 8.8, Director = "Christopher Nolan", Description = "A thief enters the dreams of others to steal secrets.", PosterUrl = "https://placehold.co/300x450/0a2a4d/ffffff?text=Inception" },
            new Movie { Id = 6, Title = "Interstellar", Genre = "Sci-Fi", Year = 2014, Rating = 8.6, Director = "Christopher Nolan", Description = "Explorers travel through a wormhole to ensure humanity's survival.", PosterUrl = "https://placehold.co/300x450/2b2b52/ffffff?text=Interstellar" },
            new Movie { Id = 7, Title = "Parasite", Genre = "Thriller", Year = 2019, Rating = 8.5, Director = "Bong Joon-ho", Description = "Greed and class discrimination threaten two families.", PosterUrl = "https://placehold.co/300x450/4a5d23/ffffff?text=Parasite" },
            new Movie { Id = 8, Title = "The Matrix", Genre = "Sci-Fi", Year = 1999, Rating = 8.7, Director = "The Wachowskis", Description = "A hacker learns the true nature of reality.", PosterUrl = "https://placehold.co/300x450/0d3d0d/00ff00?text=Matrix" },
            new Movie { Id = 9, Title = "Goodfellas", Genre = "Crime", Year = 1990, Rating = 8.7, Director = "Martin Scorsese", Description = "The story of Henry Hill and his life in the mob.", PosterUrl = "https://placehold.co/300x450/5c1a1a/ffffff?text=Goodfellas" },
            new Movie { Id = 10, Title = "Fight Club", Genre = "Drama", Year = 1999, Rating = 8.8, Director = "David Fincher", Description = "An office worker and soap salesman form an underground fight club.", PosterUrl = "https://placehold.co/300x450/8b0000/ffffff?text=Fight+Club" }
        );
    }
}