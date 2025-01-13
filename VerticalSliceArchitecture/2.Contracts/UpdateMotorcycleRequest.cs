namespace VerticalSliceArchitecture._2.Contracts
{
    public sealed class UpdateMotorcycleRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BrandingName { get; set; } = string.Empty;
        public float HP { get; set; }
        public decimal Price { get; set; }
    }
}
