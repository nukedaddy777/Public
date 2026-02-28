//JJ                                     Copyright Richard Andrew Holland 2026 All Rights Reserved
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

namespace AndysWpfWindowManager
{
   
    public partial class WpfGenericDialog : Window
    {
        new public bool DialogResult;  //Always have the potential for a dialog result

        public GenericDialogSubmitProcessDelegates? SubmitDialogMethod { get; set; } = null;
        public GenericDialogSubmitProcessDelegates? CancelDialogMethod { get; set; } = null;
        public HotKeyDelegates? HotKey_Pressed { get; set; } = null;
        public PreviewMouseDelegates? MouseWheel_Scroll { get; set; } = null;

        public void EnableSubmit(Boolean set)
        {
            BtnSubmit.IsEnabled = set;
        }

        public string DisplayTitle
        {
            get { return txtBoxTitle.Text; }
            set
            {
                txtBoxTitle.Text = value;
                Title = value;
            }
        }

        public Object FrameContentData
        {
            get { return ContentFrame.Content; }
            set { ContentFrame.Content = value; }
        }

        public WpfGenericDialog(string title, object content)
        {
            InitializeComponent();
            txtBoxTitle.Text = title;
            FrameContentData = content;
        }

        public WpfGenericDialog(string title)
        {
            InitializeComponent();
            txtBoxTitle.Text = title;
        }

        #region Windows Control Buttons Top Bar Except Cancel handled below
        public void WpfGenericUtility_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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

        #endregion

        #region Submit and Cancel button actions
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (SubmitDialogMethod != null) SubmitDialogMethod(FrameContentData, this); //Submit based on window comment
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            if (CancelDialogMethod != null) CancelDialogMethod(FrameContentData, this);
            this.Close();
        }
        #endregion

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            HotKey_Pressed?.Invoke(sender, e);
        }

        private void ScrollActivated(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MouseWheel_Scroll?.Invoke(sender, e);
        }

    }
}
//SDG
