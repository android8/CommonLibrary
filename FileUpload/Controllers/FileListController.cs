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
  public class FileListController : Controller
  {
   private readonly PCC_FITContext _context;
   public FileListController(PCC_FITContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> Index()
    {
      var fileuploadViewModel = LoadAllFiles();
      ViewBag.Message = TempData["Message"];
      return View(fileuploadViewModel);
    }

    [HttpPost]
    private async Task<FileUploadViewModel> LoadAllFilesAsync()
    {
      var viewModel = new FileUploadViewModel();
      viewModel.FilesOnDatabase = await _context.FilesOnDatabase.ToListAsync();
      viewModel.FilesOnFileSystem = await _context.FilesOnFileSystem.ToListAsync();
      return viewModel;
    }

    private FileUploadViewModel LoadAllFiles()
    {
      var viewModel = new FileUploadViewModel();
      viewModel.FilesOnDatabase = _context.FilesOnDatabase.ToList();
      viewModel.FilesOnFileSystem = _context.FilesOnFileSystem.ToList();
      return viewModel;
    }
  }
}
