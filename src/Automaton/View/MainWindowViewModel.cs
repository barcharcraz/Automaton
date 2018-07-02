﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Automaton.Controllers;
using Automaton.Model.Instances;
using Automaton.Model.Modpack;
using GalaSoft.MvvmLight.Command;

namespace Automaton.View
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<Window> CloseWindowCommand { get; set; }
        public RelayCommand<Window> MinimizeWindowCommand { get; set; }
        public RelayCommand<Window> MoveWindowCommand { get; set; }

        public int CurrentTransitionerIndex { get; set; } = 0;

        public MainWindowViewModel()
        {
            // Initialize relaycommand bindings
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
            MoveWindowCommand = new RelayCommand<Window>(MoveWindow);

            // Initialize event handlers
            ModpackUtilities.ModpackLoadedEvent += ModpackLoaded;
            ViewIndexController.ViewIndexChangedEvent += ViewIndexUpdate;
        }

        public void ModpackLoaded()
        {
            // Modpack has been loaded, so increment the current view index
            ViewIndexController.IncrementCurrentViewIndex();

            ViewIndexController.CurrentViewIndex = 3;

            ApplyAutomatonModpack();
        }

        private void ViewIndexUpdate(int index)
        {
            CurrentTransitionerIndex = index;
        }

        private void ApplyAutomatonModpack()
        {
            var modpackHeader = ModpackInstance.ModpackHeader;

            if (!string.IsNullOrEmpty(modpackHeader.BackgroundColor))
            {
                Application.Current.Resources["BackgroundColor"] =
                    (SolidColorBrush)new BrushConverter().ConvertFromString(modpackHeader.BackgroundColor);
            }

            if (!string.IsNullOrEmpty(modpackHeader.FontColor))
            {
                Application.Current.Resources["FontColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString(modpackHeader.FontColor);
            }

            if (!string.IsNullOrEmpty(modpackHeader.ButtonColor))
            {
                Application.Current.Resources["ButtonColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString(modpackHeader.ButtonColor);
            }

            if (!string.IsNullOrEmpty(modpackHeader.AssistantControlColor))
            {
                Application.Current.Resources["AssistantControlColor"] = (SolidColorBrush)new BrushConverter().ConvertFromString(modpackHeader.AssistantControlColor);
            }

            if (!string.IsNullOrEmpty(modpackHeader.HeaderImage))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                bitmapImage.UriSource = new Uri(modpackHeader.HeaderImage);

                bitmapImage.EndInit();

                Application.Current.Resources["HeaderImage"] = bitmapImage;
            }

            Application.Current.Resources["ModpackName"] = modpackHeader.ModpackName;
            Application.Current.Resources["ModpackDescription"] = modpackHeader.Description;
        }

        #region Window Manipulation Code
        private static void CloseWindow(Window window)
        {
            window.Close();
        }

        private static void MinimizeWindow(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        private static void MoveWindow(Window window)
        {
            try
            {
                window.DragMove();
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        #endregion
    }
}
