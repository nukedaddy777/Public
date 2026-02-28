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

namespace AndysWpfWindowManager
{
    /// <summary>
    /// Interaction logic for YPageTest.xaml
    /// </summary>
    public partial class YPageTest : Page
    {
        public YPageTest()
        {
            InitializeComponent();
            txtBoxDescription.Text =
                "This window is an example of a dynamic detachable and retrievable page\n" +
                "The checkboxes simply allow one to see that the state of the window is\n"+
                "not changed. Pages are used for these windows, and are encapsulated in\n"+
                "tabs or when detached, separate windows.";
        }
    }
}
