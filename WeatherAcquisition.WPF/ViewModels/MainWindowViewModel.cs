using WeatherAcquisition.WPF.Services.Interfaces;
using WeatherAcquisition.WPF.ViewModels.Base;

namespace WeatherAcquisition.WPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;

        private readonly IDataService _dataService;

        #region Title : string - Заголовок окна

        /// <summary>
        /// Заголовок окна
        /// </summary>
        private string _title = "Main Window";

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _title;

            set => Set(ref _title, value);
        }

        #endregion

        #region Status : string - Статус

        /// <summary>
        /// Статус
        /// </summary>
        private string _status = "Ready";

        /// <summary>
        /// Статус
        /// </summary>
        public string Status
        {
            get => _status;

            set => Set(ref _status, value);
        }

        #endregion

        public MainWindowViewModel(IUserDialog userDialog, IDataService dataService)
        {
            _userDialog = userDialog;
            _dataService = dataService;
        }
    }
}
