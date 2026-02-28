// JJ                                   Copyright 2026 Richard Andrew Holland All Rights Reservered
/*
 *  The purpose of this window is to provide a dialog to select a color from a palette. This can be done and previewed using text, sliders
 *  or selecting one of 256 colors that are displayed.
 * 
 */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;

namespace WpfMenuTestApp
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Gets the brightness of a System.Windows.Media.Color.
        /// The brightness is a value from 0.0 to 1.0.
        /// </summary>
        public static float GetColorfulness(this System.Windows.Media.Color c)
        {
            // Convert the Media Color to a Drawing Color to access the GetBrightness method
            return (float) 256 * 256 * c.B + 256 * c.G + c.R;          
        }
    }

    public partial class ColorPickerWindow : Window
    {
        private bool _isUpdating;
        private Color _currentColor;

        /// <summary>
        /// The SolidColorBrush the user selected (updated live).
        /// </summary>
        public SolidColorBrush SelectedBrush { get; private set; }

        private BitmapPalette palette;
        private static List<ColorItem> colorItems = new List<ColorItem>();  //Static so we don't have to regenerate every initialization

        public ColorPickerWindow(SolidColorBrush? initialBrush)
        {
            _isUpdating = true;                    // ← ADD THIS LINE (before InitializeComponent)
            InitializeComponent();

            // Use supplied brush or default to white
            _currentColor = initialBrush?.Color ?? Colors.Black;
            SelectedBrush = new SolidColorBrush(_currentColor);

            // Use the built-in 256-color "default" palette
            palette = BitmapPalettes.Halftone256;   // ← or WebSafe256, Gray4, etc.

            InitializePaletteGrid();

            UpdateUI();
        }

        private void InitializePaletteGrid()
        {
            if (colorItems.Count > 0)
            {
                PaletteGrid.ItemsSource = colorItems;   //colorItems static already initialized previously
                return;
            }
            colorItems.Clear();

            for (int i = 0; i < palette.Colors.Count; i+=2)
            {
                var c = palette.Colors[i];
                c.A -= 20;
                string hex = $"#{c.R:X2}{c.G:X2}{c.B:X2}";

                colorItems.Add(new ColorItem
                {
                    Brush = c,
                    Hex = hex,
                    Index = i
                });
            }

            var sorted = colorItems.OrderByDescending(x => x.Brush.GetColorfulness()).ToList();

            colorItems = sorted;

            PaletteGrid.ItemsSource = colorItems;
        }

        private void PaletteGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var element = e.OriginalSource as FrameworkElement;
            if (element == null) return;

            var item = element.DataContext as ColorItem;
            if (item == null) return;

            textHex.Text = item.Hex;

            _currentColor = item.Brush;
            SelectedBrush = new SolidColorBrush(_currentColor);
        }

        public class ColorItem   // simple view model for each cell
        {
            public Color Brush { get; set; }
            public string Hex { get; set; }
            public int Index { get; set; }
        }

        private void UpdateUI()
        {
            _isUpdating = true;

            sliderR.Value = _currentColor.R;
            sliderG.Value = _currentColor.G;
            sliderB.Value = _currentColor.B;
            sliderA.Value = _currentColor.A;

            textR.Text = _currentColor.R.ToString();
            textG.Text = _currentColor.G.ToString();
            textB.Text = _currentColor.B.ToString();
            textA.Text = _currentColor.A.ToString();
            textHex.Text = _currentColor.ToString();

            colorPreview.Fill = SelectedBrush = new SolidColorBrush(_currentColor);

            _isUpdating = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdating) return;

            _currentColor = Color.FromArgb(
                (byte)sliderA.Value,
                (byte)sliderR.Value,
                (byte)sliderG.Value,
                (byte)sliderB.Value);

            UpdateUI();
        }

        private void ColorText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating || sender is not TextBox tb) return;

            if (byte.TryParse(tb.Text, out byte value))
            {
                Color newColor = _currentColor;

                if (tb == textR) newColor = Color.FromArgb(newColor.A, value, newColor.G, newColor.B);
                else if (tb == textG) newColor = Color.FromArgb(newColor.A, newColor.R, value, newColor.B);
                else if (tb == textB) newColor = Color.FromArgb(newColor.A, newColor.R, newColor.G, value);
                else if (tb == textA) newColor = Color.FromArgb(value, newColor.R, newColor.G, newColor.B);

                _currentColor = newColor;
                UpdateUI();
            }
        }

        private void HexText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(textHex.Text))
                {
                    Color newColor = (Color)ColorConverter.ConvertFromString(textHex.Text.Trim());
                    _currentColor = newColor;
                    UpdateUI();
                }
            }
            catch
            {
                // Invalid input – ignore until it's valid again
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    } //ColorPickerWindow
} //namespace ColorPickerExample
  //SDG