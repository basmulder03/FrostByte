using Frostbyte.App.Pages;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Frostbyte.App;

public partial class App : Application
{
	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new CalendarPage());
	}
}