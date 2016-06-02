﻿using QSSLTool.Compacts;
using QSSLTool.FileParsers;
using QSSLTool.Gateways;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace QSSLTool
{
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
            PublishResultsCheckbox.Unchecked += PublishResultsChecked;
            UseCacheCheckBox.Checked += UseCacheChecked;
            UseCacheCheckBox.Unchecked += UseCacheChecked;
            IgnoreMismatchCheckBox.Checked += IgnoreMismatchChecked;
            IgnoreMismatchCheckBox.Unchecked += IgnoreMismatchChecked;
            WarningDaysTB.TextChanged += WarningDaysTBTextChanged;

            NegativeBGPicker.SelectedColorChanged += NegativeBGPickerColorChanged;
            NegativeFGPicker.SelectedColorChanged += NegativeFGPickerColorChanged;
            PositiveBGPicker.SelectedColorChanged += PositiveBGPickerColorChanged;
            PositiveFGPicker.SelectedColorChanged += PositiveFGPickerColorChanged;
            NeutralBGPicker.SelectedColorChanged += NeutralBGPickerColorChanged;
            NeutralFGPicker.SelectedColorChanged += NeutralFGPickerColorChanged;
        }

        private void restoreCheckBoxes()
        {
            WarningDaysTB.Text = Settings.Static.AnalyzerSettings.WarningDays.ToString();

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
            NeutralBGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.NeutralBG);
            NeutralFGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.NeutralFG);
            PositiveBGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.PositiveBG);
            PositiveFGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.PositiveFG);
            NegativeBGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.NegativeBG);
            NegativeFGPicker.SelectedColor =
                DataFormatter.Static.ColorHolderToColor(Settings.Static.ColorSettings.NegativeFG);
        }

        private void WarningDaysTBTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                Settings.Static.AnalyzerSettings.WarningDays = 
                    int.Parse(WarningDaysTB.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
            Color sel = (Color)NeutralFGPicker.SelectedColor;
            Settings.Static.ColorSettings.NeutralFG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            NeutralSampleTB.Foreground = new SolidColorBrush(sel);
        }

        private void NeutralBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color sel = (Color)NeutralBGPicker.SelectedColor;
            Settings.Static.ColorSettings.NeutralBG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            NeutralSampleTB.Background = new SolidColorBrush(sel);
        }

        private void PositiveFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color sel = (Color)PositiveFGPicker.SelectedColor;
            Settings.Static.ColorSettings.PositiveFG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            PositiveSampleTB.Foreground = new SolidColorBrush(sel);
        }

        private void PositiveBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color sel = (Color)PositiveBGPicker.SelectedColor;
            Settings.Static.ColorSettings.PositiveBG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            PositiveSampleTB.Background = new SolidColorBrush(sel);
        }

        private void NegativeFGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color sel = (Color)NegativeFGPicker.SelectedColor;
            Settings.Static.ColorSettings.NegativeFG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            NegativeSampleTB.Foreground = new SolidColorBrush(sel);
        }

        private void NegativeBGPickerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color sel = (Color)NegativeBGPicker.SelectedColor;
            Settings.Static.ColorSettings.NegativeBG = new ColorHolder(sel.A, sel.R, sel.G, sel.B);
            NegativeSampleTB.Background = new SolidColorBrush(sel);
        }
        #endregion
    }
}
