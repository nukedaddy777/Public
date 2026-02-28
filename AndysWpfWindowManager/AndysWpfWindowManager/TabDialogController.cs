//JJ                          Copyright Richard Holland 2026 All Rights Reserved
/*
 *  This file contains the TabDialogController that is used for dialogs where there are common attributes and one can open
 *  several tabbed dialogs for fast paced input data. For example when an operator has a number of entries of a similar type
 *  (a number of people entering a facility door in a group) and one makes a quick count and then enters the data. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AndysWpfWindowManager
{
    /// <summary>
    /// This delegate is used for the Submit button on PageTabDialogBar that frame content
    /// data (usually pages).
    /// </summary>
    /// <param name="content">Framed content that contains data for processing when cast to appropriate type</param>
    /// <param name="unit">The framing unit making the submit call</param>
    public delegate void DialogProcessDelegate(object content, PageTabDialogBar unit);
    public delegate void DialogClassAllDelegate(object content);

    /// <summary>
    /// This class provides a dialog that contains tabs within it for busy operations, where one may have a number
    /// of entries with common attributes in a fast paced environment, and rather than having several dialogs opened
    /// there is one master window with several dialogs.
    /// </summary>
    public class TabDialogController : TabControl
    {
        public WpfGenericUtility MasterWindow { get; set; }               //The master generic window containing this control 
        public Dictionary<PageTabDialogBar, TabItem> TabDict
                            = new Dictionary<PageTabDialogBar, TabItem>();  //The TabDictionary contains the framing dialog page and tab item
        public Boolean CloseOnLastTab { get; set; } = false;                 //Indicates whether the Window should be closed when the last tab is emptied
        public DialogClassAllDelegate? CloseAllDialogMethod { get; set; } = null;


        /// <summary>
        /// The instantiation of this as a child of a WpfGenericUtility, not a WindowGenericDialog that is only designed for 
        /// one dialog. The Container manages the closing of dialogs created under this control.
        /// </summary>
        /// <param name="masterWindow"></param>
        public TabDialogController(WpfGenericUtility masterWindow)
        {
            MasterWindow = masterWindow;
            MasterWindow.Closing += CloseAll;
        }

        /// <summary>
        /// This method adds a new PageDialogUnit page that contains a frame with the passed
        /// content (usually a page) within.
        /// </summary>
        /// <param name="content">Data accessed and framed by a new PageTabDialogBar page</param>
        /// <returns>A handle to the new PageTabDialogBar</returns>
        public PageTabDialogBar AddNewTabContent(object content)
        {
            return NewTabItem(content);
        }

        /// <summary>
        /// This method adds a new PageDialogUnit page that contains a frame with the passed
        /// content (usually a page) within. It also immediately attaches the submit method for 
        /// processing the content data of the framed page
        /// </summary>
        /// <param name="content">Data accessed and framed by a new PageTabDialogBar page</param>
        /// <param name="submitProcessMethod">Method to process when "Submit" button hit on PageTabDialogBar framing page</param>
        /// <returns></returns>
        public PageTabDialogBar AddNewTabContent(object content, DialogProcessDelegate submitProcessMethod)
        {
            PageTabDialogBar barUnit = NewTabItem(content);
            barUnit.SubmitDialogMethod = submitProcessMethod;
            return barUnit;
        }

        /// <summary>
        /// This method makes a new PageTabDialogBar object framing the data and the new page within this
        /// encapsulating new tab item container. 
        /// </summary>
        /// <param name="content">Enapsulated data within the new PageTabDialogBar from</param>
        /// <returns>The new PageTabDialogBar object created</returns>
        private PageTabDialogBar NewTabItem(object content)
        {
            PageTabDialogBar barUnit = new PageTabDialogBar(this);    //Create a new dialog frame
            barUnit.FrameContentData = content;                        //The content passed is framed and presented within the dialog unit
            TabItem newTabItem = new TabItem();                  //A new tab item to contain the Page tab dialog and framed content

            Frame frame = new Frame();                                //A new frame for the tab unit content
            frame.Content = barUnit;                                     //Assign the unit to the frame's content

            newTabItem.Content = frame;                               //set the tab content to the frame 
            Items.Add(newTabItem);                                    //Add the fram to the Items within this control
            TabDict.Add(barUnit, newTabItem);                      //Create the dictionary reference of the new tab associated with the new unit
            return barUnit;
        }

        /// <summary>
        /// The public close is called by the passed PageTabDialogBar when a submit or cancel
        /// button is hit. 
        /// </summary>
        /// <param name="barUnit">The unit making the call for reference into the dictionary and item</param>
        public void Close(PageTabDialogBar barUnit)
        {
            TabItem tabItem = TabDict[barUnit];                 //Quickly obtain the tab item being closed
            TabDict.Remove(barUnit);                            //Remove from dictionary
            Items.Remove(tabItem);                                 //Remove tab item from tab control from submit or cancel
            if ((TabDict.Count < 1) && CloseOnLastTab)       //If last item and CloseOnLastTab is true, close containing master window
            {
                if (CloseAllDialogMethod != null)
                {
                    CloseAllDialogMethod(this);
                }
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
            if (CloseAllDialogMethod != null)
            {
                CloseAllDialogMethod(this);
            }
            TabDict.Clear();
            Items.Clear();
        }

    } //TabDialogContainer
}
//SDG