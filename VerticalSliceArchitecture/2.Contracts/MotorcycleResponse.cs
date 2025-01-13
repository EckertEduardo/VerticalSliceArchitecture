using VerticalSliceArchitecture.Entities;

namespace VerticalSliceArchitecture.Contracts
{
    public sealed class MotorcycleResponse
    {
        public string Name { get; set; } = string.Empty;
        public string BrandingName { get; set; } = string.Empty;
        public float HP { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public MotorcycleResponse(Motorcycle entity)
        {
            Name = entity.Name;
            BrandingName = entity.BrandingName;
            HP = entity.HP;
            Price = entity.Price;
            CreatedAt = entity.CreatedAt;
        }
    }
}
