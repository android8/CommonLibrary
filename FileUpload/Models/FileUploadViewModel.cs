using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Models
{
  public class FileUploadViewModel
  {
    public List<FilesOnFileSystem> FilesOnFileSystem { get; set; }
    public List<FilesOnDatabase> FilesOnDatabase { get; set; }
  }
}
