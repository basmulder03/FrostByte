using FrostByte.Application.Services;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AuthPage> _logger;

    public AuthPage(IAuthService auth, ILogger<AuthPage> logger)
    {
        _auth = auth ?? throw new ArgumentNullException(nameof(auth));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        _logger.LogDebug("WebView handler changed, initializing WebView2 if applicable.");
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI && _webView.Handler?.PlatformView is WebView2 wv2)
            {
                _logger.LogDebug("WebView2 platform view detected, initializing WebView2.");
                await InitializeWebView2Async(wv2);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize WebView2: {Message}", ex.Message);
            // Optionally, you can show an error message to the user
            await DisplayAlert("Error", "Failed to initialize the authentication page. Please try again later.", "OK");
        }
    }

    private async Task InitializeWebView2Async(WebView2 wv2)
    {
        _logger.LogDebug("Ensuring WebView2 core is initialized.");
        await wv2.EnsureCoreWebView2Async();
        wv2.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
    }

    private async void OnNavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        _logger.LogDebug("Navigation completed with URL: {Url}, Success: {Success}", sender.Source, args.IsSuccess);
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

            _logger.LogInformation("Session cookie found. Storing session cookie with expiration: {Expires}", expires);
            await _auth.StoreSessionCookieAsync(session.Value, expires);
            _logger.LogInformation("Session cookie stored successfully. Navigating back to main page.");
            MainThread.BeginInvokeOnMainThread(async void () =>
            {
                try
                {
                    _logger.LogInformation("Closing authentication page and returning to main page.");
                    await Navigation.PopModalAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error navigating back to main page: {Message}", e.Message);
                    // Optionally, you can show an error message to the user
                    await DisplayAlert("Error", "An error occurred while navigating back. Please try again.", "OK");
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing navigation completion: {Message}", ex.Message);
            // Optionally, you can show an error message to the user
            await DisplayAlert("Error", "An error occurred while processing the authentication. Please try again.", "OK");
        }
    }
}
