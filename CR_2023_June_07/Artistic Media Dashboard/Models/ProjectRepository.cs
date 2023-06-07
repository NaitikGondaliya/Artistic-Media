using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Artistic_Media_Dashboard.Models
{
    public class ProjectRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add Employee details    
        public long AddProject(ProjectModel obj)
        {


            connection();
            SqlCommand com = new SqlCommand("ManageProject", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProjectId", obj.ProjectId);
            com.Parameters.AddWithValue("@ProjectNumber", obj.ProjectNumber);
            com.Parameters.AddWithValue("@ProjectDate", obj.ProjectDate);
            com.Parameters.AddWithValue("@ProjectName", obj.ProjectName);
            com.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;

            con.Open();
            int i = com.ExecuteNonQuery();
            long id = Convert.ToInt64(com.Parameters["@id"].Value);
            con.Close();
            if (i >= 1)
            {

                return id;

            }
            else
            {

                return 0;
            }


        }
        public bool AddProjectAttachment(long projectId, int attachmentType, string filepath)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewAttachment", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@projectId", projectId);
            com.Parameters.AddWithValue("@attachmentType", attachmentType);
            com.Parameters.AddWithValue("@AttachmentName", filepath);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteProjectAttachment(long projectAttachmentMappingId)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteAttachmentById", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@projectAttachmentMappingId", projectAttachmentMappingId);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ProjectModel> GetAllProject()
        {
            connection();
            List<ProjectModel> lstProject = new List<ProjectModel>();


            SqlCommand com = new SqlCommand("GetProjects", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                lstProject.Add(

                    new ProjectModel
                    {
                        ProjectId = Convert.ToInt64(dr["ProjectId"]),
                        AttachmentName = Convert.ToString(dr["AttachmentName"]),
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"])
                    }
                    );
            }


            List<ProjectModel> consolidateProduct = new List<ProjectModel>();
            if (lstProject != null && lstProject.Any())
            {
                foreach (long projectId in lstProject.Select(x => x.ProjectId).Distinct())
                {
                    ProjectModel objProject = lstProject.Where(x => x.ProjectId == projectId).FirstOrDefault();
                    foreach (string objProjAtt in lstProject.Where(x => x.ProjectId == projectId).Select(x => x.AttachmentName).ToList())
                    {
                        if (objProject.lstAttachmentModel == null)
                        {
                            objProject.lstAttachmentModel = new List<AttachmentModel>();
                        }
                        objProject.lstAttachmentModel.Add(
                            new AttachmentModel()
                            {
                                AttachmentName = objProjAtt.Split('/').Last(),
                                AttachmentURL = HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty) + "/" + objProjAtt
                            });
                    };
                    consolidateProduct.Add(objProject);
                }
            }

            return consolidateProduct;
        }
        public ProjectModel GetById(long projectId)
        {
            connection();
            ProjectModel objProject = new ProjectModel();
            List<ProjectWithAttachmentModel> lstProjectwithAttachments = new List<ProjectWithAttachmentModel>();
            SqlCommand com = new SqlCommand("GetProjectById", con);
            com.Parameters.AddWithValue("@projectId", projectId);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                lstProjectwithAttachments.Add(

                    new ProjectWithAttachmentModel
                    {
                        ProjectId = Convert.ToInt64(dr["ProjectId"]),
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"]),
                        ProjectAttachmentMappingId = dr.IsNull("ProjectAttachmentMappingId") ? 0 : Convert.ToInt32(dr["ProjectAttachmentMappingId"]),
                        AttachmentName = dr.IsNull("ProjectAttachmentMappingId") ? null : Convert.ToString(dr["AttachmentName"]),
                        AttachmentType = dr.IsNull("ProjectAttachmentMappingId") ? 0 : Convert.ToInt32(dr["AttachmentType"])
                    }
                    );
            }

            objProject.ProjectId = lstProjectwithAttachments.FirstOrDefault().ProjectId;
            objProject.ProjectName = lstProjectwithAttachments.FirstOrDefault().ProjectName;
            objProject.ProjectNumber = lstProjectwithAttachments.FirstOrDefault().ProjectNumber;
            objProject.ProjectDate = lstProjectwithAttachments.FirstOrDefault().ProjectDate;

            foreach (ProjectWithAttachmentModel objProj in lstProjectwithAttachments)
            {
                if (objProject.lstAttachmentModel == null)
                {
                    objProject.lstAttachmentModel = new List<AttachmentModel>();
                }
                objProject.lstAttachmentModel.Add(new AttachmentModel()
                {
                    AttachmentName = objProj.AttachmentName,
                    AttachmentType = objProj.AttachmentType,
                    ProjectAttachmentMappingId = objProj.ProjectAttachmentMappingId,
                    AttachmentURL = HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty) + "/" + objProj.AttachmentName
                });
            }
            return objProject;
        }
        public bool UpdateEmployee(ProjectModel obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateProject", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProjectId", obj.ProjectId);
            com.Parameters.AddWithValue("@ProjectDate", obj.ProjectDate);
            com.Parameters.AddWithValue("@ProjectName", obj.ProjectName);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteProject(long Id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteProjectById", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProjectId", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        public List<DashboardResponse> DashboardData()
        {
            connection();
            List<DashboardResponse> dashboardResponses = new List<DashboardResponse>();
            List<ProjectModel> lstProject = new List<ProjectModel>();


            SqlCommand com = new SqlCommand("GetProjectsForDashboard", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                lstProject.Add(

                    new ProjectModel
                    {
                        ProjectId = Convert.ToInt64(dr["ProjectId"]),
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"]),
                        DispoAttachment = dr.IsNull("DispoAttachment") ?
                        string.Empty : HttpContext.Current.Request.Url.ToString().Contains("Dashboard")
                        ? (HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty) + "/" + Convert.ToString(dr["DispoAttachment"])) :
                        (HttpContext.Current.Request.Url.ToString() + Convert.ToString(dr["DispoAttachment"]))
                        //Convert.ToString(dr["DispoAttachment"])
                    }
                    );
            }

            foreach (ProjectModel objProjectModel in lstProject)
            {
                connection();
                SqlCommand com1 = new SqlCommand("GetProjectsAttachment", con);
                com1.CommandType = CommandType.StoredProcedure;
                com1.Parameters.AddWithValue("@ProjectId", objProjectModel.ProjectId);
                SqlDataAdapter da1 = new SqlDataAdapter(com1);
                DataTable dt1 = new DataTable();

                con.Open();
                da1.Fill(dt1);
                con.Close();

                List<string> tempAttachmentName = new List<string>();
                //Bind EmpModel generic list using dataRow     
                foreach (DataRow dr in dt1.Rows)
                {
                    // tempAttachmentName.Add(HttpContext.Current.Request.Url.ToString() + Convert.ToString(dr["AttachmentName"]));
                    tempAttachmentName.Add(HttpContext.Current.Request.Url.ToString().Contains("Dashboard")
                        ? (HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty) + "/" + Convert.ToString(dr["AttachmentName"])) :
                        (HttpContext.Current.Request.Url.ToString() + Convert.ToString(dr["AttachmentName"])));
                    if (objProjectModel.lstAttachments == null)
                    {
                        objProjectModel.lstAttachments = new List<string>();
                    }
                    objProjectModel.lstAttachments = tempAttachmentName;
                }
            }

            List<string> lstDates = lstProject.OrderByDescending(x => x.ProjectDate).Select(x => x.ProjectDateOnly).Distinct().ToList();
            foreach (var objDate in lstDates)
            {
                dashboardResponses.Add(new DashboardResponse() { ProjectDate = objDate, lstProject = lstProject.Where(x => x.ProjectDateOnly == objDate).ToList() });
            }

            return dashboardResponses;

        }
        public int Authenticate(LoginModel loginModel)
        {
            connection();
            int userid = 0;
            SqlCommand com = new SqlCommand("AuthenticateUser", con);
            com.Parameters.AddWithValue("@UserName", loginModel.Username);
            com.Parameters.AddWithValue("@Password", loginModel.Password);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {
                userid = Convert.ToInt32(dr["UserId"]);
                //lstProjectwithAttachments.Add(

                //    new ProjectWithAttachmentModel
                //    {
                //        ProjectId = Convert.ToInt64(dr["ProjectId"]),
                //        ProjectName = Convert.ToString(dr["ProjectName"]),
                //        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                //        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"]),
                //        ProjectAttachmentMappingId = Convert.ToInt32(dr["ProjectAttachmentMappingId"]),
                //        AttachmentName = Convert.ToString(dr["AttachmentName"])
                //    }
                //    );
            }
            return userid;
        }

        public long AddReleaseDisposition(ReleaseDispositionModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("AddReleaseDispo", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DispoDate", obj.DispoDate);
            com.Parameters.AddWithValue("@AttachmentName", obj.AttachmentName);
            com.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;

            con.Open();
            int i = com.ExecuteNonQuery();
            long id = Convert.ToInt64(com.Parameters["@id"].Value);
            con.Close();
            if (i >= 1)
            {

                return id;

            }
            else
            {

                return 0;
            }
        }
        public List<ReleaseDispositionModel> GetReleaseDispo()
        {
            connection();
            List<ReleaseDispositionModel> lstrd = new List<ReleaseDispositionModel>();


            SqlCommand com = new SqlCommand("GetReleaseDispo", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                lstrd.Add(

                    new ReleaseDispositionModel
                    {
                        ReleaseDispositionId = Convert.ToInt32(dr["ReleaseDispoId"]),
                        DispoDate = Convert.ToDateTime(dr["DispoDate"]),
                        AttachmentName = Convert.ToString(dr["AttachmentName"]),
                        AttachmentURL = HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty) + "/" + Convert.ToString(dr["AttachmentName"]),
                    }
                    );
            }
            return lstrd;
        }

        public bool DeleteReleaseDispo(long Id)
        {
            connection();
            SqlCommand com = new SqlCommand("DeleteReleaseDispo", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@dispoId", Id);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ReleaseDispositionModel
    {
        public int ReleaseDispositionId { get; set; }
        public DateTime DispoDate { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentURL { get; set; }
    }

    public class DashboardResponse
    {
        public string ProjectDate { get; set; }
        public List<ProjectModel> lstProject { get; set; }
    }


}