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
  public class FileUploadController : Controller
  {
   private readonly PCC_FITContext _context;
   public FileUploadController(PCC_FITContext context)
    {
      _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
    {
      foreach (var file in files)
      {
        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
        bool basePathExists = System.IO.Directory.Exists(basePath);
        if (!basePathExists) Directory.CreateDirectory(basePath);
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var filePath = Path.Combine(basePath, file.FileName);
        var extension = Path.GetExtension(file.FileName);
        if (!System.IO.File.Exists(filePath))
        {
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }
          var fileModel = new FilesOnFileSystem
          {
            CreatedOn = DateTime.UtcNow,
            FileType = file.ContentType,
            Extension = extension,
            Name = fileName,
            Description = description,
            FilePath = filePath
          };
          _context.FilesOnFileSystem.Add(fileModel);
          _context.SaveChanges();
        }
      }

      TempData["Message"] = "File successfully uploaded to File System.";
      return RedirectToAction("Index", "FileList");
    }

    [HttpPost]
    public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
    {
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var fileModel = new FilesOnDatabase
        {
          CreatedOn = DateTime.UtcNow,
          FileType = file.ContentType,
          Extension = extension,
          Name = fileName,
          Description = description
        };
        using (var dataStream = new MemoryStream())
        {
          await file.CopyToAsync(dataStream);
          fileModel.Data = dataStream.ToArray();
        }
        _context.FilesOnDatabase.Add(fileModel);
        _context.SaveChanges();
      }
      TempData["Message"] = "File successfully uploaded to Database";
      return RedirectToAction("Index","FileList");
    }
  }
}
