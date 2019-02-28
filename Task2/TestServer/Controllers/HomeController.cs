using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestServer.Logic;
using TestServer.Models;

namespace TestServer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var files = FileInfo.GetFiles(Server.MapPath("~/Files/"));

            ViewBag.Files = files;
            ViewBag.fileName = new SelectList(files, "File");

            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View("Upload");
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> uploads)
        {
            int loadedFiles = 0;
            DataToDB dataToDB = new DataToDB();

            foreach (var file in uploads)
            {
                if (file != null)
                {
                    string fileName = Server.MapPath("~/Files/" + System.IO.Path.GetFileName(file.FileName));

                    if (FileInfo.CanBeAdded(fileName))
                    {
                        file.SaveAs(fileName);
                        
                        if (dataToDB.SaveDataToDB(fileName))
                        {
                            loadedFiles++;
                        }
                        else
                        {
                            FileInfo.DeleteFile(fileName);
                        }
                    }
                }
            }

            ViewBag.FilesAmount = loadedFiles;
            return View("Success");
        }

        [HttpGet]
        public ActionResult FilesList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Display(string fileName)
        {
            File file = DBServant.GetFile(fileName);
            ViewBag.File = file;
            ViewBag.Classes = DBServant.GetClasses(file.FileId);
            ViewBag.TotalSum = DBServant.GetTotalSum();
            ViewBag.SumByClass = DBServant.GetSumByClass(file.FileId);

            return View();
        }
    }
}