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
            SqlCommand com = new SqlCommand("AddNewProject", con);
            com.CommandType = CommandType.StoredProcedure;
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

        public bool AddProjectAttachment(long projectId,string filepath)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewAttachment", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@projectId", projectId);
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
        //To view employee details with generic list     
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
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"])
                    }
                    );
            }

            return lstProject;
        }
        //To Update Employee details    
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
        //To delete Employee details    
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
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        ProjectDate = Convert.ToDateTime(dr["ProjectDate"]),
                        ProjectNumber = Convert.ToInt32(dr["ProjectNumber"])
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
                    tempAttachmentName.Add(HttpContext.Current.Request.Url.ToString() + Convert.ToString(dr["AttachmentName"]));
                    if (objProjectModel.lstAttachments==null)
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
    }

    public class DashboardResponse
    {
        public string ProjectDate { get; set; }
        public List<ProjectModel> lstProject { get; set; }
    }


}