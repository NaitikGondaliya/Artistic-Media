using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artistic_Media_Dashboard.Models
{
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
    }
}