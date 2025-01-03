using System.Windows;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString = "Host=sequentially-in-mackerel.data-1.use1.tembo.io;Port=5432;Username=postgres;Password=Be2gkgU6omPjbX3P;Database=postgres";

        public MainWindow()
        {
            InitializeComponent();

        }

    }
}