namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int maxPageSize = 50;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
        public string Gender { get; set; }
        public int UserId { get; set; }
    }
}