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

        public SettingsWindow()
        {
            InitializeComponent();

            restoreCheckBoxes();
            setupViews();
            restoreColors();
        }

        private void setupViews()
        {
            PublishResultsCheckbox.Checked += PublishResultsChecked;
            UseCacheCheckBox.Checked += UseCacheChecked;
            IgnoreMismatchCheckBox.Checked += IgnoreMismatchChecked;

            NegativeBGPicker.SelectedColorChanged += NegativeBGPickerColorChanged;
            NegativeFGPicker.SelectedColorChanged += NegativeFGPickerColorChanged;
            PositiveBGPicker.SelectedColorChanged += PositiveBGPickerColorChanged;
            PositiveFGPicker.SelectedColorChanged += PositiveFGPickerColorChanged;
            NeutralBGPicker.SelectedColorChanged += NeutralBGPickerColorChanged;
            NeutralFGPicker.SelectedColorChanged += NeutralFGPickerColorChanged;
        }

        private void restoreCheckBoxes()
        {
            if (Settings.Static.AnalyzerSettings.FromCache 
                == SSLLabsApiWrapper.SSLLabsApiService.FromCache.On)
            {
                UseCacheCheckBox.IsChecked = true;
            }
            else UseCacheCheckBox.IsChecked = false;

            if (Settings.Static.AnalyzerSettings.Publish
                == SSLLabsApiWrapper.SSLLabsApiService.Publish.On)
            {
                PublishResultsCheckbox.IsChecked = true;
            }
            else PublishResultsCheckbox.IsChecked = false;

            if (Settings.Static.AnalyzerSettings.IgnoreMismatch 
                == SSLLabsApiWrapper.SSLLabsApiService.IgnoreMismatch.On)
            {
                IgnoreMismatchCheckBox.IsChecked = true;
            }
            else IgnoreMismatchCheckBox.IsChecked = false;

        }

        private void restoreColors()
        {
            NeutralBGPicker.SelectedColor = Settings.Static.ColorSettings.NeutralBG;
            NeutralFGPicker.SelectedColor = Settings.Static.ColorSettings.NeutralFG;
            PositiveBGPicker.SelectedColor = Settings.Static.ColorSettings.PositiveBG;
            PositiveFGPicker.SelectedColor = Settings.Static.ColorSettings.PositiveFG;
            NegativeBGPicker.SelectedColor = Settings.Static.ColorSettings.NegativeBG;
            NegativeFGPicker.SelectedColor = Settings.Static.ColorSettings.NegativeFG;
        }

        #region Checkbox events
        private void IgnoreMismatchChecked(object sender, RoutedEventArgs e)
        {
            Settings.Static.AnalyzerSettings.SetIgnoreMismatch(IgnoreMismatchCheckBox.IsChecked);
        }

        private void UseCacheChecked(object sender, RoutedEventArgs e)
        {
            Settings.Static.AnalyzerSettings.SetFromCache(UseCacheCheckBox.IsChecked);
        }

        private void PublishResultsChecked(object sender, RoutedEventArgs e)
        {
            Settings.Static.AnalyzerSettings.SetPublish(PublishResultsCheckbox.IsChecked);
        }
        #endregion

        #region Color changed events
        private void NeutralFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.NeutralFG = (Color) NeutralFGPicker.SelectedColor;
            NeutralSampleTB.Foreground = new SolidColorBrush(Settings.Static.ColorSettings.NeutralFG);
        }

        private void NeutralBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.NeutralBG = (Color)NeutralBGPicker.SelectedColor;
            NeutralSampleTB.Background = new SolidColorBrush(Settings.Static.ColorSettings.NeutralBG);
        }

        private void PositiveFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.PositiveFG = (Color)PositiveFGPicker.SelectedColor;
            PositiveSampleTB.Foreground = new SolidColorBrush(Settings.Static.ColorSettings.PositiveFG);
        }

        private void PositiveBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.PositiveBG = (Color)PositiveBGPicker.SelectedColor;
            PositiveSampleTB.Background = new SolidColorBrush(Settings.Static.ColorSettings.PositiveBG);
        }

        private void NegativeFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.NegativeFG = (Color)NegativeFGPicker.SelectedColor;
            NegativeSampleTB.Foreground = new SolidColorBrush(Settings.Static.ColorSettings.NegativeFG);
        }

        private void NegativeBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Static.ColorSettings.NegativeBG = (Color)NegativeBGPicker.SelectedColor;
            NegativeSampleTB.Background = new SolidColorBrush(Settings.Static.ColorSettings.NegativeBG);
        }
        #endregion
    }
}
