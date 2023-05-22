using Artistic_Media_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Artistic_Media_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            ProjectRepository repository = new ProjectRepository();
            List<DashboardResponse> lstDashboard = repository.DashboardData();
            return View(lstDashboard);
        }

        [HttpGet]
        public ActionResult Project()
        {
            ProjectRepository repository = new ProjectRepository();
            List<ProjectModel> lstProject = repository.GetAllProject();
            return View(lstProject);
        }
        public ActionResult AddProject()
        {
            return View();
        }

        public ActionResult DeleteProject(long id)
        {
            ProjectRepository repository = new ProjectRepository();
            repository.DeleteProject(id);
            return RedirectToAction("Project", "Home");
        }


        [HttpPost]
        public JsonResult SaveProject(ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {
                ProjectRepository repository = new ProjectRepository();
                //ProjectModel projectModel = new ProjectModel();
                // projectModel.ProjectName = "TEST";
                // projectModel.ProjectNumber = 1;
                // projectModel.ProjectDate = DateTime.Now;
                long id = repository.AddProject(projectModel);
                return Json(id);
            }
            return Json(0);
        }

        [HttpPost]
        public string UploadFile(long projectid, string filename, HttpPostedFileBase file)
        {
            string message = string.Empty;
            string _path = string.Empty;
            try
            {
                if (Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
                {
                    foreach (string objfile in Request.Files)
                    {
                        var _file = Request.Files[objfile];
                        var supportedTypes = new[] { "pdf", "xls","xlsx" };
                        var fileExt = System.IO.Path.GetExtension(_file.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt))
                        {
                            message = "Invalid File Extention!!";
                            return message;
                        }
                        if (_file.ContentLength > 0 && supportedTypes.Contains(fileExt))
                        {
                            string _FileName = Path.GetFileName(_file.FileName);
                            _FileName = _FileName.Replace(" ", "_");
                            _path = Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid), $"{filename}.{fileExt}");
                            Session["Path"] = _path;
                            bool exists = Directory.Exists(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                            if (!exists)
                            {
                                Directory.CreateDirectory(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                            }

                            ProjectRepository repository = new ProjectRepository();
                            if (repository.AddProjectAttachment(projectid, $"UploadedFiles/{DateTime.Now.ToString("MMddyyyy")}/{projectid}/{filename}.{fileExt}"))
                            {
                                _file.SaveAs(_path);
                            }
                            message = "File Uploaded Successfully!!";
                            return message;
                        }
                    }
                }
                return "File And Name Are Mandatory!!";
            }
            catch
            {
                return "File upload failed!!";
            }
        }

    }
}