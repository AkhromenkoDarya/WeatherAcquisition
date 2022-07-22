using System;
using WeatherAcquisition.WPF.Infrastructure.Commands.Base;

namespace WeatherAcquisition.WPF.Infrastructure.Commands
{
    internal class RelayCommandAsync : Command
    {
        private readonly ActionAsync<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommandAsync(ActionAsync execute, Func<bool> canExecute = null)
            : this(async p => await execute(), canExecute is null ? (Func<object, bool>)null :
                p => canExecute())
        {

        }

        public RelayCommandAsync(ActionAsync<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecute(object p) => _canExecute?.Invoke(p) ?? true;

        protected override void Execute(object p) => _execute(p);
    }
}