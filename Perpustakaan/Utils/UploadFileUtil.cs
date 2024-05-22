namespace Perpustakaan.Utils
{

    public class UploadResult
    {
        public UploadResult() { }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }

        public string FileType { get; set; }
    }

    public class UploadFileUtil
    {
        public readonly string _defaultPath = "uploads";
        private readonly IWebHostEnvironment _environment;

        public UploadFileUtil(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<UploadResult> Upload(IFormFile file, string? fileName, string? newPath) 
        {
            var folderPath = newPath ?? _defaultPath;
            string uploadsFolderPath = Path.Combine(_environment.ContentRootPath, folderPath);
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadResult() { 
                FileName = uniqueFileName,
                FilePath = $"{folderPath}/{uniqueFileName}",
                OriginalFileName = file.FileName,
                FileType = file.ContentType,
            };

        }
    }
}
