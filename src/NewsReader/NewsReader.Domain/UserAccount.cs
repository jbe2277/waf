namespace Jbe.NewsReader.Domain
{
    public class UserAccount
    {
        public UserAccount(string userName)
        {
            UserName = userName;
        }


        public string UserName { get; }
    }
}
