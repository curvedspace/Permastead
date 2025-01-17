using DataAccess;
using DataAccess.Local;
using Models;
using NBitcoin.Protocol;

namespace Services;

public static class PlantingsService
{
    public static List<Planting> GetPlantings(ServiceMode mode)
    {
        var plantings = new List<Planting>();

        if (mode == ServiceMode.Local)
        {
            plantings = PlantingsRepository.GetAllPlantings(DataConnection.GetLocalDataSource());
        }
        else
        {
            plantings = DataAccess.Server.PlantingsRepository.GetAllPlantings(DataConnection.GetServerConnectionString());
        }

        return plantings;
    }
    
    public static List<Planting> GetPlantingsByPlantedDate(ServiceMode mode)
    {
        var plantings = new List<Planting>();

        if (mode == ServiceMode.Local)
        {
            plantings = PlantingsRepository.GetAllPlantings(DataConnection.GetLocalDataSource(), true);
        }
        else
        {
            plantings = DataAccess.Server.PlantingsRepository.GetAllPlantings(DataConnection.GetServerConnectionString(), true);
        }

        return plantings;
    }

    public static Planting GetPlantingFromId(ServiceMode mode, long id) 
    {
        var planting = new Planting();

        if (mode == ServiceMode.Local)
        {
            planting = PlantingsRepository.GetPlantingFromId(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            planting = DataAccess.Server.PlantingsRepository.GetPlantingFromId(DataConnection.GetServerConnectionString(), id);
        }

        return planting;
    }
    
    public static SeedPacket GetSeedPacketFromId(ServiceMode mode, long id) 
    {
        var sp = new SeedPacket();

        if (mode == ServiceMode.Local)
        {
            sp = SeedPacketRepository.GetFromId(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            sp = DataAccess.Server.SeedPacketRepository.GetFromId(DataConnection.GetServerConnectionString(), id);
        }

        return sp;
    }
    
    public static List<GardenBed> GetGardenBeds(ServiceMode mode)
    {
        var beds = new List<GardenBed>();

        if (mode == ServiceMode.Local)
        {
            beds = GardenBedRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            beds = DataAccess.Server.GardenBedRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return beds;
    }
    
    public static GardenBed GetGardenBedFromId(ServiceMode mode, long id) 
    {
        var gardenBed = new GardenBed();

        if (mode == ServiceMode.Local)
        {
            gardenBed = GardenBedRepository.GetFromId(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            gardenBed = DataAccess.Server.GardenBedRepository.GetFromId(DataConnection.GetServerConnectionString(), id);
        }

        return gardenBed;
    }
    
    public static List<GardenBedType> GetGardenBedTypes(ServiceMode mode)
    {
        var bedTypes = new List<GardenBedType>();

        if (mode == ServiceMode.Local)
        {
            bedTypes = GardenBedTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            bedTypes = DataAccess.Server.GardenBedTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return bedTypes;
    }
    
    public static List<PlantingState> GetPlantingStates(ServiceMode mode)
    {
        var states = new List<PlantingState>();

        if (mode == ServiceMode.Local)
        {
            states = PlantingStateRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            states = DataAccess.Server.PlantingStateRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return states;
    }
    
    public static List<StarterType> GetStarterTypes(ServiceMode mode)
    {
        var types = new List<StarterType>();

        if (mode == ServiceMode.Local)
        {
            types = StarterTypeRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            types = DataAccess.Server.StarterTypeRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return types;
    }
    
    public static List<Seasonality> GetSeasonalities(ServiceMode mode)
    {
        var seasonalities = new List<Seasonality>();

        if (mode == ServiceMode.Local)
        {
            seasonalities = SeasonalityRepository.GetAll(DataConnection.GetLocalDataSource());
        }
        else
        {
            seasonalities = DataAccess.Server.SeasonalityRepository.GetAll(DataConnection.GetServerConnectionString());
        }

        return seasonalities;
    }
    
    public static List<SeedPacket> GetSeedPackets(ServiceMode mode, bool byPlant = true)
    {
        var seedPackets = new List<SeedPacket>();

        if (mode == ServiceMode.Local)
        {
            if (byPlant)
                seedPackets = SeedPacketRepository.GetAllByPlant(DataConnection.GetLocalDataSource());
            else
                seedPackets = SeedPacketRepository.GetAllByDescription(DataConnection.GetLocalDataSource());
        }
        else
        {
            if (byPlant)
                seedPackets = DataAccess.Server.SeedPacketRepository.GetAllByPlant(DataConnection.GetServerConnectionString());
            else
                seedPackets = DataAccess.Server.SeedPacketRepository.GetAllByDescription(DataConnection.GetServerConnectionString());
        }

        return seedPackets;
    }
    
    public static List<Plant> GetPlants(ServiceMode mode)
    {
        var plants = new List<Plant>();

        if (mode == ServiceMode.Local)
        {
            plants = PlantRepository.GetPlants(DataConnection.GetLocalDataSource());
        }
        else
        {
            plants = DataAccess.Server.PlantRepository.GetPlants(DataConnection.GetServerConnectionString());
        }

        return plants;
    }

    public static List<PlantingObservation> GetObservationsForPlanting(ServiceMode mode, long id)
    {
        var obs = new List<PlantingObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = PlantingsRepository.GetAllObservationsForPlanting(DataConnection.GetLocalDataSource(), id);
        }
        else
        {
            obs = DataAccess.Server.PlantingsRepository.GetAllObservationsForPlanting(DataConnection.GetServerConnectionString(), id);
        }

        return obs;
    }
    
    public static List<PlantingObservation> GetPlantingObservations(ServiceMode mode)
    {
        var obs = new List<PlantingObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = PlantingsRepository.GetAllPlantingObservations(DataConnection.GetLocalDataSource());
        }
        else
        {
            obs = DataAccess.Server.PlantingsRepository.GetAllPlantingObservations(DataConnection.GetServerConnectionString());
        }

        return obs;
    }
    
    public static bool AddPlantingObservation(ServiceMode mode, PlantingObservation obs)
    {
        bool rtnValue = false;
        
        if (obs != null)
        {
            if (obs.Planting.Id > 0)
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    PlantingsRepository.InsertPlantingObservation(
                        DataConnection.GetLocalDataSource(), obs);
                }
                else
                {
                    DataAccess.Server.PlantingsRepository.InsertPlantingObservation(
                        DataConnection.GetServerConnectionString(), 
                        obs);
                }
            }
        }

        return rtnValue;
    }
    
    public static List<SeedPacketObservation> GetSeedPacketObservations(ServiceMode mode)
    {
        var obs = new List<SeedPacketObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = SeedPacketRepository.GetAllSeedPacketObservations(DataConnection.GetLocalDataSource());
        }
        else
        {
            obs = DataAccess.Server.SeedPacketRepository.GetAllSeedPacketObservations(DataConnection.GetServerConnectionString());
        }

        return obs;
    }
    
    public static List<SeedPacket> GetSeedPacketForPlant(ServiceMode mode, long plantId)
    {
        var obs = new List<SeedPacket>();

        if (mode == ServiceMode.Local)
        {
            obs = SeedPacketRepository.GetAllForPlant(DataConnection.GetLocalDataSource(), plantId);
        }
        else
        {
            obs = DataAccess.Server.SeedPacketRepository.GetAllForPlant(DataConnection.GetServerConnectionString(), plantId);
        }

        return obs;
    }
    
    public static List<Planting> GetPlantingsForPlant(ServiceMode mode, long plantId)
    {
        var obs = new List<Planting>();

        if (mode == ServiceMode.Local)
        {
            obs = PlantingsRepository.GetAllForPlant(DataConnection.GetLocalDataSource(), plantId);
        }
        else
        {
            obs = DataAccess.Server.PlantingsRepository.GetAllForPlant(DataConnection.GetServerConnectionString(), plantId);
        }

        return obs;
    }

    public static bool CommitRecord(ServiceMode mode, Planting? planting)
    {
        bool rtnValue = false;
        
        if (planting != null)
        {
            if (planting.Id > 0)
            {
                if (mode == ServiceMode.Local)
                {
                    PlantingsRepository.Update(planting);
                }
                else
                { 
                    DataAccess.Server.PlantingsRepository.Update(planting);
                }
            }
            else
            {
                // insert new record
                if (mode == ServiceMode.Local)
                {
                    PlantingsRepository.Insert(planting);
                }
                else
                {
                    DataAccess.Server.PlantingsRepository.Insert(planting);
                }
            }
        }

        return rtnValue;
        
    }
    
    public static List<Entity> GetPlantingsAsEntityList(ServiceMode mode)
    {
        var items = new List<Entity>();
        var plantings = GetPlantings(mode).OrderBy(x => x.Description);

        foreach (var plt in plantings)
        {
            var entity = new Entity();
            entity.Id = plt.Id;
            entity.Name = plt.Description + " (" + plt.Bed.Description + ")";
            items.Add(entity);
        }

        return items;
    }
}