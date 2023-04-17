using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Models
{
  public class FilesOnDatabase : FileModel
  {
    public byte[] Data { get; set; }
  }
}
