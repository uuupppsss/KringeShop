using KringeShopLib.Model;

namespace KringeShopWeb.Services
{
    public class UserService
    {
        public User CurrentUser { get; private set; }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }
}
