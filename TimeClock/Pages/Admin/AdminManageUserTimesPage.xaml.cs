using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TimeClock.Pages.Admin
{
    /// <summary>
    /// Interaction logic for AdminManageUserTimesPage.xaml
    /// </summary>
    public partial class AdminManageUserTimesPage : Page
    {
        //TODO Implement managing times
        //TODO Turn this application into a fully-featured HR nightmare, with payroll calculations and everything else.
        //TODO Maybe combine managing all user stuff into a tabbed interface.
        public AdminManageUserTimesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}