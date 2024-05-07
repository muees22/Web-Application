using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;
using System.Security.Policy;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext _ctx;
        public GenreService(DatabaseContext ctx)
        {
            _ctx = ctx; 
        }
        public bool Add(Genre model)
        {
            try
            { 
                _ctx.Genre.Add(model);
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
                _ctx.Genre.Remove(data);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Genre GetById(int id)
        {
           return _ctx.Genre.Find(id);
        }

        public IQueryable<Genre> List()
        {
             var data = _ctx.Genre.AsQueryable();
            return data;
        }

        public bool Update(Genre model)
        {
            try
            { 
                _ctx.Genre.Update(model);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      
    }
}
