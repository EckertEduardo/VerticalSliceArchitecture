using static VerticalSliceArchitecture.Features.Motorcycles.CreateMotorcycle;

namespace VerticalSliceArchitecture.Entities
{
    public sealed class Motorcycle
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BrandingName { get; set; } = string.Empty;
        public float HP { get; set; }
        public decimal Price { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        private Motorcycle()
        {

        }
        //public Motorcycle() { }
        public Motorcycle(Command request)
        {
            Id = Guid.NewGuid();
            Name = request.Name;
            BrandingName = request.BrandingName;
            HP = request.HP;
            Price = request.Price;
            CreatedAt = DateTime.UtcNow;
        }
        public void Delete()
        {
            Deleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
