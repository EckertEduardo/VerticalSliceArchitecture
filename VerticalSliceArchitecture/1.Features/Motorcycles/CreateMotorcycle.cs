using Carter;
using FluentValidation;
using MediatR;
using System.Net;
using VerticalSliceArchitecture._4.Shared;
using VerticalSliceArchitecture.Contracts;
using VerticalSliceArchitecture.DataBase;
using VerticalSliceArchitecture.Entities;
using VerticalSliceArchitecture.Helpers;
using VerticalSliceArchitecture.Shared;
using static VerticalSliceArchitecture.Features.Motorcycles.CreateMotorcycle;

namespace VerticalSliceArchitecture.Features.Motorcycles;

public static class CreateMotorcycle
{
    public class Command : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string BrandingName { get; set; } = string.Empty;
        public float HP { get; set; }
        public decimal Price { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.BrandingName).NotNull().NotEmpty();
            RuleFor(x => x.HP).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly MyContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(MyContext context, IValidator<Command> validator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<Guid>(new Error(HttpStatusCode.BadRequest, $"{ErrorMessages.Validation(nameof(CreateMotorcycle))}", validationResult.ToString()));
            }

            var entity = new Motorcycle(request);

            await _context.Set<Motorcycle>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}

public class CreateMotorcycleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/motorcycle", async (CreateMotorcycleRequest request, ISender sender) =>
        {
            var command = new Command
            {
                Name = request.Name,
                BrandingName = request.BrandingName,
                HP = request.HP,
                Price = request.Price,
            };

            var result = await sender.Send(command);
            return EndpointHelper.GetReponse(result);
        });
    }
}