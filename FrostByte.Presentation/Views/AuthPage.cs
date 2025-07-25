using FrostByte.Application.Services;
using Grid = Microsoft.Maui.Controls.Grid;

#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
#endif

namespace FrostByte.Presentation.Views;

public partial class AuthPage : ContentPage
{
    private readonly IAuthService _auth;
    private readonly WebView _webView;

    public AuthPage(IAuthService auth)
    {
        _auth = auth ?? throw new ArgumentNullException(nameof(auth));
        Title = "Sign in to Advent of Code";

        _webView = new WebView
        {
            Source = "https://adventofcode.com/auth/login",
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        // Hook up when the native control is ready
        _webView.HandlerChanged += OnHandlerChanged;

        Content = new Grid { Children = { _webView } };
    }

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI && _webView.Handler?.PlatformView is WebView2 wv2)
            {
                await InitializeWebView2Async(wv2);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task InitializeWebView2Async(WebView2 wv2)
    {
        await wv2.EnsureCoreWebView2Async();
        wv2.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
    }

    private async void OnNavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        try
        {
            // When AoC redirects to "/{year}" after login
            if (!sender.Source.StartsWith("https://adventofcode.com/") ||
                sender.Source == "https://adventofcode.com/auth/login") return;
            var cookies = await sender.CookieManager.GetCookiesAsync("https://adventofcode.com");
            var session = cookies.FirstOrDefault(c => c.Name == "session");
            if (session is null) return;
            DateTimeOffset? expires = null;
            try
            {
                expires = DateTimeOffset.FromUnixTimeSeconds((long)session.Expires);
            }
            catch
            { /* ignore invalid expires */
            }

            await _auth.StoreSessionCookieAsync(session.Value, expires);
            await Shell.Current.Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
