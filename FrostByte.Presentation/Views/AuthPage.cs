using System;
using System.Linq;
using FrostByte.Application.Services;
using Microsoft.Maui.Controls;
using Grid = Microsoft.Maui.Controls.Grid;

#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
#endif

namespace FrostByte.Presentation.Views;

public class AuthPage : ContentPage
{
    private readonly IAuthService _auth;
    private readonly WebView _webView;

    public AuthPage(IAuthService auth)
    {
        _auth = auth ?? throw new ArgumentNullException(nameof(auth));
        Title = "Sign in to Advent of Code";

        _webView = new WebView
        {
            Source = "https://adventofcode.com/auth/signin",
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        // Hook up when the native control is ready
        _webView.HandlerChanged += OnHandlerChanged;

        Content = new Grid { Children = { _webView } };
    }

    private void OnHandlerChanged(object? sender, EventArgs e)
    {
#if WINDOWS
        if (_webView.Handler?.PlatformView is WebView2 wv2)
        {
            InitializeWebView2(wv2);
        }
#endif
    }

#if WINDOWS
    private void InitializeWebView2(WebView2 wv2)
    {
        wv2.EnsureCoreWebView2Async()
            .GetAwaiter().GetResult();

        wv2.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
    }

    private void OnNavigationCompleted(
        CoreWebView2 sender,
        CoreWebView2NavigationCompletedEventArgs args)
    {
        var uri = sender.Source;
        // ignore the sign-in form itself
        if (uri.EndsWith("/auth/signin", StringComparison.OrdinalIgnoreCase))
            return;

        // must be AoC domain
        if (!uri.StartsWith("https://adventofcode.com/", StringComparison.OrdinalIgnoreCase))
            return;

        var cookies = sender.CookieManager.GetCookiesAsync("https://adventofcode.com").GetAwaiter().GetResult();
        var session = cookies.FirstOrDefault(c => c.Name == "session");
        if (session == null) return;

        DateTimeOffset? expires = DateTimeOffset.FromUnixTimeSeconds((long)session.Expires);

        _auth.StoreSessionCookieAsync(session.Value, expires).GetAwaiter().GetResult();
        Shell.Current.Navigation.PopModalAsync().GetAwaiter().GetResult();
    }
#endif
}
