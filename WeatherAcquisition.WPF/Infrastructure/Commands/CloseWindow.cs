using System.Windows;
using WeatherAcquisition.WPF.Infrastructure.Commands.Base;

namespace WeatherAcquisition.WPF.Infrastructure.Commands
{
    internal class CloseWindow : Command
    {
        private static Window GetWindow(object p) => p as Window ?? App.FocusedWindow ??
            App.ActiveWindow;

        protected override bool CanExecute(object p) => GetWindow(p) != null;

        protected override void Execute(object p) => GetWindow(p)?.Close();
    }
}
