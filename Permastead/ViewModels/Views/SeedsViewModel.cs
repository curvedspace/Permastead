using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

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
        var myPackets = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, true);
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
        
        SeedsSource = new FlatTreeDataGridSource<SeedPacket>(Packets)
        {
            Columns =
            {
                new TextColumn<SeedPacket, DateTime>
                    ("Date", x => x.CreationDate),
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
                new TextColumn<SeedPacket, int>
                    ("DTH", x => x.DaysToHarvest),
                new TextColumn<SeedPacket, long>
                    ("Gens", x => x.Generations),
                new TextColumn<SeedPacket, string>
                    ("Vendor", x => x.Vendor.Description),
                new TextColumn<SeedPacket, string>
                    ("Instructions", x => x.Instructions)
            },
        };

        SeedsCount = Packets.Count;
        
        Console.WriteLine("Refreshed Seeds view: " + SeedsCount);
    }
    
    public SeedsViewModel()
    {
       this.RefreshDataOnly();
    }
}