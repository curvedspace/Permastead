using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Schema;
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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Image = SixLabors.ImageSharp.Image;

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
        try
        {
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Image File",
                SuggestedFileName = "*.jpg",
                AllowMultiple = false
            });

            if (files.Count >= 1)
            {
                var vm = (PlantsViewModel)DataContext;

                
                
                var image = Image.Load(files[0].Path.AbsolutePath);
                image.Mutate(x => x.Resize(400, 300));

                float angle = 90;
                var rp = new RotateProcessor(angle, image.Size);

                image.Mutate(x => x.ApplyProcessor(rp));
                
                // image.Save("result.jpg");

                //save the file into the config area
                FileInfo fi = new FileInfo(files[0].Path.AbsolutePath);

                var newLocation = "";
                    
                if (fi.Exists)
                {
                    var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                    if (isWindows)
                    {
                        newLocation = userFolder + @"\.config\permastead\images\plants\" + vm.CurrentPlant.Id + ".png";
                        //fi.CopyTo(newLocation);
                        image.SaveAsPng(newLocation);
                    }
                    else
                    {
                        newLocation = userFolder + @"/.config/permastead/images/plants/" + vm.CurrentPlant.Id + ".png";
                        //fi.CopyTo(newLocation);
                        image.SaveAsPng(newLocation);
                    }

                }
                
                // Open reading stream from the first file.
                FileInfo nfi = new FileInfo(newLocation);

                if (nfi.Exists)
                {
                    
                    using var stream = nfi.OpenRead();
                    {
                        Bitmap pic = new Bitmap(stream);
                            
                        if (pic != null)
                        {
                            vm.Picture = pic;
                        }
                    }
                
                }
                
            }
            
            

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
        
    }

    private void MenuItemRemoveImage_OnClick(object? sender, RoutedEventArgs e)
    {
        var newLocation = string.Empty;
        
        try
        {
            //check to see if the file exists before removing it
            var vm = (PlantsViewModel)DataContext;
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                newLocation = userFolder + @"\.config\permastead\images\plants\" + vm.CurrentPlant.Id + ".png";
            }
            else
            {
                newLocation = userFolder + @"/.config/permastead/images/plants/" + vm.CurrentPlant.Id + ".png";
            }
            
            FileInfo fi = new FileInfo(newLocation);
            if (fi.Exists)
            {
                fi.Delete();
                vm.GetMetaData();
            }
        
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}