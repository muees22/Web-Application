using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;
using MovieStoreMvc.Repositories.Implementation;

namespace MovieStoreMvc.Controllers
{
    public class GenreController : Controller
    {
      
        private readonly IGenreService _genreservice;
        public GenreController(IGenreService genreservice)
        {
            _genreservice = genreservice;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if (ModelState.IsValid)
            { 
                var result = _genreservice.Add(model);
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
            return View(model);
        }


        public IActionResult Edit(int id)
        {

            var data = _genreservice.GetById(id);
            return View(data);
        }
       


            [HttpPost]
        public IActionResult Edit(Genre model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            var result = _genreservice.Update(model);
            if (result == true)
            {
                TempData["msg"] = "Data updated successfully";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Data Could not update";
                return View(model);
            }

        }

        public IActionResult GenreList( )
        {
            var data = this._genreservice.List().ToList();

            return View(data);
        }


        public IActionResult Delete(int id)
        { 
            var result = _genreservice.Delete(id);
            
                return RedirectToAction(nameof(GenreList)); 
        }





    }
}
