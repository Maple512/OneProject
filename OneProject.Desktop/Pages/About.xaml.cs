namespace OneProject.Desktop.Pages;

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
using System.Windows.Shapes;
using OneProject.Desktop.Themes;
using OneProject.Desktop.ViewModels;

public partial class About : UserControl
{
    public About()
    {
        DataContext = new AboutModel();

        InitializeComponent();
    }
}
