namespace BlazorApp.Data
{
    public class UserContext
    {
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }

        public UserContext(string UserName = "none", bool isAdmin = false) 
        {
            this.UserName = UserName;
            this.IsAdmin = isAdmin;
        }
    }
}
