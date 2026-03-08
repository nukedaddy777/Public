What is this?
This is a ready-to-use demonstration of a clean, custom WPF window management system I built for real-world apps that need flexible, non-blocking UI.
You can:

Pop tabs out into independent floating windows
Snap them back into the tab strip with one click
Open multi-tabbed dialogs that stay open while the rest of the app runs
Use a full-featured Color Picker palette (256-color grid + sliders + hex input) that integrates directly with any page

Everything is built with plain WPF (no third-party docking libraries), so it’s lightweight, easy to understand, and perfect for learning or extending.
Features

Pop-out / Retrieve windows – Drag or click to detach any tab into its own resizable window
Multi-tab dialogs – Open several related dialogs in one floating window (great for fast data entry)
Color Picker Window – 256-color palette, RGB/Alpha sliders, live preview, hex input, and easy integration
Factory pattern – Never duplicate windows for the same content
Hotkey & mouse-wheel hooks – Ready for custom shortcuts
DPI-aware positioning – Looks sharp on any monitor
GPL-3.0 licensed – Free for friends, students, and personal projects

Quick Demo 

Run the app
Click + to add new tabs (YPageTest pages)
Click the ↗️ arrow on any tab → it pops out as a separate window
Click the ↙️ button in the floating window → it snaps back into the tab strip

Color Application
Click any color swatch in the Material list → the Color Picker opens with live preview
Change colors → they update instantly in the main list

Perfect for showing how a “poor man’s docking system” can feel modern without heavy libraries.
Getting Started (5 minutes)
1. Clone the repo
Bashgit clone https://github.com/yourusername/AndysWpfWindowManager.git
cd AndysWpfWindowManager
2. Open in Visual Studio

Open WpfMenuTestApp.sln (or the solution file)
Target .NET Framework 4.8 or .NET 6+ WPF (both work)
Press F5 to run

3. Try the Color Picker

Click any material color button in the list on the left
The picker appears with a 256-color grid + sliders
Pick a color → it updates the swatch instantly

How to Use in Your Own Project

Copy the AndysWpfWindowManager and/or ColorPicker folder into your solution
Add a reference (or just include the files)
Create your own Page (e.g. MyDataPage.xaml)
Attach it exactly like the demo:

C#// In your MainWindow or any container for window manager
var page = new MyDataPage();
AttachPageToNewContainer(page);   // ← pops out / retrieves automatically

Full examples are in MainWindow.xaml.cs (look for AttachPageToNewContainer and DetachButton_Click).
Want to Add Drag-and-Drop?
The foundation is already there (see the commented drag handlers in MainWindow.xaml.cs).
If you want the “drag any tab header to pop out” version with a semi-transparent ghost preview, just uncomment and tweak the three 
small methods I left in the code. It takes ~30 lines and feels like Visual Studio or Chrome. Future Ideas (optional)

Hook into a database (MariaDB/MySQL ready)
Real-time change notifications via RabbitMQ
Drag-and-drop between windows
NuGet package for one-click install

License
GPL-3.0 – feel free to use, modify, and share with friends.
Just keep the copyright notice and license file.

Made for friends who want clean, detachable WPF UIs without paying for DevExpress or Telerik.
Questions? Open an issue or ping me at nukedaddy777@gmail.com
Happy coding!
— Andy (Richard Andrew Holland)
March 2026
