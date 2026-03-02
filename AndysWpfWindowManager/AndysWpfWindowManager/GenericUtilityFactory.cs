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
 *
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