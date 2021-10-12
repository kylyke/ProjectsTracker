using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Database.Concrete
{
    public partial class Note
    {
        [Key]
        public int Id { get; set; }
        public string DemandNote { get; set; }
        public DateTime CreateDate { get; set; }
        public int DemandId { get; set; }
    }
}
