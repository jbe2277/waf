namespace Waf.NewsReader.Domain
{
    public class UserAccount
    {
        public UserAccount(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }


        public string Id { get; }

        public string UserName { get; }
    }
}
