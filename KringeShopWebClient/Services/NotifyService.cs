namespace KringeShopWebClient.Services
{
    public class NotifyService
    {
        public event Action<string> OnNotifyError;
        public event Action<string> OnNotifySuccess;

        public void NotifyError(string message)
        {
            OnNotifyError?.Invoke(message);
        }

        public void NotifySuccess(string message)
        {
            OnNotifySuccess?.Invoke(message);
        }
    }
}
