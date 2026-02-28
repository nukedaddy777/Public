//JJ                                 Copyright Richard Andrew Holland 2026 All Rights Reserved
/*
 *  The purpose of this file is to provide a factor dictionary lookup based on WpfGenericUtility page content. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndysWpfWindowManager
{
    public class GenericUtilityFactory
    {
        public Dictionary<object, WpfGenericUtility> WindowContentDict = new Dictionary<object, WpfGenericUtility>();

        public WpfGenericUtility? GetNewWindow(string name, object content)
        {
            if (WindowContentDict.ContainsKey(content)) { return null; }

            WpfGenericUtility returnWindow = new WpfGenericUtility(name, content);
            WindowContentDict.Add(content, returnWindow);

            return returnWindow;
        }

        public WpfGenericUtility? GetWindow(object content)
        {
            if (WindowContentDict.ContainsKey(content))
            {
                return WindowContentDict[content];
            }

            return null;
        }

        public void Remove(object content)
        {
            if (!WindowContentDict.ContainsKey(content)) return;
            WpfGenericUtility window = WindowContentDict[content];
            if (window != null)
            {
                WindowContentDict.Remove(content);
            }
        }

        public void Remove(WpfGenericUtility window)
        {
            if ((window != null) && (window.Content != null))
            {
                object content = window.UtilityContentFrame.Content;
                if (!WindowContentDict.ContainsKey(content)) return;
                WindowContentDict.Remove(content);
            }
        }

    } //GenericUtilityFactory
} //namespace
//SDG