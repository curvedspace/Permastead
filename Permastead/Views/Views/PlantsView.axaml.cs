using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using DataAccess.Server;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.ViewModels.Views;
using Permastead.Views.Dialogs;

namespace Permastead.Views.Views;

public partial class PlantsView : UserControl
{
    public PlantsView()
    {
        InitializeComponent();
    }

    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        // add a new plant via a dialog
        try
        {
            PlantsViewModel plantVm  = (PlantsViewModel)DataContext;
            
            
            var plantWindow = new PlantWindow();
            var vm = new PlantWindowViewModel();
            
            plantWindow.DataContext = vm;
        
            plantWindow.Topmost = true;
            plantWindow.Width = 900;
            plantWindow.Height = 700;
            plantWindow.Opacity = 0.95;
            plantWindow.Title = "New Plant";
            plantWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
            plantWindow.Show();

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PlantsList_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            var currentItem = sender as ListBox;
            var current = currentItem.SelectedValue as Plant;
           
            var vm = (PlantsViewModel)DataContext;
            vm.CurrentPlant = current;
            vm.GetMetaData();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            PlantsViewModel vm  = (PlantsViewModel)DataContext;
            vm.SavePlant();

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void SearchBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if the key is return, do a search and filter the grid data
        if (e.Key == Key.Return)
        {
            var textValue = SearchBox.Text;
            var vm = (PlantsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.RefreshDataOnly(textValue);
            }
        }

        if (e.Key == Key.Escape)
        {
            var vm = (PlantsViewModel)this.DataContext;

            if (vm != null)
            {
                vm.SearchText = "";
                vm.RefreshDataOnly(vm.SearchText);
            }
        }
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        
    }

    private async void MenuItemAddImage_OnClick(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Image File",
            SuggestedFileName = "*.png",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            var vm = (PlantsViewModel)DataContext;
            
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            using (var streamReader = new StreamReader(stream))
            {
                Bitmap pic = new Bitmap(stream);
                
                if (pic != null)
                {
                    vm.Picture = pic;
                }
            }
      
            // save the image and update the plant object with the new image id
            // vm.CurrentPlant.Image = Services.ImageHelperService.SaveImage(AppSession.ServiceMode, files[0].Path.AbsolutePath);
            //
            // if (vm.CurrentPlant.Image != null)
            // {
            //     Services.PlantService.CommitRecord(AppSession.ServiceMode, vm.CurrentPlant);
            // }
            
            //save the file into the config area
            FileInfo fi = new FileInfo(files[0].Path.AbsolutePath);

            if (fi.Exists)
            {
                var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                
                var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                if (isWindows)
                {
                    var newLocation = userFolder + @"\.config\permastead\images\plants\" + vm.CurrentPlant.Id + ".png";
                    fi.CopyTo(newLocation);
                }
                else
                {
                    var newLocation = userFolder + @"/.config/permastead/images/plants/" + vm.CurrentPlant.Id + ".png";
                    fi.CopyTo(newLocation);
                }
                
            }
            
        }
    }
}