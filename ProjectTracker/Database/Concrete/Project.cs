using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Database.Concrete
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int UserId { get; set; }
        public string CompanyAbout { get; set; }
        public bool? UpdateAgreement { get; set; }
        public DateTime? UpdateAgreementStartDate { get; set; }
        public DateTime? UpdateAgreementFinisDate { get; set; }
        public virtual User User { get; set; }
        public string PhoneNumber { get; set; }
        public bool? MaintenanceAgreement { get; set; }
        public DateTime? MaintenanceAgreementStartDate { get; set; }
        public DateTime? MaintenanceAgreementFinishDate { get; set; }
        public string? ConnectionAdress { get; set; }

    }
}
