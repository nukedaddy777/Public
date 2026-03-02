/* JJ               @Copyright Richard Andrew Holland 2026 All Rights Reserved 

# __AndysWpfWindowManager__ is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# __AndysWpfWindowManager__ is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
 *
 * This mainwindow demonstrates the creation of a specialized main window with title and controls.This improves
 * visualization, the Main Window Control and xaml pattern below followed to produce a customizable window
 * 
 * Above the title, one must catch left mouse clicks, MouseLeftButtonDown in the xaml as follows:
 *           mc:Ignorable="d" WindowStyle="None" 
 *           MouseLeftButtonDown="Window_MouseLeftButtonDown"
 * 
 * Below the title is xaml as below that contains the control stackpanel and window title allow specialization.
 * Colors and Grid row definitions are dependent on application and taste. The normal title is covered over and 
 * the xaml below would be extended in the second row to contain the normal window data. The stackpanel has the buttons
 * and a MaxHeight should be set as appropriate for look and feel. 
 * 
 * <WindowChrome.WindowChrome>
        <WindowChrome GlassFrameThickness="0" CornerRadius="0" CaptionHeight="0"/>
    </WindowChrome.WindowChrome>
    <Border Background="Black" BorderBrush="Black" BorderThickness="2" CornerRadius="5" Padding="5">
        <Grid Background="#F0F0F0">
            <Grid.RowDefinitions>
                <RowDefinition Height="1*"/>
                <RowDefinition Height="10*"/>
            </Grid.RowDefinitions>
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="10*"/>
                    <ColumnDefinition Width="1*"/>
                </Grid.ColumnDefinitions>
                <TextBlock FontSize="18" VerticalAlignment="Center" Margin="10,0,0,0">
                 Andy's Window Manager Testing
                </TextBlock>
                <StackPanel Grid.Column="4" MaxHeight="20"
                     Orientation="Horizontal" HorizontalAlignment="Right">
                    <Button x:Name="BtnMinimize" Content="___" Margin="5,0,5,0"
                     BorderBrush="Transparent" Background="Transparent" Click="BtnMinimize_Click"/>
                    <Button x:Name="BtnMaximize" Content="🗖" Margin="5,0,5,0"
                 BorderBrush="Transparent" Background="Transparent" Click="BtnMaximize_Click"/>
                    <Button x:Name="BtnClose" Content="X" Margin="5,0,5,0"
                 BorderBrush="Transparent" Background="Transparent" Click="BtnClose_Click"/>
                </StackPanel>
            </Grid>
        </Grid>
    </Border>
 * 
 */
using System.ComponentModel;
using System.Reflection;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameTabContainer.Content = FirstTab();
        }

        GenericUtilityFactory WindowFactory = new GenericUtilityFactory();

        //The Main Window Control region below contains the standard WindowState mouse left button for dragging the window,
        //the minimize, the maximize and the close button override as the title is presented larger
        //customizable in the xaml.
        #region Main Window Control
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else WindowState = WindowState.Maximized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        //The Specific Testing Code region below contains examples for testing window functionality.
        #region Specific Testing Code

        int TestTabCount = 1;   //Index to show what dynamic tab number has been added to the window

        /// <summary>
        /// The Add button is a common header so its attachment is pulled out separately
        /// </summary>
        /// <param name="tabTitle">Empty new TextBlock to attach the title</param>
        /// <returns></returns>
        public StackPanel CreateHeader(TextBlock tabTitle)
        {
            StackPanel headerPanel = new StackPanel();                              //Stack Panel for header to contain buttons
            headerPanel.Orientation = Orientation.Horizontal;

            //First create new button to add more tabs as a test - this could copy existing page if applicable using sender
            Button newButton = new Button();
            newButton.Content = "+";                      // Or an Image
            newButton.Margin = new Thickness(4, 0, 4, 0); // Add some margin
            newButton.Padding = new Thickness(2);
            newButton.Background = Brushes.Transparent;
            newButton.BorderBrush = Brushes.Transparent;
            newButton.Click += BtnNewTab_Click;           // Attach event handler which is this button for subsequent additions
            headerPanel.Children.Add(newButton);          // Add button to header panel which will be added to the tab 

            tabTitle.Text = "Dynamic Tab " + TestTabCount.ToString();
            TestTabCount++;                            // Maintain count for testing purposes
            headerPanel.Children.Add(tabTitle);           //Add title to header

            return headerPanel;
        }

        /// <summary>
        /// This function provides the first tab. As we do not have a strip across the top, we have the first page set
        /// as a window controller for demonstration
        /// </summary>
        /// <returns>This returns the TabContainer for attachment to a Frame</returns>
        public TabCustomControl FirstTab()
        {
            TabCustomControl tabContainer = new TabCustomControl(this);

            TabItem tabItem = tabContainer.AddNewTabContent(new XPageTest()); //Create new Tab

            TextBlock tabTitle = new TextBlock();         // Text for tab title right now kind of boring
            StackPanel headerPanel = CreateHeader(tabTitle);

            tabTitle.Text = "First tab Window - fixed";
           
            tabItem.Header = headerPanel;
            return tabContainer;
        }

        public TabItem AttachPageToNewContainer(Page page)
        {
            TabCustomControl tabContainer = (TabCustomControl)FrameTabContainer.Content;    //The tab container initialized with window creation

            TabItem tabItem = tabContainer.AddNewTabContent(page);       //Create new Tab

            TextBlock tabTitle = new TextBlock();         // Text for tab title right now kind of boring
            StackPanel headerPanel = CreateHeader(tabTitle);

            //Only include the closing button after the first 
            if (tabContainer.TabDictionary.Count > 1)
            {
                tabTitle.Text += " ";
                Button detachButton = new Button();
                detachButton.Content = "↗️";
                detachButton.Margin = new Thickness(4, 0, 4, 0); // Add some margin
                detachButton.Padding = new Thickness(2);
                detachButton.Background = Brushes.Transparent;
                detachButton.BorderBrush = Brushes.Transparent;
                detachButton.Click += DetachButton_Click;      // Attach event handler
                headerPanel.Children.Add(detachButton);

                tabTitle.Text += " ";
                Button closeButton = new Button();        //A closing button is added, but we are going to keep the first button without a close
                closeButton.Content = "x";                      // Or an Image
                closeButton.Margin = new Thickness(4, 0, 4, 0); // Add some margin
                closeButton.Padding = new Thickness(2);
                closeButton.Background = Brushes.Transparent;
                closeButton.BorderBrush = Brushes.Transparent;
                closeButton.Click += TabCloseButton_Click;      // Attach event handler
                headerPanel.Children.Add(closeButton);
            }

            // Set the Header property of the TabItem to the StackPanel
            tabItem.Header = headerPanel;
            return tabItem;
        }

        /// <summary>
        /// This button adds a new tab container based on buttons on the Tabs - both the first tab and subsequent tabs for illustration
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        public void BtnNewTab_Click(object sender, RoutedEventArgs e)
        {
            if (FrameTabContainer.Content == null) return;
            AttachPageToNewContainer(new YPageTest());
        }

        public void DetachButton_Click(object sender, RoutedEventArgs e)
        {   
            if (WindowFactory == null) return;
            if (sender == null) return;
            TabCustomControl tabContainer = (TabCustomControl)FrameTabContainer.Content;
            TabItem? tabItem = tabContainer.FindSenderParent(sender);
            if (tabItem == null) return;
            Frame frame = (Frame) tabItem.Content;
            Page page = (Page) frame.Content;
            tabContainer.Close(tabItem);            //Close old window

            WpfGenericUtility window = WindowFactory.GetNewWindow(page.Title,page);
           
            StackPanel stackPanel = window.stackPanel;

            Button returnButton = new Button();        //A closing button is added, but we are going to keep the first button without a close
            returnButton.Content = "↙️";                      // Or an Image
            returnButton.Margin = new Thickness(4, 0, 4, 0); // Add some margin
            returnButton.Padding = new Thickness(2);
            returnButton.Background = Brushes.Transparent;
            returnButton.BorderBrush = Brushes.Transparent;
            returnButton.FontWeight = FontWeights.Bold;
            returnButton.Click += OnWindowReturn;      // Attach event handler
            
            stackPanel.Children.Add(returnButton);

            window.Closing += OnWindowClosing;
            window.Show();
        }

        public void OnWindowReturn(object sender, RoutedEventArgs e)
        {
            if (sender==null) return;
            Button btn                    =  sender as Button;
            if (btn== null) return; 

            Window window = Window.GetWindow(btn);

            WpfGenericUtility? testWindow = (WpfGenericUtility) window;
          
            if (testWindow == null) return;
            Page page = (Page) testWindow.UtilityContentFrame.Content;
            WindowFactory.Remove(testWindow);
            testWindow.Close();
            AttachPageToNewContainer(page);
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (sender == null) return;
            WpfGenericUtility? testWindow = sender as WpfGenericUtility;
            if (testWindow == null) return;
            object content = testWindow.UtilityContentFrame.Content;
            WindowFactory.Remove(content);
        }


        public void TabCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            TabCustomControl tabContainer = (TabCustomControl)FrameTabContainer.Content;
            TabItem? tabItem = tabContainer.FindSenderParent(sender);
            if (tabItem == null) return;
            tabContainer.Close(tabItem);

        }

       

        #endregion
    }
}
//SDG