using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using DataAccess.Local;
using Models;

namespace Services
{
    public static class ObservationsService
    {
        public static List<Observation> GetObservations(ServiceMode mode)
        {
            var observations = new List<Observation>();

            if (mode == ServiceMode.Local)
            {
                observations = ObservationRepository.GetObservations(DataConnection.GetLocalDataSource());
            }
            else
            {
                observations = DataAccess.Server.ObservationRepository.GetObservations(DataConnection.GetServerConnectionString());
            }

            return observations;
        }

        public static List<Observation> GetObservationsForAllEntities(ServiceMode mode)
        {
            // get base observations
            var observations = GetObservations(mode);
            
            // now get planting observations, convert to Observation objects
            var plantingObs = PlantingsService.GetPlantingObservations(mode);
            var seedPacketObs = PlantingsService.GetSeedPacketObservations(mode);
            var personObs = PersonService.GetAllPersonObservations(mode);
            var animalsObs = AnimalService.GetAnimalObservations(mode);
            var invObs = InventoryService.GetAllInventoryObservations(mode);
            var presObs = FoodPreservationService.GetAllPreservationObservations(mode);
            
            // add the planting observations 
            foreach (var po in plantingObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = po.AsOfDate,
                    Author = po.Author,
                    Annotation = "(PG:" + po.Planting.Description + ")",
                    Comment = po.Comment,
                    CommentType = po.CommentType,
                    CreationDate = po.CreationDate,
                    EndDate = po.EndDate,
                    Id = po.Id,
                    StartDate = po.StartDate
                };
                
                observations.Add(newObs);
            }
            
            // add the seed packet observations
            foreach (var po in seedPacketObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = po.AsOfDate,
                    Author = po.Author,
                    Annotation = "(S:" + po.SeedPacket.Description + ")",
                    Comment = po.Comment,
                    CommentType = po.CommentType,
                    CreationDate = po.CreationDate,
                    EndDate = po.EndDate,
                    Id = po.Id,
                    StartDate = po.StartDate
                };
                
                observations.Add(newObs);
            }
            
            // add the people observations 
            foreach (var po in personObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = po.AsOfDate,
                    Author = po.Author,
                    Annotation = "(C:" + po.Person.FullName() + ")",
                    Comment =  po.Comment,
                    CommentType = po.CommentType,
                    CreationDate = po.CreationDate,
                    EndDate = po.EndDate,
                    Id = po.Id,
                    StartDate = po.StartDate
                };
                
                observations.Add(newObs);
            }
            
            // add the animal observations 
            foreach (var ao in animalsObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = ao.AsOfDate,
                    Author = ao.Author,
                    Annotation = "(A:" + ao.Animal.Name + ")",
                    Comment =  ao.Comment,
                    CommentType = ao.CommentType,
                    CreationDate = ao.CreationDate,
                    EndDate = ao.EndDate,
                    Id = ao.Id,
                    StartDate = ao.StartDate
                };
                
                observations.Add(newObs);
            }
            
            // add the inventory observations 
            foreach (var io in invObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = io.AsOfDate,
                    Author = io.Author,
                    Annotation = "(I:" + io.Inventory.Description + ")",
                    Comment = io.Comment,
                    CommentType = io.CommentType,
                    CreationDate = io.CreationDate,
                    EndDate = io.EndDate,
                    Id = io.Id,
                    StartDate = io.StartDate
                };
                
                observations.Add(newObs);
            }
            
            // add the food preservation observations 
            foreach (var observation in presObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = observation.AsOfDate,
                    Author = observation.Author,
                    Annotation = "(PR:" + observation.FoodPreservation.Name + ")",
                    Comment = observation.Comment,
                    CommentType = observation.CommentType,
                    CreationDate = observation.CreationDate,
                    EndDate = observation.EndDate,
                    Id = observation.Id,
                    StartDate = observation.StartDate
                };
                
                observations.Add(newObs);
            }

            return observations;
        }
        
        public static List<CommentType> GetCommentTypes(ServiceMode mode)
        {
            var commentTypes = new List<CommentType>();

            if (mode == ServiceMode.Local)
            {
                commentTypes = CommentTypeRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                commentTypes = DataAccess.Server.CommentTypeRepository.GetAll(DataConnection.GetServerConnectionString());
            }
            
            
            commentTypes = commentTypes.OrderBy(x => x.Description).ToList();
            
            return commentTypes;
        }

        public static bool InsertRecord(ServiceMode mode, Observation obs)
        {
            var rtnValue = false;

            if (mode == ServiceMode.Local)
            {
                rtnValue = ObservationRepository.InsertObservation(DataConnection.GetLocalDataSource(), obs);
            }
            else
            {
                rtnValue = DataAccess.Server.ObservationRepository.InsertObservation(DataConnection.GetServerConnectionString(), obs);
            }

            return rtnValue;
        }
        
        /// <summary>
        /// Commits the observation record to the correct table.
        /// There are different tables depending on the observation type for updates.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="obs"></param>
        /// <returns></returns>
        public static bool CommitRecord(ServiceMode mode, Observation obs)
        {
            var rtnValue = false;

            if (mode == ServiceMode.Local)
            {
                if (obs.Id == 0)
                {
                    rtnValue = ObservationRepository.InsertObservation(DataConnection.GetLocalDataSource(), obs);
                }
                else
                {
                    switch (obs.CommentTypeId)
                    {
                        case 4: //animal
                            rtnValue = AnimalRepository.UpdateAnimalObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        case 5: //contact
                            rtnValue = PersonRepository.UpdatePersonObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        case 6: //seeds
                            rtnValue = SeedPacketRepository.UpdateSeedPacketObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        case 7: //planting
                            rtnValue = PlantingsRepository.UpdatePlantingObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        case 8: //preservation
                            rtnValue = PreservationRepository.UpdatePreservationObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        case 9: //inventory
                            rtnValue = InventoryRepository.UpdateInventoryObservation(DataConnection.GetLocalDataSource(), obs);
                            break;
                        default:
                            rtnValue = ObservationRepository.UpdateObservation(DataConnection.GetLocalDataSource(),
                                obs);
                            break;
                    }
                }
            }
            else
            {
                if (obs.Id == 0)
                {
                    rtnValue = DataAccess.Server.ObservationRepository.InsertObservation(DataConnection.GetServerDataSource(), obs);
                }
                else
                {
                    {
                        switch (obs.CommentTypeId)
                        {
                            case 4: //animal
                                rtnValue = DataAccess.Server.AnimalRepository.UpdateAnimalObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            case 5: //contact
                                rtnValue = DataAccess.Server.PersonRepository.UpdatePersonObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            case 6: //seeds
                                rtnValue = DataAccess.Server.SeedPacketRepository.UpdateSeedPacketObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            case 7: //planting
                                rtnValue = DataAccess.Server.PlantingsRepository.UpdatePlantingObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            case 8: //preservation
                                rtnValue = DataAccess.Server.PreservationRepository.UpdatePreservationObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            case 9: //inventory
                                rtnValue = DataAccess.Server.InventoryRepository.UpdateInventoryObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                            default:
                                rtnValue = DataAccess.Server.ObservationRepository.UpdateObservation(DataConnection.GetServerDataSource(), obs);
                                break;
                        }
                    }
                }
            }

            return rtnValue;
        }

        public static long GetObservationWordCount(List<Observation> observations)
        {
            long wordCount = 0;

            foreach (Observation ob in observations)
            {
                int index = 0;
                
                // skip whitespace until first word
                while (index < ob.Comment.Length && char.IsWhiteSpace(ob.Comment[index]))
                    index++;

                while (index < ob.Comment.Length)
                {
                    // check if current char is part of a word
                    while (index < ob.Comment.Length && !char.IsWhiteSpace(ob.Comment[index]))
                        index++;

                    wordCount++;

                    // skip whitespace until next word
                    while (index < ob.Comment.Length && char.IsWhiteSpace(ob.Comment[index]))
                        index++;
                }
            }

            return wordCount;
        }

        public static Observation GetCurrentYearInReview(ServiceMode mode, int year)
        {
            // try to get the existing YIR record
            Observation obs = null;
            
            if (mode == ServiceMode.Local)
            { 
                obs = ObservationRepository.GetYearInReviewObservation(DataConnection.GetLocalDataSource(), year);
            }
            else
            { 
                obs = DataAccess.Server.ObservationRepository.GetYearInReviewObservation(DataConnection.GetServerConnectionString(), year);
            }
            
            if (obs == null)
            {
                // get a default year in review comment if none found
                obs = new Observation()
                {
                    AsOfDate = DateTime.Now,
                    Author = Person.Gaia(),
                    StartDate = new DateTime(year, 1, 1),
                    EndDate = new DateTime(year, 12, 31),
                    CommentType = new CommentType(),
                    Comment = ""
                };
            
                var commentTypes = GetCommentTypes(mode);

                foreach (var ct in commentTypes)
                {
                    if (ct.Code == "YIR")
                        obs.CommentType = ct;
                }

            }
            
            return obs;

        }

        public static Observation GetObservationById(ServiceMode mode, long id, string subType)
        {
            Observation obs = null;

            if (mode == ServiceMode.Local)
            {
                switch (subType)
                {
                    case "Animal":
                        obs = AnimalRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    case "Contact":
                        obs = PersonRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    case "Planting":
                        obs = PlantingsRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    case "Preservation":
                        obs = PreservationRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    case "Inventory":
                        obs = InventoryRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    case "Seeds":
                        obs = SeedPacketRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    default:
                    {
                        obs = ObservationRepository.GetObservationById(DataConnection.GetLocalDataSource(), id);
                        break;
                    }
                }
            }
            else
            {
                switch (subType)
                {
                    case "Animal":
                        obs = DataAccess.Server.AnimalRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    case "Contact":
                        obs = DataAccess.Server.PersonRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    case "Planting":
                        obs = DataAccess.Server.PlantingsRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    case "Preservation":
                        obs = DataAccess.Server.PreservationRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    case "Inventory":
                        obs = DataAccess.Server.InventoryRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    case "Seeds":
                        obs = DataAccess.Server.SeedPacketRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    default:
                    {
                        obs = DataAccess.Server.ObservationRepository.GetObservationById(DataConnection.GetServerConnectionString(),id); 
                        break;
                    }
                }
                
            }

            return obs;
        }
    }
}
