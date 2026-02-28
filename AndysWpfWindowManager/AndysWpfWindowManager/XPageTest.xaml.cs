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
    /// Interaction logic for XPageTest.xaml
    /// </summary>
    public partial class XPageTest : Page
    {
        public XPageTest()
        {
            InitializeComponent();
            txtBoxDescription.Text =
                "The purpose of this window is to demonstrate independent window control\n" +
                "capability. This test application uses this window as fixed on a dynamic\n" +
                "tab controller allowing pop out and pop in windows attached to tabs and\n" +
                "dialogs that can allow the application to run with the dialog open. There\n" +
                "is also a unique multi-tabbed dialog that is useful when operators have a\n" +
                "number of inputs that have grouped initial attributes for fast paced\n" +
                "operator action.\n\n"+
                "The listbox window on the left illustrates that the windows in the app\n"+
                "are manageable from a central location and one can focus, close or retrieve\n"+
                "desparate windows in a number of ways";
        }

        WpfGenericUtility? mdialog = null;   //Generic window utility fram for trucks, only one per location
      
        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            Button btn = sender as Button;
            Point buttonPosition = btn.PointToScreen(new Point(-100, -100));

            YPageTest page = new YPageTest();

            TabDialogController? container = null;   //Tab contained that allows adding dialogs to tabs within window
            PageTabDialogBar? unit = null;         //Tabbed unit in containing pageTruckEdit
            Boolean showFlag = false;

            if (mdialog==null)
            {
                mdialog = new WpfGenericUtility("Multi Dialog");

                double scaleFactor = PresentationSource.FromVisual(btn).CompositionTarget.TransformToDevice.M11;

                // Adjust the position for the display scale
                double adjustedX = buttonPosition.X / scaleFactor;
                double adjustedY = buttonPosition.Y / scaleFactor;

                buttonPosition.X = adjustedX;
                buttonPosition.Y = adjustedY;

                mdialog.Left = buttonPosition.X;                                                                //Position 
                mdialog.Top = buttonPosition.Y;

                container = new TabDialogController(mdialog);  //The Tab Dialog Container allows PageTabDialogUnit pages to be put on tabs
                container.CloseOnLastTab = true;                  //When the last tab is used, the window is closed, so multiDialog.IsLoaded=false
                container.CloseAllDialogMethod = CloseAllWindow;  //Manage TruckEditWindowDictionary
                mdialog.FrameContentObj = container;         //The tabControl of dialogs is loaded into the Generic Window frame
                unit = container.AddNewTabContent(page, mSubmit);
                unit.DisplayTitle = container.TabDict.Count.ToString();
                mdialog.Left = buttonPosition.X;
                mdialog.Top = buttonPosition.Y;
                showFlag = true;
            } else
            {
                container = (TabDialogController) mdialog.FrameContentObj;        //Grab associated tab dialog container 
                unit = container.AddNewTabContent(page, mSubmit);                    //Create new tabbed content of truck page and associated submit event
                unit.DisplayTitle = container.TabDict.Count.ToString();        //Tab number for current new tab item 
            }

            if (showFlag)
            {
                mdialog.Show();
            }
            else
            {
                mdialog.Activate();
            }

        }
        
        //Empty no function
        private void mSubmit (object content, PageTabDialogBar unit)
        {

        }

        /// <summary>
        /// This routine closes dialog windows just for trucks, closing all the tabs.
        /// </summary>
        /// <param name="content"></param>
        public void CloseAllWindow(object content)
        {
            if (content == null) return;
            TabDialogController container = content as TabDialogController;
            if (container == null) return;
            WpfGenericUtility? window = container.MasterWindow;
            mdialog = null;
        }

        private void btnSingleDialog_Click(object sender, RoutedEventArgs e)
        { 
            if (sender == null) return;
            Button btn = sender as Button;
            Point buttonPosition = btn.PointToScreen(new Point(-100, -100));

            YPageTest page = new YPageTest();
            WpfGenericDialog dialog = new WpfGenericDialog("Single Dialog " + page.Title, page);

            double scaleFactor = PresentationSource.FromVisual(btn).CompositionTarget.TransformToDevice.M11;
            
            // Adjust the position for the display scale
            double adjustedX = buttonPosition.X / scaleFactor;
            double adjustedY = buttonPosition.Y / scaleFactor;

            buttonPosition.X = adjustedX;
            buttonPosition.Y = adjustedY;

            dialog.Left = buttonPosition.X;                                                                //Position 
            dialog.Top = buttonPosition.Y;

            dialog.SubmitDialogMethod = Empty;

            dialog.Show();

        }

        private void Empty(object content, WpfGenericDialog window)
        {

        }

   
    }
}
