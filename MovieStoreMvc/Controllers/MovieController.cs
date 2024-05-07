using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;
using MovieStoreMvc.Repositories.Implementation;

namespace MovieStoreMvc.Controllers
{
    public class MovieController : Controller
    {

        private readonly IMovieService _movieservice;
        private readonly IFileServices _fileServices;
        private readonly IGenreService _genreService;


        public MovieController(IMovieService movieService, IFileServices fileServices, IGenreService genreService)
        {
            _movieservice = movieService;
            _fileServices = fileServices;
            _genreService = genreService;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }


        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileResult = _fileServices.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not save";
                    return View(model);
                }

                var imageName = fileResult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieservice.Add(model);
            if (result == true)
            {
                TempData["msg"] = "Data added successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Data Could not save";
                return View(model);
            }
        

    

        }


        public IActionResult Update(int id)
        {
            var model = _movieservice.GetById(id);
            var selectedGenres =  _movieservice.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenrelist = new MultiSelectList(_genreService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenrelist;
            return View(model);
        }
       


            [HttpPost]
        public IActionResult Update(Movie model)
        {
            var selectedGenres = _movieservice.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenrelist = new MultiSelectList(_genreService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenrelist;
            if (!ModelState.IsValid) 
                return View(model);
            if (model.ImageFile != null)
            {
                var fileResult = _fileServices.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not save";
                    return View(model);
                }

                var imageName = fileResult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieservice.Update(model);
            if (result == true)
            {
                TempData["msg"] = "Data updated successfully";
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                TempData["msg"] = "Data Could not update";
                return View(model);
            }

        }

        public IActionResult MovieList( )
        {
            var data = this._movieservice.List();
             
            return View(data);
        }


        public IActionResult Delete(int id)
        { 
            var result = _movieservice.Delete(id);
            
                return RedirectToAction(nameof(MovieList)); 
        }





    }
}
