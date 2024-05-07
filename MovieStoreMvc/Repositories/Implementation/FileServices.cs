using MovieStoreMvc.Repositories.Abstract;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment _environment;
        public FileServices(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

     

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = _environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // Check the allowed extention
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtintions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtintions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extintions are allowed", string.Join(",", allowedExtintions));
                    return new Tuple<int, string>(0, msg); 
                }
                string uniqueString = Guid.NewGuid().ToString();
                // Trying to create a unique filename 
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
                
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }
         
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwpath = _environment.WebRootPath;
                var path = Path.Combine(wwwpath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true; 
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
