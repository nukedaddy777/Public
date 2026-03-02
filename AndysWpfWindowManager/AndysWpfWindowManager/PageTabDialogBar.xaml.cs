/* JJ               @Copyright Richard Andrew Holland 2026 All Rights Reserved 

# __AndysWpfWindowManager__ is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# __AndysWpfWindowManager__ is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details. -->
*/
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
    /// Interaction logic for PageTabDialogBar.xaml
    /// </summary>
    public partial class PageTabDialogBar : Page
    {
        public bool DialogResult;  //Always have the potential for a dialog result
        public DialogProcessDelegate? SubmitDialogMethod { get; set; } = null;
        public HotKeyDelegates? HotKey_Pressed { get; set; } = null;
        public PreviewMouseDelegates? MouseWheel_Scroll { get; set; } = null;
        public TabDialogController TabContainer { get; set; }

        public string DisplayTitle
        {
            get { return Title; }
            set
            {
                Title = value;
                if (TabContainer != null)
                {
                    TabItem tabItem = TabContainer.TabDict[this];
                    tabItem.Header = Title;
                }
            }
        }

        public Object FrameContentData
        {
            get { return ContentFrame.Content; }
            set { ContentFrame.Content = value; }
        }

        public PageTabDialogBar(TabDialogController tabContainer)
        {
            InitializeComponent();
            TabContainer = tabContainer;
        }

        #region Submit and Cancel button actions

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (SubmitDialogMethod != null) SubmitDialogMethod(FrameContentData, this); //Submit based on window comment
            TabContainer.Close(this);

        }

        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            TabContainer.Close(this);
        }
        #endregion

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            HotKey_Pressed?.Invoke(sender, e);
        }

        private void ScrollActivated(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MouseWheel_Scroll?.Invoke(sender, e);
        }

    } //PageTabDialogUnit
} //namespace
//SDG