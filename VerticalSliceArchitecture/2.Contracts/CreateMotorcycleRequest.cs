namespace VerticalSliceArchitecture.Contracts
{
    public sealed class CreateMotorcycleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string BrandingName { get; set; } = string.Empty;
        public float HP { get; set; }
        public decimal Price { get; set; }
    }
}
