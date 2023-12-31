using DataAccess;
using DataAccess.Local;
using Models;

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

        return plantings;
    }
    
    public static List<Planting> GetPlantingsByPlantedDate(ServiceMode mode)
    {
        var plantings = new List<Planting>();

        if (mode == ServiceMode.Local)
        {
            plantings = PlantingsRepository.GetAllPlantings(DataConnection.GetLocalDataSource(), true);
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

        return planting;
    }
    
    public static SeedPacket GetSeedPacketFromId(ServiceMode mode, long id) 
    {
        var sp = new SeedPacket();

        if (mode == ServiceMode.Local)
        {
            sp = SeedPacketRepository.GetFromId(DataConnection.GetLocalDataSource(), id);
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

        return beds;
    }
    
    public static GardenBed GetGardenBedFromId(ServiceMode mode, long id) 
    {
        var gardenBed = new GardenBed();

        if (mode == ServiceMode.Local)
        {
            gardenBed = GardenBedRepository.GetFromId(DataConnection.GetLocalDataSource(), id);
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

        return bedTypes;
    }
    
    public static List<PlantingState> GetPlantingStates(ServiceMode mode)
    {
        var states = new List<PlantingState>();

        if (mode == ServiceMode.Local)
        {
            states = PlantingStateRepository.GetAll(DataConnection.GetLocalDataSource());
        }

        return states;
    }
    
    public static List<Seasonality> GetSeasonalities(ServiceMode mode)
    {
        var seasonalities = new List<Seasonality>();

        if (mode == ServiceMode.Local)
        {
            seasonalities = SeasonalityRepository.GetAll(DataConnection.GetLocalDataSource());
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

        return seedPackets;
    }
    
    public static List<Plant> GetPlants(ServiceMode mode)
    {
        var plants = new List<Plant>();

        if (mode == ServiceMode.Local)
        {
            plants = PlantRepository.GetPlants(DataConnection.GetLocalDataSource());
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

        return obs;
    }

    public static List<SeedPacketObservation> GetObservationsForSeedPacket(ServiceMode mode, long id)
    {
        var obs = new List<SeedPacketObservation>();

        if (mode == ServiceMode.Local)
        {
            obs = SeedPacketRepository.GetAllObservationsForSeedPacket(DataConnection.GetLocalDataSource(), id);
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

        return obs;
    }
    
    public static List<Planting> GetPlantingsForPlant(ServiceMode mode, long plantId)
    {
        var obs = new List<Planting>();

        if (mode == ServiceMode.Local)
        {
            obs = PlantingsRepository.GetAllForPlant(DataConnection.GetLocalDataSource(), plantId);
        }

        return obs;
    }

    public static bool CommitRecord(ServiceMode serviceMode, Planting? planting)
    {
        bool rtnValue = false;
        
        if (planting != null)
        {
            if (planting.Id > 0)
            {
                PlantingsRepository.Update(planting);
            }
            else
            {
                // insert new record
                PlantingsRepository.Insert(planting);
            }
        }

        return rtnValue;
    }
}