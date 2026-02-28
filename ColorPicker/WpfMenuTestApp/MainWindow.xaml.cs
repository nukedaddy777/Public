// JJ
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMenuTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListMaterials.Add(new MaterialBase(Brushes.Chocolate, "Material1"));
            ListMaterials.Add(new MaterialBase(Brushes.Bisque, "Material2"));
            ListMaterials.Add(new MaterialBase(Brushes.LightBlue, "Material3"));
            ListMaterials.Add(new MaterialBase(Brushes.LightGreen, "Material4"));
            LbMaterialx.ItemsSource = ListMaterials;
        }
        ObservableCollection<MaterialBase> ListMaterials = new ObservableCollection<MaterialBase>();

        private void Select_Click(object sender, RoutedEventArgs e)
        {
           if (sender==null) return;
           Button button = sender as Button;
           if ((button==null) || (button.DataContext == null)) return;
           MaterialBase material = button.DataContext as MaterialBase;
           if (material == null) return;
           txtBoxConsole.Text += "\n"+ material.MaterialName + " Selected \n";
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            foreach (MaterialBase material in ListMaterials)
            {
                txtBoxConsole.Text += material.MaterialName + "\n";
            }
        }

        private void ColorSelector_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            Button button = sender as Button;
            if ((button == null) || (button.DataContext == null)) return;
            MaterialBase material = button.DataContext as MaterialBase;
            if (material == null) return;

            // Pass your current brush (optional)
            var picker = new ColorPickerWindow(material.MaterialColor);
            
            picker.Title = "Select Color for " + material.MaterialName;

            if (picker.ShowDialog() == true)
            {
                LbMaterialx.ItemsSource = null;

                material.MaterialColor = picker.SelectedBrush;   // SolidColorBrush with the chosen color
                                                                 // or just myCurrentBrush.Color = picker.SelectedBrush.Color;
                LbMaterialx.ItemsSource = ListMaterials;

                txtBoxConsole.Text += "changing color for" + material.MaterialName + "\n";
            }
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e) 
        {
            int i = ListMaterials.Count;
            MaterialBase material = new MaterialBase(Brushes.Chocolate, "Material" + i.ToString());
            ListMaterials.Add(material);
            txtBoxConsole.Text += "new material " + material.MaterialName + "\n";
        }

    }

    public class MaterialBase
    {
        public SolidColorBrush? MaterialColor { get; set; }
        public string? MaterialName { get; set; }

        public MaterialBase(SolidColorBrush? materialColor, string? materialName) 
        {
            MaterialColor = materialColor;
            MaterialName = materialName;
        }
    }
} //namespace
//SDG