namespace BlazorApp.Data
{
    public class UserSent
    {
        public string usn { get; set; }
        public string pwd { get; set; }

        public UserSent(string usn, string pwd)
        {
            this.usn = usn;
            this.pwd = pwd;
        }
    }

}
