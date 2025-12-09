using System;
using System.Collections.Generic;
using Avalonia.Controls.Converters;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.Extensions;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class  CyclesViewModel : ViewModelBase
{
    public double Value { get; set; } = 80;
    public double Value2 { get; set; } = 10;
    public double Value3 { get; set; } = 60;
    
    public double BirthdayValue1 { get; set; } = 0;
    public double BirthdayValue2 { get; set; } = 0;
    public double BirthdayValue3 { get; set; } = 0;
    
    public string BirthdayName1 { get; set; } = "";
    public string BirthdayName2 { get; set; } = "";
    public string BirthdayName3 { get; set; } = "";

    public bool BirthdayVisible1 { get; set; } = false;
    
    public bool BirthdayVisible2 { get; set; } = false;
    public bool BirthdayVisible3 { get; set; } = false;
    
    private List<AnEvent> myEvents { get; set; } = new List<AnEvent>();
    
    public double HolidayValue1 { get; set; } = 0;
    public double HolidayValue2 { get; set; } = 0;
    public double HolidayValue3 { get; set; } = 0;
    
    public string HolidayName1 { get; set; } = "";
    public string HolidayName2 { get; set; } = "";
    public string HolidayName3 { get; set; } = "";
    
    private List<AnEvent> myHolidays { get; set; } = new List<AnEvent>();
    
    public double PlaceValue1 { get; set; } = 0;
    public double PlaceValue2 { get; set; } = 0;
    public double PlaceValue3 { get; set; } = 0;
    
    public string PlaceName1 { get; set; } = "";
    public string PlaceName2 { get; set; } = "";
    public string PlaceName3 { get; set; } = "";
    
    private List<AnEvent> myPlaceEvents { get; set; } = new List<AnEvent>();
    
    public double TodoValue1 { get; set; } = 0;
    public double TodoValue2 { get; set; } = 0;
    public double TodoValue3 { get; set; } = 0;
    
    public string TodoName1 { get; set; } = "";
    public string TodoName2 { get; set; } = "";
    public string TodoName3 { get; set; } = "";
    
    public string CurrentCycle { get; set; } = "";
    
    public IEnumerable<ISeries> Series { get; set; }
    
    public IEnumerable<ISeries> Birthday1 { get; set; }
    public IEnumerable<ISeries> Birthday2 { get; set; }
    public IEnumerable<ISeries> Birthday3 { get; set; }
    
    public Func<ChartPoint, string> LabelFormatter { get; set; } =
        point => $"{365 - point.Coordinate.PrimaryValue} days : {point.Context.Series.Name}";
    
    public Func<ChartPoint, string> MonthLabelFormatter { get; set; } =
        point => $"{30 - point.Coordinate.PrimaryValue} days : {point.Context.Series.Name}";
    
    [RelayCommand]
    private void RefreshData()
    {
        var nextBirthdays = EventsService.GetNextBirthdays(AppSession.ServiceMode);
        if (nextBirthdays != null)
        {
            if (nextBirthdays.Count > 0)
            {
                BirthdayValue1 = 365 - nextBirthdays[0].DaysUntilNext;
                BirthdayName1 = nextBirthdays[0].Description!;
                BirthdayVisible1 = true;
            }
            if (nextBirthdays.Count > 1)
            {
                BirthdayValue2 = 365 - nextBirthdays[1].DaysUntilNext;
                BirthdayName2 = nextBirthdays[1].Description!;
                BirthdayVisible2 = true;
            }
            if (nextBirthdays.Count > 2)
            {
                BirthdayValue3 = 365 - nextBirthdays[2].DaysUntilNext;
                BirthdayName3 = nextBirthdays[2].Description!;
                BirthdayVisible3 = true;
            }
            
        }
        
        var nextHolidays = EventsService.GetNextHolidays(AppSession.ServiceMode);
        if (nextHolidays != null)
        {
            if (nextHolidays.Count > 0)
            {
                HolidayValue1 = 365 - nextHolidays[0].DaysUntilNext;
                HolidayName1 = nextHolidays[0].Description!;
            }
            if (nextHolidays.Count > 1)
            {
                HolidayValue2 = 365 - nextHolidays[1].DaysUntilNext;
                HolidayName2 = nextHolidays[1].Description!;
            }
            if (nextHolidays.Count > 2)
            {
                HolidayValue3 = 365 - nextHolidays[2].DaysUntilNext;
                HolidayName3 = nextHolidays[2].Description!;
            }
            
        }
        
        myPlaceEvents = EventsService.GetNextPlaceEvents(AppSession.ServiceMode);
        if (myPlaceEvents != null)
        {
            if (myPlaceEvents.Count > 0)
            {
                PlaceValue1 = 365 - myPlaceEvents[0].DaysUntilNext;
                PlaceName1 = myPlaceEvents[0].Description!;
            }
            if (myPlaceEvents.Count > 1)
            {
                PlaceValue2 = 365 - myPlaceEvents[1].DaysUntilNext;
                PlaceName2 = myPlaceEvents[1].Description!;
            }
            if (myPlaceEvents.Count > 2)
            {
                PlaceValue3 = 365 - myPlaceEvents[2].DaysUntilNext;
                PlaceName3 = myPlaceEvents[2].Description!;
            }
            
        }
        
        var myTodos = ToDoService.GetActiveTodos(AppSession.ServiceMode);
        if (myTodos != null)
        {
            if (myTodos.Count > 0)
            {
                TodoValue1 = 30 - myTodos[0].DaysUntilDue;
                TodoName1 = myTodos[0].Description!;
            }
            if (myTodos.Count > 1)
            {
                TodoValue2 = 30 - myTodos[1].DaysUntilDue;
                TodoName2 = myTodos[1].Description!;
            }
            if (myTodos.Count > 2)
            {
                TodoValue3 = 30 - myTodos[2].DaysUntilDue;
                TodoName3 = myTodos[2].Description!;
            }
            
        }
        
        Series = null;
        Series = GaugeGenerator.BuildSolidGauge();
        
        Birthday1 = GaugeGenerator.BuildSolidGauge();
        Birthday2 = GaugeGenerator.BuildSolidGauge();
        Birthday3 = GaugeGenerator.BuildSolidGauge();

        var currentEvents = Services.EventsService.GetCurrentHoliday(AppSession.ServiceMode);

        if (currentEvents is { Count: > 0 })
        {
            CurrentCycle = "Current: " + currentEvents[0].Description.ToString();
        }
        else
        {
            CurrentCycle = string.Empty;
        }

    }

    public CyclesViewModel()
    {
        RefreshData();
    }
}