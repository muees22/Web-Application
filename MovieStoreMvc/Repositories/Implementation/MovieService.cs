using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;
using System.Security.Policy;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext _ctx;
        public MovieService(DatabaseContext ctx)
        {
            _ctx = ctx; 
        }
        public bool Add(Movie model)
        {
            try
            {  
                _ctx.Movie.Add(model);
                _ctx.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre 
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    _ctx.MovieGenre.Add(movieGenre); 
                }
                _ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if(data==null) 
                 return false;
                var movieGenres = _ctx.MovieGenre.Where(a => a.MovieId == data.Id);
                foreach(var movieGenre in movieGenres)
                {
                    _ctx.MovieGenre.Remove(movieGenre);
                }
                _ctx.Movie.Remove(data);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
           return _ctx.Movie.Find(id);
        }

        public MovieListVm List()
        {
            var list = _ctx.Movie.ToList();
            foreach (var movie in list)
            {
                var genres = (from genre in _ctx.Genre
                              join mg in _ctx.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }

            var data = new MovieListVm
            {
                MovieList = list.AsQueryable()
            };
            return data;
        }

        public bool Update(Movie model)
        {
            try
            { 
                _ctx.Movie.Update(model);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      public List<int> GetGenreByMovieId(int movieId)
        {
            var genreIds = _ctx.MovieGenre.Where(a=>a.MovieId == movieId).Select(a=>a.GenreId).ToList();
            return genreIds;
        }
    }
}
