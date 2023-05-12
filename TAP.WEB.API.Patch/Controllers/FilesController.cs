using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Web;
using System.Configuration;

namespace TAP.WEB.API.Patch.Controllers
{    
    public class FilesController : ApiController
    {
        private readonly string _filesDirectoryPath;


        public FilesController()
        {
            _filesDirectoryPath = ConfigurationManager.AppSettings["FilesDirectoryPath"];
        }


        [HttpGet]
        public List<FileDetails> GetFileDetails()
        {
           
            try
            {
                var files = Directory.GetFiles(_filesDirectoryPath, "*.*", SearchOption.AllDirectories)
                .Select(f => new FileDetails
                {
                    FullFileName = f,
                    FileName = Path.GetFileName(f),
                    LastModified = System.IO.File.GetLastWriteTime(f)
                }).ToList();

                return files;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class FileDetails
        {
            public string FullFileName { get; set; }
            public string FileName { get; set; }
            public DateTime LastModified { get; set; }
        }
    }
}