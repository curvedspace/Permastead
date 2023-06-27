
namespace Models
{
    public class GardenBedType : CodeTable
    {
        public Person Author { get; set; }

        public long AuthorId { get { return this.Author == null ? 0 : this.Author.Id; } }

        public GardenBedType()
        {
            this.Author = Person.Anonymous();
        }

        public GardenBedType(long id) : this()
        {
            this.Id = id;
        }
    }
}
