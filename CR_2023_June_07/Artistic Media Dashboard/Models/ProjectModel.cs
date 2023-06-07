using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artistic_Media_Dashboard.Models
{

    public class LoginModel {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class ProjectModel
    {
        public long ProjectId { get; set; }
        [Required]
        [Range(1, 99999)]
        public int ProjectNumber { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public DateTime ProjectDate { get; set; }
        public string ProjectDateOnly { get { return ProjectDate.ToString("MM/dd/yyyy hh:00 tt"); } }
        public string strProjectDate { get { return ProjectDate.ToString("MM/dd/yyyy hh:00 tt"); } }
        public List<string> lstAttachments { get; set; }
        public string AttachmentName { get; set; }
        public List<AttachmentModel> lstAttachmentModel { get; set; }
        public string DispoAttachment { get; set; }
    }

    public class AttachmentModel
    {
        public string AttachmentName { get; set; }
        public int AttachmentType { get; set; }
        public string AttachmentURL { get; set; }
        public long ProjectAttachmentMappingId { get; set; }
    }

    public class ProjectWithAttachmentModel
    {
        public long ProjectId { get; set; }
        public int ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectDate { get; set; }
        public string AttachmentName { get; set; }
        public int AttachmentType { get; set; }
        public long ProjectAttachmentMappingId { get; set; }
    }
}