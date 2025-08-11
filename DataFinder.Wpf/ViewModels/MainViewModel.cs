using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataFinder.Wpf.Models;

namespace DataFinder.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _url;
        private string _searchedText;
        private string _resultText;
        private readonly string _connectionString;

        public MainViewModel()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("DBConnect");

            SearchCommand = new RelayCommand(async _ => await ExecuteSearchAsync(), _ => !string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(SearchedText));
            OpenReportCommand = new RelayCommand(_ => OpenReport());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }

        public string SearchedText
        {
            get => _searchedText;
            set
            {
                if (_searchedText != value)
                {
                    _searchedText = value;
                    OnPropertyChanged(nameof(SearchedText));
                }
            }
        }

        public string ResultText
        {
            get => _resultText;
            set
            {
                if (_resultText != value)
                {
                    _resultText = value;
                    OnPropertyChanged(nameof(ResultText));
                }
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand OpenReportCommand { get; }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                var sourceText = await httpClient.GetStringAsync(Url);

                int count = 0;
                int index = sourceText.IndexOf(SearchedText, StringComparison.Ordinal);
                while (index != -1)
                {
                    count++;
                    index = sourceText.IndexOf(SearchedText, index + SearchedText.Length, StringComparison.Ordinal);
                }

                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseSqlServer(_connectionString)
                    .Options;

                using var context = new DataContext(options);
                var model = new UrlSearch
                {
                    UserId = 0,
                    SearchDate = DateTime.Now,
                    SearchUrl = Url?.Trim(),
                    SearchedTerm = SearchedText,
                    CountFoundTerm = count
                };
                context.UrlSearch.Add(model);
                await context.SaveChangesAsync();

                var sb = new StringBuilder();
                sb.AppendLine($"The Url  : {Url}");
                sb.AppendLine($"The Searched Term  : {SearchedText}");
                sb.AppendLine($"Count   : {count}");
                ResultText = sb.ToString();
            }
            catch (Exception)
            {
                ResultText = "The Url is not valid";
            }
        }

        private void OpenReport()
        {
            var window = new ReportWindow();
            window.Show();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}