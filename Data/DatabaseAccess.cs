using AMDB.Models;
using AMDB.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AMDB.Data
{
    public class DatabaseAccess
    {
        public void SeedGenres()
        {
            using var db = new DatabaseContext();
            if (!db.Genres.Any())
            {
                var genres = Enum
                    .GetNames(typeof(GenreTypes))
                    .Select(n => n
                        .Replace("_", " "));
                foreach (var name in genres)
                {
                    db.Genres.Add(new Genre() { Name = name });
                    db.SaveChanges();
                }
            }
        }

        public User GetSignedInUser()
        {
            using var db = new DatabaseContext();
            var user = db.Users.Where(u => u.SignedIn == true).FirstOrDefault();
            return user;
        }

        public async Task<User> RegisterUserAsync(RegisterUserVM model)
        {
            using var db = new DatabaseContext();
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Password = model.Password,
                SignedIn = false
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User> SignInUserAsync(SignInUserVM model)
        {
            using var db = new DatabaseContext();
            var user = await db.Users.Where(u => u.UserName.Equals(model.UserName) && u.Password.Equals(model.Password)).FirstOrDefaultAsync();
            if (user != null)
            {
                await db.Users.ForEachAsync(u => u.SignedIn = false );
                user.SignedIn = true;
                await db.SaveChangesAsync();
            }
            return user;
        }

        public async Task SignOutUserAsync(int id)
        {
            using var db = new DatabaseContext();
            var user = await db.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.SignedIn = false;
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> CreateMovieAsync(MovieVM model)
        {
            using var db = new DatabaseContext();
            var movie = new Movie()
            {
                Title = model.Title,
                ReleaseDate = model.ReleaseDate,
                PosterImage = model.PosterFileName,
                Duration = model.Duration,
                Rating = new Rating { UserRatings = new List<UserRating>() },
                Description = model.Description,
                TrailerURL = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)").Replace(model.TrailerURL, "http://www.youtube.com/embed/$1")
            };
            movie.Director = new DirectorPerson
            {
                Person = db.Persons
                    .Single(p => p.PersonId == model.Director),
                Production = movie
            };
            movie.Genres = db.Genres
                .Where(g => model.SelectedGenres
                    .Any(i => g.GenreId == i))
                .Select(g => new ProductionGenre { Genre = g, Production = movie })
                .ToList();
            movie.Stars = db.Persons
                .Where(p => model.SelectedStars
                    .Any(i => p.PersonId == i))
                .Select(p => new ProductionPerson { Person = p, Production = movie })
                .ToList();
            movie.Keywords = db.Keywords
                .Where(t => model.SelectedKeywords
                    .Any(n => t.Name
                        .Equals(n)))
                .Select(t => new ProductionKeyword { Keyword = t, KeywordId = t.KeywordId, Production = movie })
                .ToList();
            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();
            return movie.ProductionId;
        }

        public async Task UpdateMovieAsync(MovieVM model, int id)
        {
            using var db = new DatabaseContext();
            var movie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .Include(m => m.Stars)
                .Include(m => m.Keywords)
                .FirstOrDefaultAsync(m => m.ProductionId == id);
            movie.Title = model.Title;
            movie.ReleaseDate = model.ReleaseDate;
            movie.PosterImage = model.PosterFileName;
            movie.Duration = model.Duration;
            movie.Description = model.Description;
            movie.TrailerURL = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)").Replace(model.TrailerURL, "http://www.youtube.com/embed/$1");
            movie.Director = new DirectorPerson
            {
                Person = db.Persons
                    .Single(p => p.PersonId == model.Director),
                Production = movie
            };
            movie.Genres = db.Genres
                .Where(g => model.SelectedGenres
                    .Any(i => g.GenreId == i))
                .Select(g => new ProductionGenre { Genre = g, Production = movie })
                .ToList();
            movie.Stars = db.Persons
                .Where(p => model.SelectedStars
                    .Any(i => p.PersonId == i))
                .Select(p => new ProductionPerson { Person = p, Production = movie })
                .ToList();
            movie.Keywords = db.Keywords
                .Where(t => model.SelectedKeywords
                    .Any(n => t.Name
                        .Equals(n)))
                .Select(t => new ProductionKeyword { Keyword = t, KeywordId = t.KeywordId, Production = movie })
                .ToList();
            await db.SaveChangesAsync();
        }

        public async Task<int> CreateTVShowAsync(TVShowVM model)
        {
            using var db = new DatabaseContext();
            var tvshow = new TVShow()
            {
                Title = model.Title,
                ReleaseDate = model.ReleaseDate,
                EndDate = model.EndDate,
                Seasons = model.Seasons,
                Rating = new Rating { UserRatings = new List<UserRating>() },
                PosterImage = model.PosterFileName,
                Duration = model.Duration,
                Description = model.Description,
                TrailerURL = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)").Replace(model.TrailerURL, "http://www.youtube.com/embed/$1")
            };
            tvshow.Director = new DirectorPerson
            {
                Person = db.Persons
                    .Single(p => p.PersonId == model.Director),
                Production = tvshow
            };
            tvshow.Genres = db.Genres
                .Where(g => model.SelectedGenres
                    .Any(i => g.GenreId == i))
                .Select(g => new ProductionGenre { Genre = g, Production = tvshow })
                .ToList();
            tvshow.Stars = db.Persons
                .Where(p => model.SelectedStars
                    .Any(i => p.PersonId == i))
                .Select(p => new ProductionPerson { Person = p, Production = tvshow })
                .ToList();
            tvshow.Keywords = db.Keywords
                .Where(t => model.SelectedKeywords
                    .Any(n => t.Name
                        .Equals(n)))
                .Select(t => new ProductionKeyword { Keyword = t, KeywordId = t.KeywordId, Production = tvshow })
                .ToList();
            await db.TVShows.AddAsync(tvshow);
            await db.SaveChangesAsync();
            return tvshow.ProductionId;
        }

        public async Task UpdateTVShowAsync(TVShowVM model, int id)
        {
            using var db = new DatabaseContext();
            var tvshow = await db.TVShows
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .Include(m => m.Stars)
                .Include(m => m.Keywords)
                .FirstOrDefaultAsync(m => m.ProductionId == id);
            tvshow.Title = model.Title;
            tvshow.ReleaseDate = model.ReleaseDate;
            tvshow.EndDate = model.EndDate;
            tvshow.Seasons = model.Seasons;
            tvshow.PosterImage = model.PosterFileName;
            tvshow.Duration = model.Duration;
            tvshow.Description = model.Description;
            tvshow.TrailerURL = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)").Replace(model.TrailerURL, "http://www.youtube.com/embed/$1");
            tvshow.Director = new DirectorPerson
            {
                Person = db.Persons
                    .Single(p => p.PersonId == model.Director),
                Production = tvshow
            };
            tvshow.Genres = db.Genres
                .Where(g => model.SelectedGenres
                    .Any(i => g.GenreId == i))
                .Select(g => new ProductionGenre { Genre = g, Production = tvshow })
                .ToList();
            tvshow.Stars = db.Persons
                .Where(p => model.SelectedStars
                    .Any(i => p.PersonId == i))
                .Select(p => new ProductionPerson { Person = p, Production = tvshow })
                .ToList();
            tvshow.Keywords = db.Keywords
                .Where(t => model.SelectedKeywords
                    .Any(n => t.Name
                        .Equals(n)))
                .Select(t => new ProductionKeyword { Keyword = t, KeywordId = t.KeywordId, Production = tvshow })
                .ToList();
            await db.SaveChangesAsync();
        }

        public async Task<int> CreatePersonAsync(PersonVM model)
        {
            using var db = new DatabaseContext();
            var person = new Person()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImage = model.ProfileImageFileName,
                DateOfBirth = model.DateOfBirth,
                Biography = model.Biography
            };
            await db.Persons.AddAsync(person);
            await db.SaveChangesAsync();
            return person.PersonId;
        }

        public async Task<int> UpdatePersonAsync(PersonVM model, int id)
        {
            using var db = new DatabaseContext();
            var person = await db.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.ProfileImage = model.ProfileImageFileName;
            person.DateOfBirth = model.DateOfBirth;
            person.Biography = model.Biography;
            await db.SaveChangesAsync();
            return person.PersonId;
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            using var db = new DatabaseContext();
            var movies = await db.Movies
                .Include(m => m.Director)
                    .ThenInclude(dp => dp.Person)
                .Include(m => m.Genres)
                    .ThenInclude(pg => pg.Genre)
                .Include(m => m.Stars)
                    .ThenInclude(pp => pp.Person)
                .Include(m => m.Keywords)
                    .ThenInclude(pt => pt.Keyword)
                .Include(m => m.Rating.UserRatings)
                .OrderByDescending(tvs => tvs.ReleaseDate.Year)
                .ToListAsync();
            return movies;
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            using var db = new DatabaseContext();
            var movie = await db.Movies
                .Include(m => m.Director)
                    .ThenInclude(dp => dp.Person)
                .Include(m => m.Genres)
                    .ThenInclude(pg => pg.Genre)
                .Include(m => m.Stars)
                    .ThenInclude(pp => pp.Person)
                .Include(m => m.Keywords)
                    .ThenInclude(pt => pt.Keyword)
                .Include(m => m.Rating.UserRatings)
                .FirstOrDefaultAsync(m => m.ProductionId == id);
            return movie;
        }

        public async Task<List<TVShow>> GetAllTVShowsAsync()
        {
            using var db = new DatabaseContext();
            var tvshows = await db.TVShows
                .Include(tvs => tvs.Director)
                    .ThenInclude(dp => dp.Person)
                .Include(tvs => tvs.Genres)
                    .ThenInclude(pg => pg.Genre)
                .Include(tvs => tvs.Stars)
                    .ThenInclude(pp => pp.Person)
                .Include(tvs => tvs.Keywords)
                    .ThenInclude(pt => pt.Keyword)
                .Include(tvs => tvs.Rating)
                .Include(tvs => tvs.Rating.UserRatings)
                .OrderByDescending(tvs => tvs.ReleaseDate.Year)
                .ToListAsync();
            return tvshows;
        }

        public async Task<TVShow> GetTVShowAsync(int id)
        {
            using var db = new DatabaseContext();
            var tvshow = await db.TVShows
                .Include(tvs => tvs.Director)
                    .ThenInclude(dp => dp.Person)
                .Include(tvs => tvs.Genres)
                    .ThenInclude(pg => pg.Genre)
                .Include(tvs => tvs.Stars)
                    .ThenInclude(pp => pp.Person)
                .Include(tvs => tvs.Keywords)
                    .ThenInclude(pt => pt.Keyword)
                .Include(tvs => tvs.Rating.UserRatings)
                .FirstOrDefaultAsync(tvs => tvs.ProductionId == id);
            return tvshow;
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            using var db = new DatabaseContext();
            var persons = await db.Persons
                .OrderBy(p => p.FirstName)
                .ToListAsync();
            return persons;
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            using var db = new DatabaseContext();
            var person = await db.Persons
                .Include(p => p.DirectorCredits)
                    .ThenInclude(dp => dp.Production)
                .Include(p => p.ProductionCredits)
                    .ThenInclude(pp => pp.Production.Rating.UserRatings)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            return person;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            using var db = new DatabaseContext();
            var genres = await db.Genres
                .OrderBy(p => p.Name)
                .ToListAsync();
            return genres;
        }

        public async Task<Genre> GetGenreAsync(int id)
        {
            using var db = new DatabaseContext();
            var genre = await db.Genres
                .Include(g => g.Productions)
                    .ThenInclude(pg => pg.Production.Rating.UserRatings)
                .FirstOrDefaultAsync(m => m.GenreId == id);
            return genre;
        }

        public async Task<List<string>> GetAllKeywordNamesAsync()
        {
            using var db = new DatabaseContext();
            var Keywords = await db.Keywords
                .Select(t => t.Name)
                .OrderBy(p => p)
                .ToListAsync();
            return Keywords;
        }

        public async Task<Keyword> GetKeywordAsync(int id)
        {
            using var db = new DatabaseContext();
            var genre = await db.Keywords
                .Include(p => p.Productions)
                    .ThenInclude(pt => pt.Production.Rating.UserRatings)
                .FirstOrDefaultAsync(m => m.KeywordId == id);
            return genre;
        }

        public async Task CreateUniqueKeywordNamesAsync(List<string> Keywords)
        {
            using var db = new DatabaseContext();
            await db.Keywords.AddRangeAsync(Keywords
                .Where(n => db.Keywords
                .All(t => !t.Name
                    .Equals(n)))
                .Select(n => new Keyword { Name = n }));
            await db.SaveChangesAsync();
        }

        public async Task<List<Production>> GetProductionsOrderedByBestKeywordMatches(Production prod)
        {
            using var db = new DatabaseContext();
            var prodKeywordIds = prod.Keywords
                .Select(pt => pt.KeywordId)
                .ToList();
            var prodMatches = await db.Productions
                .Where(p => p.Keywords
                    .Any(pt => prodKeywordIds
                        .Contains(pt.KeywordId) && pt.ProductionId != prod.ProductionId))
                .Include(p => p.Keywords)
                .ToListAsync();
            var prodsOrdered = prodMatches
                .Select(p => p)
                .OrderByDescending(p => p.Keywords
                    .Select(p => p.KeywordId)
                    .Intersect(prodKeywordIds)
                    .Count())
                .Take(10)
                .ToList();
            return prodsOrdered;
        }

        public async Task<Production> CreateUserRating(UserRatingVM model)
        {
            using var db = new DatabaseContext();
            var rating = await db.Ratings.Where(r => r.RatingId == model.RatingId).FirstOrDefaultAsync();
            var user = await db.Users.Where(r => r.UserId == model.UserId).FirstOrDefaultAsync();
            await db.UserRatings.AddAsync(new UserRating { User = user, Rating = rating, Value = model.Value });
            await db.SaveChangesAsync();
            var prod = await db.Productions.Where(p => p.Rating.RatingId == model.RatingId).FirstOrDefaultAsync();
            return prod;
        }

        public async Task<SearchResultVM> SearchAsync(string searchString)
        {
            string search = searchString.ToLower();
            using var db = new DatabaseContext();
            var result = new SearchResultVM { SearchString = search };
            var productions = await db.Productions
                .Where(p => p.Title
                    .ToLower()
                    .Contains(search)
                    || p.Keywords
                        .Any(pk => pk.Keyword.Name
                            .ToLower()
                            .Contains(search)))
                .OrderByDescending(m => m.ReleaseDate.Year)
                .ToListAsync();
            result.Movies = productions.Where(p => p is Movie).ToList();
            result.TVShows = productions.Where(p => p is TVShow).ToList();
            var people = await db.Persons
                .Where(p => p.FirstName
                    .ToLower()
                    .Contains(search) || p.LastName
                        .ToLower()
                        .Contains(search))
                .OrderBy(p => p.FirstName)
                .ToListAsync();
            result.People = people;
            return result;
        }
    }
}
