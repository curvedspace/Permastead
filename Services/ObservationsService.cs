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

            return observations;
        }

        public static List<Observation> GetObservationsForAllEntities(ServiceMode mode)
        {
            // get base observations
            var observations = GetObservations(mode);
            
            // now get planting observations, convert to Observation objects
            var plantingObs = PlantingsRepository.GetAllPlantingObservations(DataConnection.GetLocalDataSource());

            foreach (var po in plantingObs)
            {
                var newObs = new Observation()
                {
                    AsOfDate = po.AsOfDate,
                    Author = po.Author,
                    Comment = "(PG:" + po.Planting.Description + ") " + po.Comment,
                    CommentType = po.CommentType,
                    CreationDate = po.CreationDate,
                    EndDate = po.EndDate,
                    Id = po.Id,
                    StartDate = po.StartDate
                };
                
                observations.Add(newObs);

            }

            return observations;
        }

        public static bool InsertRecord(ServiceMode mode, Observation obs)
        {
            var rtnValue = false;

            if (mode == ServiceMode.Local)
            {
                rtnValue = ObservationRepository.InsertObservation(DataConnection.GetLocalDataSource(), obs);
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
    }
}
