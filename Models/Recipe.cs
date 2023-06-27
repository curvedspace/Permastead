using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Recipe : CodeTable
    {
        public Person Author { get; set; }

        public long AuthorId { get { return Author == null ? 0 : Author.Id; } }

        public FeedSource FeedSource { get; set; }

        public long FeedSourceId { get { return FeedSource == null ? 0 : FeedSource.Id; } }

        public IList<Ingredient> Ingredients { get; set; }

        public IList<string> Steps { get; set; }

        public decimal OutputSize { get; set; }

        public string OutputMeasurementType { get; set; } = string.Empty;

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();

            CreationDate = DateTime.Now;
            StartDate = DateTime.Today;
            EndDate = DateTime.MaxValue;

            FeedSource = new FeedSource();
            Author = Person.Anonymous();
        }

        public long NumberOfIngredients => Ingredients.Count;

        public long NumberOfSteps => Steps.Count;

        public override string ToString()
        {
            return "Name: " + Description + ", \n";
        }
    }
}
