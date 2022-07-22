using System;
using WeatherAcquisition.WPF.Infrastructure.Commands.Base;

namespace WeatherAcquisition.WPF.Infrastructure.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(p => execute(), canExecute is null ? (Func<object, bool>)null : p => canExecute())
        {

        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecute(object p) => _canExecute?.Invoke(p) ?? true;

        protected override void Execute(object p) => _execute(p);
    }
}
