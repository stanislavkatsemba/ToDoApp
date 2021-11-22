namespace ToDoApp.Web.Common.Authentication
{
    public class UserInfo
    {
        public UserInfo(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
