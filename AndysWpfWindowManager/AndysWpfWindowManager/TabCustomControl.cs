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
 *  This file presents a custom control 
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
    public class TabCustomControl : TabControl
    {
        public Window MasterWindow;                              //The master generic window containing this control 
        public Dictionary<Page, TabItem> TabDictionary
                            = new Dictionary<Page, TabItem>();   //The TabDictionary contains the framed page tab item
        public Dictionary<TabItem, Page> ReverseTabDictionary
                            = new Dictionary<TabItem, Page>();   //The reverse keyed dictionary for access based on tabItem
        public Boolean CloseOnLastTab { get; set; } = false;     //Indicates whether the Window should be closed when the last tab is emptied

        /// <summary>
        /// The instantiation of this as a child of a WpfGenericUtility, 
        /// The Container manages the closing of items created under this control.
        /// </summary>
        /// <param name="masterWindow"></param>
        public TabCustomControl(Window masterWindow)
        {
            MasterWindow = masterWindow;
            MasterWindow.Closing += CloseAll;
        }

        /// <summary>
        /// This method makes a new Page object framing the data and the new page within this
        /// encapsulating new tab item container. 
        /// </summary>
        /// <param name="page">Enapsulated data within the new Page</param>
        public TabItem AddNewTabContent(Page page)
        {
            TabItem newTabItem = new TabItem();                  //A new tab item to contain the Page framed content

            Frame frame = new Frame();                           //A new frame for the tab unit content
            frame.Content = page;                             //Assign the page to the frame's content
            newTabItem.Background = page.Background;
            newTabItem.Foreground = page.Foreground;          //Use default color scheme of page content
            newTabItem.Content = frame;                          //set the tab content to the frame
            Items.Add(newTabItem);                               //Add the frame to the Items within this control
            TabDictionary.Add(page, newTabItem);              //Create the dictionary reference of the new tab item associated with content
            ReverseTabDictionary.Add(newTabItem, page);
            return newTabItem;
        }
        /// <summary>
        /// Attach a new page content to a tab, and also allow specification of colors. 
        /// </summary>
        /// <param name="page">Page displayed in the tab</param>
        /// <param name="foreground">tab foreground color</param>
        /// <param name="background">tab background color</param>
        /// <returns></returns>
        public TabItem AddNewTabContent(Page page,System.Windows.Media.SolidColorBrush foreground, System.Windows.Media.SolidColorBrush background)
        {
            TabItem newTabItem = new TabItem();                  //A new tab item to contain the Page framed content

            Frame frame = new Frame();                           //A new frame for the tab unit content
            frame.Content = page;                             //Assign the page to the frame's content

            newTabItem.Content = frame;                          //set the tab content to the frame
            if (foreground != null)
            {
                newTabItem.Foreground = foreground;
            }
            if (background != null)
            {

                newTabItem.Background = background; 
            }         

            Items.Add(newTabItem);                               //Add the frame to the Items within this control
            TabDictionary.Add(page, newTabItem);              //Create the dictionary reference of the new tab item associated with content
            ReverseTabDictionary.Add(newTabItem, page);
            return newTabItem;
        }



        /// <summary>
        /// The public close is called by the passed Page if there is a tab item to 'X'
        /// </summary>
        /// <param name="unit">The unit making the call for reference into the dictionary and item</param>
        public void Close(Page unit)
        {
            TabItem tabItem = TabDictionary[unit];                 //Quickly obtain the tab item being closed
            ReverseTabDictionary.Remove(tabItem);                  //Remove from reverse dictionary
            TabDictionary.Remove(unit);                            //Remove from dictionary
            Items.Remove(tabItem);                                 //Remove tab item from tab control from submit or cancel
            if ((TabDictionary.Count < 1) && CloseOnLastTab)       //If last item and CloseOnLastTab is true, close containing master window
            {
                MasterWindow.Close();
            }
        }
        /// <summary>
        /// This is the close logic when the tab Item is known rather than the page
        /// and this uses the ReverseDictionary to find the page for removal from tab references
        /// </summary>
        /// <param name="tabItem"></param>
        public void Close(TabItem tabItem)
        {
            Page page = ReverseTabDictionary[tabItem];
            TabDictionary.Remove(page);
            ReverseTabDictionary.Remove(tabItem);
            Items.Remove(tabItem);
            if ((TabDictionary.Count < 1) && CloseOnLastTab)       //If last item and CloseOnLastTab is true, close containing master window
            {
                MasterWindow.Close();
            }
        }

        /// <summary>
        /// This even handler passed onto the master window clears the dictionary and Items to free garbage collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseAll(object sender, EventArgs e)
        {
            ReverseTabDictionary.Clear();
            TabDictionary.Clear();
            Items.Clear();
        }

        /// <summary>
        ///  Convenience routine to find a tab item parent based on a sender button, for use
        ///  when there is a button in the header. 
        /// </summary>
        /// <param name="sender">A button in a dependency tree, for use with buttons in headers</param>
        /// <returns>THe tab Item (or null) associated with button sender</returns>
        public TabItem? FindSenderParent(object sender)
        {
            if (sender == null) return null;
            DependencyObject current = sender as DependencyObject;
            while (current != null && !(current is TabItem))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as TabItem;
        }

    } //Class TabContainer
} //namespace
//SDG