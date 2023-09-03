using System.Data.Common;
using DataAccess.Local;
using Models;
using Serilog;

namespace Services;

public static class ScoreBoardService
{
    public static ScoreBoard ComputeTotalScore(ServiceMode mode)
    {
        var scoreBoard = new ScoreBoard();

        //set up achievements
        var achievements = new Dictionary<AchievementType,Achievement>
        {
            {
                AchievementType.AddObservation,
                new Achievement() { Type = AchievementType.AddObservation, InitialPoints = 50 }
            },
            { AchievementType.AddAction, new Achievement() { Type = AchievementType.AddAction, InitialPoints = 50 } },
            { AchievementType.AddInventory, new Achievement() { Type = AchievementType.AddInventory, InitialPoints = 50 } },
            { AchievementType.AddPlanting, new Achievement() { Type = AchievementType.AddPlanting, InitialPoints = 100 } },
            { AchievementType.AddEvent, new Achievement() { Type = AchievementType.AddEvent, InitialPoints = 100 } },
            { AchievementType.AddFermentation, new Achievement() { Type = AchievementType.AddObservation, InitialPoints = 100 } },
            { AchievementType.SaveSeed, new Achievement() { Type = AchievementType.SaveSeed, InitialPoints = 50 } }
        };

        var obs = ObservationsService.GetObservationsForAllEntities(mode);
        var todos = ToDoService.GetAllToDos(mode);
        var inventory = InventoryService.GetAllInventory(mode);
        var plantings = PlantingsService.GetPlantings(mode);
        var events = EventsService.GetAllEvents(mode);

        scoreBoard.Actions = todos.Count;
        scoreBoard.Observations = obs.Count;
        scoreBoard.Plantings = plantings.Count;
        scoreBoard.Events = events.Count;

        //iterate through data
       
        //observations
        var obsAchievement = achievements[AchievementType.AddObservation];
        foreach (var ob in obs)
        {
            obsAchievement.Count = obsAchievement.Count + 1;
            scoreBoard.TotalScore += obsAchievement.CurrentPoints;
        }

        //inventory
        var invAchievement = achievements[AchievementType.AddInventory];
        foreach (var inv in inventory)
        {
            invAchievement.Count = invAchievement.Count + 1;
            scoreBoard.TotalScore += invAchievement.CurrentPoints;
        }

        //plantings
        var plantingsAchievement = achievements[AchievementType.AddPlanting];
        foreach (var p in plantings)
        {
            plantingsAchievement.Count = plantingsAchievement.Count + 1;
            scoreBoard.TotalScore += plantingsAchievement.CurrentPoints;

        }
        
        //events
        var eventAchievement = achievements[AchievementType.AddEvent];
        foreach (var e in events)
        {
            eventAchievement.Count = eventAchievement.Count + 1;
            scoreBoard.TotalScore += eventAchievement.CurrentPoints;
        }

        // bonus calculation - observations
        if (obs.Count >= 10) scoreBoard.TotalScore += 10;
        if (obs.Count >= 50) scoreBoard.TotalScore += 50;
        if (obs.Count >= 100) scoreBoard.TotalScore += 100;
        if (obs.Count >= 500) scoreBoard.TotalScore += 500;
        if (obs.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (obs.Count >= 10000) scoreBoard.TotalScore += 10000;

        //get word count
        var wordCount = ObservationsService.GetObservationWordCount(obs.ToList());
        
        if (wordCount >= 500) scoreBoard.TotalScore += 10;
        if (wordCount >= 1000) scoreBoard.TotalScore += 50;
        if (wordCount >= 5000) scoreBoard.TotalScore += 100;
        if (wordCount >= 10000) scoreBoard.TotalScore += 500;
        if (wordCount >= 50000) scoreBoard.TotalScore += 1000;
        if (wordCount >= 100000) scoreBoard.TotalScore += 5000;
        if (wordCount >= 1000000) scoreBoard.TotalScore += 10000;

        var actionAchievement = achievements[AchievementType.AddAction];

        foreach (var todo in todos)
        {
            actionAchievement.Count = actionAchievement.Count + 1;
            scoreBoard.TotalScore += actionAchievement.CurrentPoints;
        }
            
        
        //bonus computation - actions
        if (todos.Count >= 10) scoreBoard.TotalScore += 10;
        if (todos.Count >= 50) scoreBoard.TotalScore += 50;
        if (todos.Count >= 100) scoreBoard.TotalScore += 100;
        if (todos.Count >= 500) scoreBoard.TotalScore += 500;
        if (todos.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (todos.Count >= 10000) scoreBoard.TotalScore += 10000;
        
        //bonus computation - inventory
        if (inventory.Count >= 10) scoreBoard.TotalScore += 10;
        if (inventory.Count >= 50) scoreBoard.TotalScore += 50;
        if (inventory.Count >= 100) scoreBoard.TotalScore += 100;
        if (inventory.Count >= 500) scoreBoard.TotalScore += 500;
        if (inventory.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (inventory.Count >= 10000) scoreBoard.TotalScore += 10000;

        //bonus computation - plantings
        if (plantings.Count >= 10) scoreBoard.TotalScore += 10;
        if (plantings.Count >= 25) scoreBoard.TotalScore += 25;
        if (plantings.Count >= 50) scoreBoard.TotalScore += 50;
        if (plantings.Count >= 100) scoreBoard.TotalScore += 100;
        if (plantings.Count >= 500) scoreBoard.TotalScore += 500;
        if (plantings.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (plantings.Count >= 10000) scoreBoard.TotalScore += 10000;
        
        //bonus computation - events
        if (events.Count >= 10) scoreBoard.TotalScore += 10;
        if (events.Count >= 25) scoreBoard.TotalScore += 25;
        if (events.Count >= 50) scoreBoard.TotalScore += 50;
        if (events.Count >= 100) scoreBoard.TotalScore += 100;
        if (events.Count >= 500) scoreBoard.TotalScore += 500;
        if (events.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (events.Count >= 10000) scoreBoard.TotalScore += 10000;

        // check for leveling up
        if (scoreBoard.TotalScore < 1000)
        {
            scoreBoard.Level = 1;
            scoreBoard.LevelMin = 0;
            scoreBoard.LevelMax = 1000;
        }

        
        if (scoreBoard.TotalScore > 1000)
        {
            scoreBoard.Level = 2;
            scoreBoard.LevelMin = 1000;
            scoreBoard.LevelMax = 5000;
        }

        if (scoreBoard.TotalScore > 5000)
        {
            scoreBoard.Level = 3;
            scoreBoard.LevelMin = 5000;
            scoreBoard.LevelMax = 10000;
        }

        if (scoreBoard.TotalScore > 10000)
        {
            scoreBoard.Level = 4;
            scoreBoard.LevelMin = 10000;
            scoreBoard.LevelMax = 20000;
        }

        if (scoreBoard.TotalScore > 20000)
        {
            scoreBoard.Level = 5;
            scoreBoard.LevelMin = 20000;
            scoreBoard.LevelMax = 30000;
        }

        if (scoreBoard.TotalScore > 30000)
        {
            scoreBoard.Level = 6;
            scoreBoard.LevelMin = 30000;
            scoreBoard.LevelMax = 50-00;
        }

        if (scoreBoard.TotalScore > 50000)
        {
            scoreBoard.Level = 7;
            scoreBoard.LevelMin = 50000;
            scoreBoard.LevelMax = 75000;
        }

        if (scoreBoard.TotalScore > 75000)
        {
            scoreBoard.Level = 8;
            scoreBoard.LevelMin = 75000;
            scoreBoard.LevelMax = 100000;
        }

        if (scoreBoard.TotalScore > 100000)
        {
            scoreBoard.Level = 9;
            scoreBoard.LevelMin = 100000;
            scoreBoard.LevelMax = 500000;
        }

        if (scoreBoard.TotalScore > 500000)
        {
            scoreBoard.Level = 10;
            scoreBoard.LevelMin = 500000;
            scoreBoard.LevelMax = 9999999999999999;
        }
        
        Log.Logger.Information("Computed user score of {TotalScore}", scoreBoard.TotalScore);

        return scoreBoard;
        
    }

    public static List<string> CheckForNewToDos(ServiceMode mode)
    {
        var newList = new List<string>();
        var events = Services.EventsService.GetAllEvents(mode);
        
        //loop through events, see if
        //1) we need to trigger a new action and
        //2) see if we need to post any warnings for upcoming dates
        foreach (var e in events)
        {
            
            if (e.NextDate >= DateTime.Today && e.LastTriggerDate < DateTime.Today)
            {
                //we have a possible action trigger
                if (e.NextDate.Date.AddDays(e.WarningDays * -1) < DateTime.Now)
                {
                    if (e.ToDoTrigger)
                    {
                        //check for an action in the db, create one if it is not found
                        if (!ToDoService.DoesToDoExist(mode, e.Description))
                        {
                            var t = new ToDo();
                            t.Assignee = new Person(1);
                            t.Assigner = new Person(1);
                            t.Description = e.Description;
                            t.CreationDate = DateTime.Now;
                            t.DueDate = e.NextDate;
                            t.StartDate = DateTime.Today;
                            t.ToDoStatus.Id = 1;
                            t.ToDoType.Id = 1;
                            
                            var rtn = ToDoRepository.Insert(todo: t);
                            
                            newList.Add("Action: " + t.Description + " (" + e.NextDate.ToShortDateString() + ")");
                            
                            e.LastTriggerDate = DateTime.Today;
                            // update the event so we know it was triggered
                            AnEventRepository.Update(e);
                            
                        }
                    }
                    else
                    {
                        //just post as a warning for dashboard?
                        newList.Add("Informational: " + e.Description + " (" + e.NextDate.ToShortDateString() + ")");
                    }
                    
                    
                }
            }
        }
        
        
        return newList;
    }
}