using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjectTracker.Database.Concrete
{
    public partial class Demand //: IUserIncluded
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string CompanyAbout { get; set; }
        public DateTime CreateDate { get; set; }
        public string Demander { get; set; }
        public string Execution { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Data { get; set; }
        public bool? Situation { get; set; }

        public int Status { get; set; }

        //[ForeignKey("ProjectId")]
        public Project Project { get; set; }

        //[ForeignKey("UserId")]
        public User User { get; set; }

    }
}
