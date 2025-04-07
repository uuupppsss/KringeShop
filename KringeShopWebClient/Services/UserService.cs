using KringeShopLib.Model;

namespace KringeShopWebClient.Services
{
    public class UserService
    {
        public delegate void UserHandler();
        public event UserHandler? Notify;
        public UserDTO? CurrentUser { get; private set; }

        public void SetCurrentUser(UserDTO user)
        {
            CurrentUser = user;
            Notify?.Invoke();
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }
}
