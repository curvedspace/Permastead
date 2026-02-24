using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Permastead.ViewModels.Views;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Ursa.Controls;
using Image = SixLabors.ImageSharp.Image;

namespace Permastead.Views.Views;

public partial class SettingsView : UserControl
{
    private SettingsViewModel? _viewModel;
    
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not SettingsViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
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

    private async void MenuItemAddImage_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Property Image File",
                SuggestedFileName = "*.jpg",
                AllowMultiple = false
            });

            if (files.Count >= 1)
            {
                var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var vm = (SettingsViewModel)DataContext;

                var image = Image.Load(files[0].Path.AbsolutePath);
                if (isWindows)
                {
                    image.Mutate(x => x.Resize(1024, 1024)); 
                }
                else
                {
                    image.Mutate(x => x.Resize(1024, 1024)); 
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
                        newLocation = userFolder + @"\.config\permastead\images\properties\" + vm.HomesteadName + ".png";
                        //fi.CopyTo(newLocation);
                        image.SaveAsPng(newLocation);
                    }
                    else
                    {
                        newLocation = userFolder + @"/.config/permastead/images/properties/" + vm.HomesteadName + ".png";
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
                            vm.PropertyPicture = pic;
                            vm.ToastManager?.Show(new Toast("Image has been added for " + vm.HomesteadName +
                                                            "."));
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
            var vm = (SettingsViewModel)DataContext;
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                newLocation = userFolder + @"\.config\permastead\images\properties\" + vm.HomesteadName + ".png";
            }
            else
            {
                newLocation = userFolder + @"/.config/permastead/images/properties/" + vm.HomesteadName + ".png";
            }
            
            FileInfo fi = new FileInfo(newLocation);
            if (fi.Exists)
            {
                fi.Delete();
                vm.ToastManager?.Show(new Toast("Image has been removed for " + vm.HomesteadName + "."));
            }
        
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}