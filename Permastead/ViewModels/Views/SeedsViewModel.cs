using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Permastead.ViewModels.Dialogs;
using Permastead.Views.Dialogs;
using Services;

namespace Permastead.ViewModels.Views;

public partial class SeedsViewModel : ViewModelBase
{
    [ObservableProperty]
    private SeedPacket _currentItem;
    
    [ObservableProperty]
    private ObservableCollection<SeedPacket> _packets = new ObservableCollection<SeedPacket>();

    [ObservableProperty] private string _searchText = "";
    
    [ObservableProperty] 
    private bool _currentOnly = true;
    
    [ObservableProperty] 
    private long _seedsCount;
    
    [ObservableProperty] 
    private bool _showObservations;
    
    [ObservableProperty] private SeedPacketObservation _currentObservation = new SeedPacketObservation();
    
    [ObservableProperty]
    private ObservableCollection<SeedPacketObservation> _seedPacketObservations = new ObservableCollection<SeedPacketObservation>();
    
    public FlatTreeDataGridSource<SeedPacket> SeedsSource { get; set; }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly(SearchText);
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = "";
        RefreshDataOnly(SearchText);
    }

    public void RefreshDataOnly(string filterText = "")
    {
        var myPackets = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, true).OrderByDescending(x => x.CreationDate);
        var caseAdjustedFilterText = filterText.Trim().ToLowerInvariant();
        
        Packets.Clear();
        foreach (var packet in myPackets)
        {
            if (string.IsNullOrEmpty(caseAdjustedFilterText))
            {
                if (CurrentOnly)
                {
                    if (packet.IsCurrent()) Packets.Add(packet);
                }
                else
                {
                    Packets.Add(packet);
                }
            }
            else
            {
                if (packet.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    packet.Instructions.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    packet.Vendor.Description.ToLowerInvariant().Contains(caseAdjustedFilterText) ||
                    packet.Plant.Description.ToLowerInvariant().Contains(caseAdjustedFilterText))
                {
                    if (CurrentOnly)
                    {
                        if (packet.IsCurrent()) Packets.Add(packet);
                    }
                    else
                    {
                        Packets.Add(packet);
                    }
                }
            }
        }

       
        if (Packets.Count > 0) 
            CurrentItem = Packets.FirstOrDefault()!;
        else
            CurrentItem = new SeedPacket();
        
        SeedPacketObservations =
            new ObservableCollection<SeedPacketObservation>(
                Services.SeedPacketService.GetObservationsForSeedPacket(AppSession.ServiceMode, CurrentItem.Id));
        
        SeedsSource = new FlatTreeDataGridSource<SeedPacket>(Packets)
        {
            Columns =
            {
                new TextColumn<SeedPacket, string>
                    ("Date", x => x.CreationDateString),
                new TextColumn<SeedPacket, string>
                    ("Code", x => x.Code),
                new TextColumn<SeedPacket, string>
                    ("Description", x => x.Description),
                new TextColumn<SeedPacket, string>
                    ("Author", x => x.Author.FirstName),
                new TextColumn<SeedPacket, string>
                    ("Plant", x => x.Plant.Description),
                new TextColumn<SeedPacket, string>
                    ("Seasonality", x => x.Seasonality.Description),
                new TextColumn<SeedPacket, string>
                    ("Species", x => x.Species),
                new TextColumn<SeedPacket, string>
                    ("Starter Type", x => x.StarterType!.Description),
                new TextColumn<SeedPacket, int>
                    ("DTH", x => x.DaysToHarvest),
                new TextColumn<SeedPacket, long>
                    ("Gens", x => x.Generations),
                new TextColumn<SeedPacket, long>
                    ("Count", x => x.PacketCount),
                new TextColumn<SeedPacket, bool>
                    ("Exchange", x => x.Exchange),
                new TextColumn<SeedPacket, string>
                    ("Vendor", x => x.Vendor.Description),
                new TextColumn<SeedPacket, string>
                    ("Instructions", x => x.Instructions)
            },
        };

        SeedsCount = Packets.Count;
        
        //Console.WriteLine("Refreshed Seeds view: " + SeedsCount);
    }
    
    public void GetObservations()
    {
        if (CurrentItem != null)
        {
            SeedPacketObservations =
                new ObservableCollection<SeedPacketObservation>(
                    Services.SeedPacketService.GetObservationsForSeedPacket(AppSession.ServiceMode, CurrentItem.Id));
        }
    }
    
    [RelayCommand]
    private void SaveObservation()
    {
        try
        {
            //saves the planting observation to database
            CurrentObservation.Author!.Id = AppSession.Instance.CurrentUser.Id;
            CurrentObservation.SeedPacket = CurrentItem;
            CurrentObservation.AsOfDate = DateTime.Today;
            CurrentObservation.CommentType!.Id = 2;
            
            SeedPacketService.AddObservation(AppSession.ServiceMode, CurrentObservation);
            
            SeedPacketObservations =
                new ObservableCollection<SeedPacketObservation>(
                    Services.SeedPacketService.GetObservationsForSeedPacket(AppSession.ServiceMode, CurrentItem.Id));

            CurrentObservation = new SeedPacketObservation();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    [RelayCommand]
    public void EditStarter()
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new StarterWindow();
        
        //get the selected row in the list
        var current = CurrentItem;
        if (current != null)
        {
            var vm = new StarterWindowViewModel(current);
            vm.ControlViewModel = this;

            plantingWindow.DataContext = vm;

            plantingWindow.Topmost = true;
            plantingWindow.Width = 1000;
            plantingWindow.Height = 650;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Starter - " + current.Description;
            plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            plantingWindow.Show();
        }

    }
    
    [RelayCommand]
    public void CreatePlanting()
    {
        // open the selected planting in a window for viewing/editing
        var plantingWindow = new PlantingWindow();
        
        //get the selected row in the list
        var current = CurrentItem;
        if (current != null)
        {
            var planting = new Planting();
            planting.SeedPacket = CurrentItem;
            planting.Description = CurrentItem.Description;
            planting.Plant = CurrentItem.Plant;
            planting.Author.Id = AppSession.Instance.CurrentUser.Id;
            
            //get underlying view's viewmodel
            var vm = new PlantingWindowViewModel(planting,  new PlantingsViewModel());
            
            plantingWindow.DataContext = vm;
        
            plantingWindow.Topmost = true;
            plantingWindow.Width = 900;
            plantingWindow.Height = 500;
            plantingWindow.Opacity = 0.95;
            plantingWindow.Title = "Planting - " + planting.Description;
            
            plantingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            plantingWindow.Show();
        }

    }
    
    public SeedsViewModel()
    {
       this.RefreshDataOnly();
    }
}