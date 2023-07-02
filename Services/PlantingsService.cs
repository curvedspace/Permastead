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
    
    public static List<PlantingState> GetPlantingStates(ServiceMode mode)
    {
        var states = new List<PlantingState>();

        if (mode == ServiceMode.Local)
        {
            states = PlantingStateRepository.GetAll(DataConnection.GetLocalDataSource());
        }

        return states;
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
}