
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class MeasurementUnit : CodeTable
    {
        public Person Author { get; set; }

        public long AuthorId => this.Author.Id;

        public MeasurementUnit()
        {
            this.Author = Person.Anonymous();
        }

        public MeasurementUnit(long id) : this()
        {
            this.Id = id;
        }
    }
}
