
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityPointOfInterest.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtentionContenttypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtentionContenttypeProvider)
        {
            _fileExtentionContenttypeProvider = fileExtentionContenttypeProvider ??
                throw new System.ArgumentNullException(nameof(fileExtentionContenttypeProvider));
        } 

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "builders-online-series-deploy-infrastructure-as-a-code-on-aws-nelli-lovchikova.pdf";

            if(!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if(!_fileExtentionContenttypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            //validate file length to limit the large files being uploaded
            if(file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("Please upload a valid file.");
            }

            //Create a file path
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"uploaded_file_{Guid.NewGuid()}.pdf");

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully.");
        }
    }
}