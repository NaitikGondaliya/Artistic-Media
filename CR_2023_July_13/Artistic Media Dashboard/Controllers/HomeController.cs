using Artistic_Media_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            //if (Session["UserID"] == null )
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ProjectRepository repository = new ProjectRepository();
            List<DashboardResponse> lstDashboard = repository.DashboardData();
            return View(lstDashboard);
        }

        public ActionResult Sedinte()
        {
            //if (Session["UserID"] == null )
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ProjectRepository repository = new ProjectRepository();
            List<DashboardResponse> lstDashboard = repository.DashboardData();
            return View(lstDashboard);
        }



        #region Auth
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Authenticate(LoginModel loginModel)
        {
            ProjectRepository repository = new ProjectRepository();
            if (repository.Authenticate(loginModel) > 0)
            {
                Session["UserID"] = loginModel.Username.ToLower();
                return Json(1);
            }
            return Json(0);
        } 
        #endregion

        #region Project
        [HttpGet]
        public ActionResult Project()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ProjectRepository repository = new ProjectRepository();
            List<ProjectModel> lstProject = repository.GetAllProject();
            return View(lstProject);
        }
        public ActionResult AddProject()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult DeleteProject(long id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ProjectRepository repository = new ProjectRepository();
            repository.DeleteProject(id);
            return RedirectToAction("Project", "Home");
        }
        public ActionResult EditProject(long id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ProjectRepository repository = new ProjectRepository();
            ProjectModel objProject = repository.GetById(id);
            return View(objProject);
        }
        [HttpPost]
        public ActionResult SaveProject(ProjectModel projectModel)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
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
        }
        [HttpPost]
        public string UploadFile(long projectid, string filename, int attachmentType, HttpPostedFileBase file)
        {
            if (Session["UserID"] == null)
            {
                RedirectToAction("Login", "Home");
                return string.Empty;
            }
            else
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
                            string formatedfilename=Regex.Replace(filename, @"\s+", "");
                            var supportedTypes = new[] { "pdf", "xls", "xlsx","doc","docx", "jpg" };
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
                                _path = Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid), $"{formatedfilename}.{fileExt}");
                                Session["Path"] = _path;
                                bool exists = Directory.Exists(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                                if (!exists)
                                {
                                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                                }

                                ProjectRepository repository = new ProjectRepository();
                                if (repository.AddProjectAttachment(projectid, attachmentType, $"UploadedFiles/{DateTime.Now.ToString("MMddyyyy")}/{projectid}/{formatedfilename}.{fileExt}", filename))
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
        [HttpPost]
        public string ManageFile(long projectid, long projectAttachmentMappingId, int attachmentType, string filename, HttpPostedFileBase file)
        {
            if (Session["UserID"] == null)
            {
                RedirectToAction("Login", "Home");
                return string.Empty;
            }
            else
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
                            string formatedfilename = Regex.Replace(filename, @"\s+", "");
                            var supportedTypes = new[] { "pdf", "xls", "xlsx", "doc", "docx", "jpg" };
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
                                _path = Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid), $"{formatedfilename}.{fileExt}");
                                Session["Path"] = _path;
                                bool exists = Directory.Exists(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                                if (!exists)
                                {
                                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/UploadedFiles"), DateTime.Now.ToString("MMddyyyy"), Convert.ToString(projectid)));
                                }

                                ProjectRepository repository = new ProjectRepository();
                                if (projectAttachmentMappingId > 0)
                                {
                                    repository.DeleteProjectAttachment(projectAttachmentMappingId);
                                }
                                if (repository.AddProjectAttachment(projectid, attachmentType, $"UploadedFiles/{DateTime.Now.ToString("MMddyyyy")}/{projectid}/{formatedfilename}.{fileExt}",filename))
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
        #endregion

        public ActionResult ReleaseDisposition()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ProjectRepository repository = new ProjectRepository();
            List<ReleaseDispositionModel> lstProject = repository.GetReleaseDispo();
            return View(lstProject);
        }
        public ActionResult AddReleaseDisposition()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult SaveReleaseDisposition(ReleaseDispositionModel releaseDispositionModel)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ProjectRepository repository = new ProjectRepository();
                    //ProjectModel projectModel = new ProjectModel();
                    // projectModel.ProjectName = "TEST";
                    // projectModel.ProjectNumber = 1;
                    // projectModel.ProjectDate = DateTime.Now;
                    long id = repository.AddReleaseDisposition(releaseDispositionModel);
                    return Json(id);
                }
                return Json(0);
            }
        }
        public ActionResult DeleteReleaseDisposition(long id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ProjectRepository repository = new ProjectRepository();
            repository.DeleteReleaseDispo(id);
            return RedirectToAction("ReleaseDisposition", "Home");
        }

        public string UploadReleaseDisposition(DateTime dispodate, string dispodesc, string filename, HttpPostedFileBase file)
        {
            if (Session["UserID"] == null)
            {
                RedirectToAction("Login", "Home");
                return string.Empty;
            }
            else
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
                            string formatedfilename = Regex.Replace(filename, @"\s+", "");
                            var supportedTypes = new[] { "pdf", "xls", "xlsx", "doc", "docx", "jpg" };
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
                                _path = Path.Combine(Server.MapPath("~/UploadedFiles/ReleaseDisposition"), $"{formatedfilename}.{fileExt}");
                                Session["Path"] = _path;
                                bool exists = Directory.Exists(Path.Combine(Server.MapPath("~/UploadedFiles/ReleaseDisposition")));
                                if (!exists)
                                {
                                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/UploadedFiles/ReleaseDisposition")));
                                }

                                ProjectRepository repository = new ProjectRepository();
                                if (repository.AddReleaseDisposition(new ReleaseDispositionModel() {AttachmentName= $"UploadedFiles/ReleaseDisposition/{formatedfilename}.{fileExt}",DispoDate=dispodate,ReleaseDispositionId=0,DispoDisc=dispodesc,filename=filename })>0)
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
                catch(Exception ex)
                {
                    return "File upload failed!!";
                }
            }
        }

    }
}

