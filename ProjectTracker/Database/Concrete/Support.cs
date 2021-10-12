using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Database.Concrete
{
    public partial class Support
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Demander { get; set; }
        public string Execution { get; set; }
        public double Situation { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
