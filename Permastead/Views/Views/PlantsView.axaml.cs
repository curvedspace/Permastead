using System;
using System.Collections.ObjectModel;
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
using Ursa.Controls;
using Image = SixLabors.ImageSharp.Image;

namespace Permastead.Views.Views;

public partial class PlantsView : UserControl
{
    private PlantsViewModel? _viewModel;
    
    public PlantsView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not PlantsViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
    }
    
    private void Add_OnTapped(object? sender, TappedEventArgs e)
    {
        // add a new plant via a dialog
        try
        {
            PlantsViewModel plantVm  = (PlantsViewModel)DataContext;
            
            var plantWindow = new PlantWindow();
            var vm = new PlantWindowViewModel();
            
            vm.ToastManager = _viewModel?.ToastManager;
            
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
            //check for a new tag to be added in the search text
            var newTag = Tags.SearchText;
            
            if (newTag != "")
            {
                // if there is a new tag to be added, add it to selected items before saving
                var newTagModel = new TagData() { TagText =  newTag };
                Tags.SelectedItems.Add(newTagModel);
                
            }
            _viewModel.SavePlant();
            
            Tags.Text = "";
            
            _viewModel?.ToastManager.Show(new Toast("Plant record (" + _viewModel.CurrentPlant.Description + ") has been updated."));
            
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
                var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var vm = (PlantsViewModel)DataContext;
                
                var image = Image.Load(files[0].Path.AbsolutePath);
                if (isWindows)
                {
                    image.Mutate(x => x.Resize(400, 300));  //for windows, use 4x3 instead
                }
                else
                {
                    image.Mutate(x => x.Resize(300, 400));  //for windows, use 4x3 instead
                }

                float angle = 0;
                if (isWindows) angle = 0; //rotate on Windows but not Linux
                
                var rp = new RotateProcessor(angle, image.Size);

                image.Mutate(x => x.ApplyProcessor(rp));

                //save the file into the config area
                FileInfo fi = new FileInfo(files[0].Path.AbsolutePath);

                var newLocation = "";
                    
                if (fi.Exists)
                {
                    var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    
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
                            vm.ToastManager?.Show(new Toast("Image has been added for " + vm.CurrentPlant.Description + "."));
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
                vm.ToastManager?.Show(new Toast("Image has been removed for " + vm.CurrentPlant.Description + "."));
            }
        
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
    
}