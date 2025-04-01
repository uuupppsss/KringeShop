using KringeShopLib.Model;

namespace KringeShopWebClient.Services
{
    public class UserService
    {
        public UserDTO CurrentUser { get; private set; }

        public void SetCurrentUser(UserDTO user)
        {
            CurrentUser = user;
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }
}
