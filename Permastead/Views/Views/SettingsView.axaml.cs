using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Permastead.ViewModels.Views;

namespace Permastead.Views.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void Location_Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var vm = DataContext as SettingsViewModel;
        
        try
        {
            
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Permastead Database File",
                AllowMultiple = false
                
            });

            if (files.Count >= 1)
            {
                //set the database file to the selected file
                vm.DatabaseLocation = files[0].Path.AbsolutePath;
                
                //try to access the path
                var fi = new FileInfo(vm.DatabaseLocation);
                
                if (!fi.Exists)
                {
                    vm.DatabaseLocation = Services.SettingsService.GetLocalDatabaseSource();
                }
            }
        }
        catch (Exception ex)
        {
            vm.DatabaseLocation = Services.SettingsService.GetLocalDatabaseSource();
        }
        finally
        {
            
        }
    }
}