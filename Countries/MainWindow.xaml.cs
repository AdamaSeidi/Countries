using Countries.Models;
using Countries.Services;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Countries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region
        private List<Country> Countries;
        private NetworkService networkService;
        private ApiService apiService;
        private DialogService dialogService;
        #endregion
        public MainWindow()
        {

            InitializeComponent();
            networkService = new NetworkService();
            apiService = new ApiService();
           // DataService = new DataService();
            dialogService = new DialogService();

            LoadCountry();
        }

        private async void LoadCountry()
        {

            var client = new HttpClient();

            var connection = networkService.CheckConnection();

            if (connection.IsSuccess)
            {
                await LoaApiCountry();
            }

            DowloadImage();
            ListBoxCountryList.ItemsSource = Countries;
        }
        private async Task LoaApiCountry()
        {
            var response = await apiService.GetCountries("http://restcountries.eu", "/rest/v2/all");
            Countries = (List<Country>)response.Result;
        } 

        private void DowloadImage()
        {
            DirectoryInfo path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            if (!Directory.Exists(path + @"\Image"))
            {
                Directory.CreateDirectory(path + @"\Image");
            }

            using (WebClient client = new WebClient())
            {
               
                foreach (Country country in Countries)
                {
                    try
                    {
                        string file = path + @"\Image" + $"\\{country.name}.svg";

                        client.DownloadFile(country.flag, file);

                        SvgDocument svgDocument = SvgDocument.Open(file);

                        Bitmap bitmap = svgDocument.Draw(100, 100);

                        bitmap.Save(path + @"\Image" + $"\\{country.name}.png", ImageFormat.Png);

                        country.Photo = new Uri(path + @"\Image" + $"\\{country.name}.png");
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        private void CountrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CountrySearch.Text))
            {
                List<Country> SearchCountries = new List<Country>();

                foreach (Char item in CountrySearch.Text)
                {
                    SearchCountries = Countries.Where(n => n.name.ToLower().StartsWith(CountrySearch.Text.ToLower())).ToList();
                }

                ListBoxCountryList.ItemsSource = SearchCountries;
            }
            else
            {
                ListBoxCountryList.ItemsSource = Countries;
            }
        }

        private void ListBoxCountryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxCountryList.SelectedItem != null)
            {
                Country country = (Country)ListBoxCountryList.SelectedItem;
               
            }
        }
    }
}
