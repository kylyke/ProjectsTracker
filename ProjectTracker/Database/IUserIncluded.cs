using ProjectTracker.Database.Concrete;

namespace ProjectTracker.Database
{
    public interface IUserIncluded
    {
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
