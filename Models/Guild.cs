
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Guild : CodeTable
    {
        public Person Author { get; set; }

        public long AuthorId { get { return this.Author == null ? 0 : this.Author.Id; } }

        public List<Plant> Plants { get; set; }

        public Guild()
        {
            this.Plants = new List<Plant>();
            this.Author = Person.Anonymous();
        }

        public bool AddPlant(Plant plant)
        {
            if (plant != null)
            {
                this.Plants.Add(plant);
                return true;
            }
            else
                return false;
        }
    }
}
