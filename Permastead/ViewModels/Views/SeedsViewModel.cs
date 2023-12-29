using System;
using System.Collections.ObjectModel;

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
    private ObservableCollection<SeedPacket> _packets;
    
    public FlatTreeDataGridSource<SeedPacket> SeedsSource { get; set; }

    [RelayCommand]
    private void RefreshData()
    {
        var p = Services.PlantingsService.GetSeedPackets(AppSession.ServiceMode, true);

        _packets = new ObservableCollection<SeedPacket>(p);
        
        SeedsSource = new FlatTreeDataGridSource<SeedPacket>(_packets)
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
    }
    
    
    public SeedsViewModel()
    {
       this.RefreshData();
    }
}