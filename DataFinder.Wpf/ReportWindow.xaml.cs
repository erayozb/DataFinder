using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataFinder.Wpf.Models;

namespace DataFinder.Wpf
{
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DBConnect");
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new DataContext(options);
            List<UrlSearch> items = context.UrlSearch
                .OrderByDescending(x => x.SearchDate)
                .ToList();
            ReportGrid.ItemsSource = items;
        }
    }
}