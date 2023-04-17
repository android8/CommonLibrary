using FileUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Controllers
{
  public class FileDownloadController : Controller
  {
   private readonly PCC_FITContext _context;
   public FileDownloadController(PCC_FITContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> DownloadFileFromDatabase(int id)
    {
      var file = _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefault();
      if (file == null) return null;
      return File(file.Data, file.FileType, file.Name + file.Extension);
    }

    public async Task<IActionResult> DownloadFileFromFileSystem(int id)
    {
      var file = _context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefault();
      if (file == null) return null;
      var memory = new MemoryStream();
      using (var stream = new FileStream(file.FilePath, FileMode.Open))
      {
        await stream.CopyToAsync(memory);
      }
      memory.Position = 0;
      return File(memory, file.FileType, file.Name + file.Extension);
    }
  }
}
