using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WetherSideClient.Models;
using WetherSideClient.Services;
using WetherSideClient.Services;
using WetherSideClient.View;
using WetherSideClient.ViewModels;
using WetherSideClient.ViewModels;


namespace WetherSideClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // יצירת DBContext ו-Service
            var dbContext = new DataContext();
            var cityService = new CityService(dbContext);
            var httpClient = new HttpClient();
            var weatherApiService = new WeatherApiService(httpClient);

            DataContext = new MainViewModel(cityService,weatherApiService);
        }

    }
}