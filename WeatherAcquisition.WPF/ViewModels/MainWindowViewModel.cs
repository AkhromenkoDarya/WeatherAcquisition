using System.Collections.ObjectModel;
using System.Windows.Input;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WPF.Infrastructure.Commands;
using WeatherAcquisition.WPF.Services.Interfaces;
using WeatherAcquisition.WPF.ViewModels.Base;

namespace WeatherAcquisition.WPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;

        private readonly IDataService _dataService;

        private readonly IRepository<DataSource> _dataSourceRepository;

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

        public ObservableCollection<DataSource> DataSources { get; } = new();

        public MainWindowViewModel(IUserDialog userDialog, IDataService dataService, 
            IRepository<DataSource> dataSourceRepository)
        {
            _userDialog = userDialog;
            _dataService = dataService;
            _dataSourceRepository = dataSourceRepository;
        }

        #region Command LoadDataSourcesCommand - Команда загрузки данных по источникам

        /// <summary>
        /// Команда загрузки данных по источникам.
        /// </summary>
        private ICommand _loadDataSourcesCommand;

        /// <summary>
        /// Команда загрузки данных по источникам.
        /// </summary>
        public ICommand LoadDataSourcesCommand => _loadDataSourcesCommand ??=
            new RelayCommand(OnLoadDataSourcesCommandExecuted, CanLoadDataSourcesCommandExecute);

        /// <summary>
        /// Проверка возможности выполнения - Команда загрузки данных по источникам.
        /// </summary>
        private bool CanLoadDataSourcesCommandExecute(object p) => true;

        /// <summary>
        /// Логика выполнения - Команда загрузки данных по источникам.
        /// </summary>
        private async void OnLoadDataSourcesCommandExecuted(object p)
        {
            DataSources.Clear();

            foreach (DataSource source in await _dataSourceRepository.GetAll())
            {
                DataSources.Add(source);
            }
        }

        #endregion
    }
}
