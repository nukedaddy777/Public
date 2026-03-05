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
 * Above the title, in the MainWindow.xaml one must catch left mouse clicks, MouseLeftButtonDown in the xaml as follows:
 *           mc:Ignorable="d" WindowStyle="None" 
 *           MouseLeftButtonDown="Window_MouseLeftButtonDown"
 *           
 * The mouse down drag and drop events are captured in the region #region Main Window Control below. 
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
 
 * You attach your own pages to containers of three basic types provided here, WpfGenericDialog ContentFrame.Content, 
 * WpfGenericUtilit UtilityContentFrame.Content and you can place tabs on a tabcontroller using FrameTabContainer.Content.
 * When needed, to assure that the left mouse button is NOT captured by decorating pages and tabs interfering with your
 * page, you can use the following in your page:
 * 
 * private void PageMouseDown(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
 *  
 *  where PageMouseDown captures the mouse down this(your Page object).MouseLeftButtonDown += PageMouseDown;
 *  in your Page constructor. 
 *  
 *  The snap on and off, the dialogs and the tab controller are all provided as examples of how to use the window management system which 
 *  is handled using the containers, and your page is fairly independent. There are several ways of managing your pages synergistically 
 *  with the container window system. 
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
        //customizable in the xaml. In order to have a nice Main Window with a large title, custom icons etc... just copy
        //this region into your main window with the xaml pattern shown above. 
        #region Main Window Control

        /// <summary>
        /// This captures the left mouse down button to drag and move the window. If not separated from your page, the 
        /// mouse down will be captured on the top level here, and the page will move. To break this feature in your page
        /// see comments above about PageMouseDown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// This minimizes the window and is attached to the minimization icon on the right hand of the xaml 
        /// presented above in the right hand stackpanel. Content="___" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// This maximizes the wind and is attached to the maximization icon on the right hand of the xaml presented above in the right hand stackpanel.
        /// Content="🗖"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// This method simply closes the main window. You can close other windows by registering windows with the GenericUtilityFactory and then calling close on them from the factory.
        /// Otherwise windows opened may linger as zombies.  Content="X"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        //The Specific Testing Code region below contains examples for testing window functionality.
        //Your coding will vary significantly based on your application, but this provides a demonstration of how to use the window management system
        //and how to attach pages to the various containers.

        #region Specific Testing Code

        int TestTabCount = 1;   //Index to show what dynamic tab number has been added to the window for convenience, not normal practice. 

        /// <summary>
        /// The Add button is a common header so its attachment is pulled out separately. In this method we create a headerPanel and attach
        /// various convenience buttons to it. Other methods are available, such as creating buttons on a header above the windows but 
        /// this system seems fairly intuitive and flexible. The headerPanel is attached to the TabItem.Header property of the tab item created. 
        /// The first button is an add button which adds tabs, but you could easily have a menu or other buttons here as well.
        /// </summary>
        /// <param name="tabTitle">Empty new TextBlock to attach the title</param>
        /// <returns>The StackPanel created</returns>
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
        /// as a window controller for demonstration. Various designs are possible, but this is a simple way to show how to attach 
        /// a page to the first tab and then add tabs from there. 
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
        /// <summary>
        /// In this method for each page we add onto the Stackpanel for the header of the tab, 
        /// we add a detach button which detaches the page into a new window and a close button which closes the tab. 
        /// The first tab is fixed and does not have these buttons for demonstration purposes, but you could easily add them to the first tab as well
        /// </summary>
        /// <param name="page">Your page content</param>
        /// <returns>The TabItem which contains the header that displays the dettach button and close x button</returns>
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

        /// <summary>
        /// When we detach from tab button, we place the page content from the tabcontainer into a new independent window 
        /// obtained from the WindowFactory. It is give the page and the title. The window's stackpanel is given a button 
        /// to return the page to the tab container and the window is shown. The closing event is also attached to remove 
        /// the window from the factory when it is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// On Window return, the page is obtained from the window content and then the window is closed and removed from the factory. 
        /// The page is then reattached to a new tab in the tab container. This snaps the page back into the tab container. You could easily 
        /// modify this to snap back into the original tab, but this is a demonstration of the window management system and how to move pages around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// In this method when a window is closed, we remove the window from the factory to prevent memory leaks and zombie windows. 
        /// If you do not do this, windows that are closed may linger in the background and cause problems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (sender == null) return;
            WpfGenericUtility? testWindow = sender as WpfGenericUtility;
            if (testWindow == null) return;
            object content = testWindow.UtilityContentFrame.Content;
            WindowFactory.Remove(content);
        }

        /// <summary>
        /// Handles the click event for a tab's close button, closing the corresponding tab in the container.
        /// </summary>
        /// <remarks>This method should be connected to the close button's click event within a tab item.
        /// If the sender does not correspond to a valid tab item, no action is taken.</remarks>
        /// <param name="sender">The source of the event, expected to be the close button within a tab item.</param>
        /// <param name="e">The event data associated with the button click.</param>
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