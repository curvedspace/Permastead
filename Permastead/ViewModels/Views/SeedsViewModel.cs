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
    
    public FlatTreeDataGridSource<SeedPacket> SeedsSource { get; set; }

    [RelayCommand]
    private void RefreshData()
    {
        RefreshDataOnly();
    }

    public void RefreshDataOnly()
    {
        var myPackets = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, true);
        
        Packets.Clear();
        foreach (var packet in myPackets)
        {
            Packets.Add(packet);
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
        
        Console.WriteLine("Refreshed Seeds view");
    }
    
    public SeedsViewModel()
    {
       this.RefreshDataOnly();
    }
}