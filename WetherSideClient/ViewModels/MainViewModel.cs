using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WetherSideClient.Models;
using WetherSideClient.Services;
using CommunityToolkit.Mvvm.Input;

namespace WetherSideClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private readonly ICityService _cityService;
        private readonly WeatherApiService _weatherApiService;

        public ObservableCollection<HistoryCity> HistoryCities { get; set; } = new();
        public ObservableCollection<FavoriteCity> FavoriteCities { get; set; } = new();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<ForecastDay> _forecast = new();
        public ObservableCollection<ForecastDay> Forecast
        {
            get => _forecast;
            set => SetProperty(ref _forecast, value);
        }

        //button of XML
        public IAsyncRelayCommand SearchCommand { get; }
        public IAsyncRelayCommand<string> AddFavoriteCommand { get; }
        public IAsyncRelayCommand<int> RemoveFavoriteCommand { get; }
        public IAsyncRelayCommand<HistoryCity> SearchHistoryCityCommand { get; set; }

        public MainViewModel(ICityService cityService, WeatherApiService weatherApiService)
        {
            _cityService = cityService;
            _weatherApiService = weatherApiService;

            SearchCommand = new AsyncRelayCommand(SearchCity, () => !string.IsNullOrWhiteSpace(SearchText));
            AddFavoriteCommand = new AsyncRelayCommand<string>(AddFavorite);
            RemoveFavoriteCommand = new AsyncRelayCommand<int>(RemoveFavorite);
            SearchHistoryCityCommand = new AsyncRelayCommand<HistoryCity>(SearchHistoryCity);


            _ = LoadData();
        }

        private async Task LoadData()
        {
            HistoryCities.Clear();
            foreach (var city in await _cityService.GetHistory())
                HistoryCities.Add(city);

            FavoriteCities.Clear();
            foreach (var fav in await _cityService.GetFavorites())
                FavoriteCities.Add(fav);
        }

        private async Task SearchHistoryCity(HistoryCity city)
        {
            if (city == null)
                return;

            //var forecastCach = await _weatherApiService.GetForecast(city.Name);
            //if (forecastCach != null && forecastCach.Days.Any())
            //{
            //    Forecast.Clear();
            //    foreach (var day in forecastCach.Days)
            //        Forecast.Add(day);

            //}
            //else
            //{
            //    SearchText = city.Name;
            //    await SearchCity();
            //}

            SearchText = city.Name;
            await SearchCity();
        }

        private async Task SearchCity()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return;

                var city = new HistoryCity { Name = SearchText, LastSearchedAt = DateTime.Now };
                await _cityService.AddCityToHistory(city);

                await LoadData();

                var forecastResponse = await _weatherApiService.GetForecast(SearchText);
                if (forecastResponse != null)
                {
                    Forecast.Clear();
                    foreach (var day in forecastResponse.Days)
                    {
                        Forecast.Add(day);
                    }
                }

                SearchText = string.Empty;
            }
            catch (Exception e)
            {
                throw new Exception("The exception is: "+e);

            }
        }

        private async Task AddFavorite(string? cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName)) return;

            await _cityService.AddFavorite(cityName);
            await LoadData();
        }

        private async Task RemoveFavorite(int id)
        {
            await _cityService.RemoveFavorite(id);
            await LoadData();
        }
    }
}
