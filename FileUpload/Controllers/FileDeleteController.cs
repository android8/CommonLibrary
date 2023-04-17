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
  public class FileDeleteController : Controller
  {
   private readonly PCC_FITContext _context;
   public FileDeleteController(PCC_FITContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> DeleteFileFromFileSystem(int id)
    {
      var file = _context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefault();
      if (file == null) return null;
      if (System.IO.File.Exists(file.FilePath))
      {
        System.IO.File.Delete(file.FilePath);
      }
      _context.FilesOnFileSystem.Remove(file);
      _context.SaveChanges();
      TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
      return RedirectToAction("Index", "FileList");
    }

    public async Task<IActionResult> DeleteFileFromDatabase(int id)
    {
      var file = _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefault();
      _context.FilesOnDatabase.Remove(file);
      _context.SaveChanges();
      TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
      return RedirectToAction("Index", "FileList");
    }
  }
}
