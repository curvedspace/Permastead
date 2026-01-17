using System;
using System.Collections.Generic;
using Avalonia.Controls.Converters;
using Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.Extensions;
using Models;
using Services;

namespace Permastead.ViewModels.Views;

public partial class  CyclesViewModel : ViewModelBase
{
    [ObservableProperty] private double _birthdayValue1;
    [ObservableProperty] private double _birthdayValue2;
    [ObservableProperty] private double _birthdayValue3;
    
    [ObservableProperty] private string _birthdayName1;
    [ObservableProperty] private string _birthdayName2;
    [ObservableProperty] private string _birthdayName3;
    
    [ObservableProperty] private bool _birthdayVisible1;
    [ObservableProperty] private bool _birthdayVisible2;
    [ObservableProperty] private bool _birthdayVisible3;
    
    private List<AnEvent> myEvents { get; set; } = new List<AnEvent>();
    
    [ObservableProperty] private double _holidayValue1;
    [ObservableProperty] private double _holidayValue2;
    [ObservableProperty] private double _holidayValue3;
    
    [ObservableProperty] private string _holidayName1;
    [ObservableProperty] private string _holidayName2;
    [ObservableProperty] private string _holidayName3;
    
    [ObservableProperty] private bool _holidayVisible1;
    [ObservableProperty] private bool _holidayVisible2;
    [ObservableProperty] private bool _holidayVisible3;
    
    private List<AnEvent> myHolidays { get; set; } = new List<AnEvent>();
    
    [ObservableProperty] private double _placeValue1;
    [ObservableProperty] private double _placeValue2;
    [ObservableProperty] private double _placeValue3;
    
    [ObservableProperty] private string _placeName1;
    [ObservableProperty] private string _placeName2;
    [ObservableProperty] private string _placeName3;
    
    [ObservableProperty] private bool _placeVisible1;
    [ObservableProperty] private bool _placeVisible2;
    [ObservableProperty] private bool _placeVisible3;
    
    private List<AnEvent> myPlaceEvents { get; set; } = new List<AnEvent>();

    [ObservableProperty] private double _todoValue1;
    [ObservableProperty] private double _todoValue2;
    [ObservableProperty] private double _todoValue3;
    
    [ObservableProperty] private string _todoName1;
    [ObservableProperty] private string _todoName2;
    [ObservableProperty] private string _todoName3;
    
    [ObservableProperty] private bool _toDoVisible1;
    [ObservableProperty] private bool _toDoVisible2;
    [ObservableProperty] private bool _toDoVisible3;
    
    
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
                HolidayVisible1 = true;
            }
            if (nextHolidays.Count > 1)
            {
                HolidayValue2 = 365 - nextHolidays[1].DaysUntilNext;
                HolidayName2 = nextHolidays[1].Description!;
                HolidayVisible2 = true;
            }
            if (nextHolidays.Count > 2)
            {
                HolidayValue3 = 365 - nextHolidays[2].DaysUntilNext;
                HolidayName3 = nextHolidays[2].Description!;
                HolidayVisible3 = true;
            }
            
        }
        
        myPlaceEvents = EventsService.GetNextPlaceEvents(AppSession.ServiceMode);
        if (myPlaceEvents != null)
        {
            if (myPlaceEvents.Count > 0)
            {
                PlaceValue1 = 365 - myPlaceEvents[0].DaysUntilNext;
                PlaceName1 = myPlaceEvents[0].Description!;
                PlaceVisible1 = true;
            }
            if (myPlaceEvents.Count > 1)
            {
                PlaceValue2 = 365 - myPlaceEvents[1].DaysUntilNext;
                PlaceName2 = myPlaceEvents[1].Description!;
                PlaceVisible2 = true;
            }
            if (myPlaceEvents.Count > 2)
            {
                PlaceValue3 = 365 - myPlaceEvents[2].DaysUntilNext;
                PlaceName3 = myPlaceEvents[2].Description!;
                PlaceVisible3 = true;
            }
            
        }
        
        var myTodos = ToDoService.GetActiveTodos(AppSession.ServiceMode);
        if (myTodos != null)
        {
            if (myTodos.Count > 0)
            {
                TodoValue1 = 30 - myTodos[0].DaysUntilDue;
                TodoName1 = TextUtils.GetSubstring(myTodos[0].Description!, 0, 25, true);
                ToDoVisible1 = true;
            }
            if (myTodos.Count > 1)
            {
                TodoValue2 = 30 - myTodos[1].DaysUntilDue;
                TodoName2 = TextUtils.GetSubstring(myTodos[1].Description!, 0, 25, true);
                ToDoVisible2 = true;
            }
            if (myTodos.Count > 2)
            {
                TodoValue3 = 30 - myTodos[2].DaysUntilDue;
                TodoName3 = TextUtils.GetSubstring(myTodos[2].Description!, 0, 25, true);
                ToDoVisible3 = true;
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