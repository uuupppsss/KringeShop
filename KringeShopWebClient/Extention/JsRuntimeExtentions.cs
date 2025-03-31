using Microsoft.JSInterop;
namespace KringeShopWebClient.Extention
{
    public static class JsRuntimeExtentions
    {
        public static ValueTask ToastrSuccess(this IJSRuntime jSRuntime, string message)
        {
            return jSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static ValueTask ToastrError(this IJSRuntime jSRuntime, string message)
        {
            return jSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
