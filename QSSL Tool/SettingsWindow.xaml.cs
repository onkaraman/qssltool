using QSSLTool.Gateways;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace QSSLTool
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ColorSettings _colorSettings;

        public SettingsWindow()
        {
            InitializeComponent();
            _colorSettings = new ColorSettings();

            restoreColors();
            setupViews();
        }

        private void setupViews()
        {
            NegativeBGPicker.SelectedColorChanged += NegativeBGPickerColorChanged;
            NegativeFGPicker.SelectedColorChanged += NegativeFGPickerColorChanged;
            PositiveBGPicker.SelectedColorChanged += PositiveBGPickerColorChanged;
            PositiveFGPicker.SelectedColorChanged += PositiveFGPickerColorChanged;
            NeutralBGPicker.SelectedColorChanged += NeutralBGPickerColorChanged;
            NeutralFGPicker.SelectedColorChanged += NeutralFGPickerColorChanged;
        }

        private void restoreColors()
        {
            NeutralBGPicker.SelectedColor = _colorSettings.NeutralBG;
            NeutralFGPicker.SelectedColor = _colorSettings.NeutralFG;
            PositiveBGPicker.SelectedColor = _colorSettings.PositiveBG;
            PositiveFGPicker.SelectedColor = _colorSettings.PositiveFG;
            NegativeBGPicker.SelectedColor = _colorSettings.NegativeBG;
            NegativeFGPicker.SelectedColor = _colorSettings.NegativeFG;
        }

        #region Color changed events
        private void NeutralFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.NeutralFG = (Color) NeutralFGPicker.SelectedColor;
        }

        private void NeutralBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.NeutralBG = (Color)NeutralBGPicker.SelectedColor;
        }

        private void PositiveFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.PositiveFG = (Color)PositiveFGPicker.SelectedColor;
        }

        private void PositiveBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.PositiveBG = (Color)PositiveBGPicker.SelectedColor;
        }

        private void NegativeFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.NegativeFG = (Color)NegativeFGPicker.SelectedColor;
        }

        private void NegativeBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _colorSettings.NegativeBG = (Color)NegativeBGPicker.SelectedColor;
        }
        #endregion
    }
}
