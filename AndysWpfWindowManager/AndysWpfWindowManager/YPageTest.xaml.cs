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

            // Prevent window drag when clicking on page
            this.MouseLeftButtonDown += PageMouseDown;
        }

        private void PageMouseDown(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}
