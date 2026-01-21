using System.Data.Common;
using DataAccess;
using DataAccess.Local;
using Models;
using Serilog;
using Serilog.Core;

namespace Services;

public static class ScoreBoardService
{
    public static ScoreBoard ComputeTotalScore(ServiceMode mode, int periodLookBack = 30)
    {
        var scoreBoard = new ScoreBoard();
        
        var periodStartDate = DateTime.Now.AddDays(-periodLookBack);
        long periodObservations = 0;
        long periodActions = 0;
        decimal periodActObsRatio = 0;

        //set up achievements
        var achievements = new Dictionary<AchievementType,Achievement>
        {
            {
                AchievementType.AddObservation,
                new Achievement() { Type = AchievementType.AddObservation, InitialPoints = 50, IterationMultiplier = 0.75m}
            },
            { AchievementType.AddAction, new Achievement() { Type = AchievementType.AddAction, InitialPoints = 50 } },
            { AchievementType.AddInventory, new Achievement() { Type = AchievementType.AddInventory, InitialPoints = 50 } },
            { AchievementType.AddPlanting, new Achievement() { Type = AchievementType.AddPlanting, InitialPoints = 100 } },
            { AchievementType.AddEvent, new Achievement() { Type = AchievementType.AddEvent, InitialPoints = 100 } },
            { AchievementType.AddFermentation, new Achievement() { Type = AchievementType.AddObservation, InitialPoints = 100 } },
            { AchievementType.AddSeedPacket, new Achievement() { Type = AchievementType.AddSeedPacket, InitialPoints = 100 } },
            { AchievementType.SaveSeed, new Achievement() { Type = AchievementType.SaveSeed, InitialPoints = 50 } },
            { AchievementType.AddHarvest, new Achievement() { Type = AchievementType.AddHarvest, InitialPoints = 100 } },
            { AchievementType.AddProcedure, new Achievement() { Type = AchievementType.AddProcedure, InitialPoints = 100 } },
            { AchievementType.AddPreservation, new Achievement() { Type = AchievementType.AddPreservation, InitialPoints = 100 } },
            { AchievementType.AddPeople, new Achievement() { Type = AchievementType.AddPeople, InitialPoints = 10 } }
        };

        var obs = ObservationsService.GetObservationsForAllEntities(mode);
        var todos = ToDoService.GetAllToDos(mode);
        var inventory = InventoryService.GetAllInventory(mode);
        var plantings = PlantingsService.GetPlantings(mode);
        var seedPackets = PlantingsService.GetSeedPackets(mode);
        var events = EventsService.GetAllEvents(mode);
        var harvests = HarvestService.GetAllHarvests(mode);
        var preservations = FoodPreservationService.GetAll(mode);
        var procedures = ProceduresService.GetAllProcedures(mode);
        var people = PersonService.GetAllPeople(mode);

        scoreBoard.Actions = todos.Count;
        scoreBoard.Observations = obs.Count;
        scoreBoard.Plantings = plantings.Count;
        scoreBoard.SeedPackets = seedPackets.Count;
        scoreBoard.Events = events.Count;
        scoreBoard.Harvests = harvests.Count;
        scoreBoard.Procedures = procedures.Count;
        scoreBoard.Preservations = preservations.Count;
        scoreBoard.People = people.Count;

        //iterate through data
       
        //observations
        var obsAchievement = achievements[AchievementType.AddObservation];
        foreach (var ob in obs)
        {
            obsAchievement.Count += 1;
            scoreBoard.TotalScore += obsAchievement.CurrentPoints;
            
            if (ob.StartDate >= periodStartDate)
            {
                periodObservations += 1;
            }
        }

        //inventory
        var invAchievement = achievements[AchievementType.AddInventory];
        foreach (var inv in inventory)
        {
            invAchievement.Count += 1;
            scoreBoard.TotalScore += invAchievement.CurrentPoints;
        }

        //plantings
        var plantingsAchievement = achievements[AchievementType.AddPlanting];
        foreach (var p in plantings)
        {
            plantingsAchievement.Count += 1;
            scoreBoard.TotalScore += plantingsAchievement.CurrentPoints;
        }
        
        //seed packets
        var seedPacketsAchievement = achievements[AchievementType.AddSeedPacket];
        foreach (var p in seedPackets)
        {
            seedPacketsAchievement.Count += 1;
            scoreBoard.TotalScore += seedPacketsAchievement.CurrentPoints;
        }
        
        //events
        var eventAchievement = achievements[AchievementType.AddEvent];
        foreach (var e in events)
        {
            eventAchievement.Count += 1;
            scoreBoard.TotalScore += eventAchievement.CurrentPoints;
        }
        
        //harvests
        var harvestsAchievement = achievements[AchievementType.AddHarvest];
        foreach (var e in harvests)
        {
            harvestsAchievement.Count += 1;
            scoreBoard.TotalScore += harvestsAchievement.CurrentPoints;
        }
        
        //procedures
        var proceduresAchievement = achievements[AchievementType.AddProcedure];
        foreach (var e in procedures)
        {
            proceduresAchievement.Count += 1;
            scoreBoard.TotalScore += proceduresAchievement.CurrentPoints;
        }
        
        //preservations
        var preservationAchievement = achievements[AchievementType.AddPreservation];
        foreach (var e in preservations)
        {
            preservationAchievement.Count += 1;
            scoreBoard.TotalScore += preservationAchievement.CurrentPoints;
        }
        
        //people
        var peopleAchievement = achievements[AchievementType.AddPeople];
        foreach (var e in people)
        {
            peopleAchievement.Count += 1;
            scoreBoard.TotalScore += peopleAchievement.CurrentPoints;
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
            actionAchievement.Count += 1;
            scoreBoard.TotalScore += actionAchievement.CurrentPoints;
            
            if (todo.StartDate >= periodStartDate)
            {
                periodActions += 1;
            }
        }

        if (periodObservations > 0)
        {
            periodActObsRatio = ((1m * periodActions / (periodActions + periodObservations)) * 100m);
        }

        scoreBoard.ActionsToObservationsRatio = periodActObsRatio;
        
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
        
        //bonus computation - seed packets
        if (seedPackets.Count >= 10) scoreBoard.TotalScore += 10;
        if (seedPackets.Count >= 25) scoreBoard.TotalScore += 25;
        if (seedPackets.Count >= 50) scoreBoard.TotalScore += 50;
        if (seedPackets.Count >= 100) scoreBoard.TotalScore += 100;
        if (seedPackets.Count >= 500) scoreBoard.TotalScore += 500;
        if (seedPackets.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (seedPackets.Count >= 10000) scoreBoard.TotalScore += 10000;
        
        //bonus computation - procedures
        if (procedures.Count >= 10) scoreBoard.TotalScore += 10;
        if (procedures.Count >= 25) scoreBoard.TotalScore += 25;
        if (procedures.Count >= 50) scoreBoard.TotalScore += 50;
        if (procedures.Count >= 100) scoreBoard.TotalScore += 100;
        if (procedures.Count >= 500) scoreBoard.TotalScore += 500;
        if (procedures.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (procedures.Count >= 10000) scoreBoard.TotalScore += 10000;
        
        //bonus computation - events
        if (events.Count >= 10) scoreBoard.TotalScore += 10;
        if (events.Count >= 25) scoreBoard.TotalScore += 25;
        if (events.Count >= 50) scoreBoard.TotalScore += 50;
        if (events.Count >= 100) scoreBoard.TotalScore += 100;
        if (events.Count >= 500) scoreBoard.TotalScore += 500;
        if (events.Count >= 1000) scoreBoard.TotalScore += 1000;
        if (events.Count >= 10000) scoreBoard.TotalScore += 10000;
        
        //bonus computation - people
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
        var events = EventsService.GetAllEvents(mode);
        
        //loop through events, see if
        //1) we need to trigger a new action and
        //2) see if we need to post any warnings for upcoming dates
        foreach (var e in events)
        {
            
            if (e.NextDate >= DateTime.Today && e.LastTriggerDate <= DateTime.Today)
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
                            t.DueDate = e.NextDate.AddDays(e.WarningDays);
                            t.StartDate = DateTime.Today;
                            t.ToDoStatus.Id = 1;
                            t.ToDoType.Id = 1;

                            var rtn = ToDoService.CommitRecord(mode, t); //ToDoRepository.Insert(todo: t);
                            
                            newList.Add("Action: " + t.Description + " (" + e.NextDate.ToShortDateString() + ")");
                            
                            e.LastTriggerDate = DateTime.Today;
                            // update the event so we know it was triggered
                            EventsService.CommitRecord(mode, e);
                            //AnEventRepository.Update(e);
                            
                        }
                    }
                    else
                    {
                        //just post as a warning for dashboard?
                        newList.Add("Informational: " + e.Description + " (" + e.NextDate.ToShortDateString() + ")");
                        
                        e.LastTriggerDate = DateTime.Today;
                        // update the event so we know it was triggered
                        EventsService.CommitRecord(mode, e);
                    }
                }
            }
        }
        
        return newList;
    }

    public static int GetEarliestRecordYear(ServiceMode mode)
    {
        DateTime earliestDate;
        
        if (mode == ServiceMode.Local)
        {
            earliestDate = ObservationRepository.GetEarliestObservationDate(DataConnection.GetLocalDataSource());
        }
        else
        {
            earliestDate = DataAccess.Server.ObservationRepository.GetEarliestObservationDate(DataConnection.GetServerConnectionString());
        }

        return earliestDate.Year;
    }
}