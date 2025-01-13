using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VerticalSliceArchitecture._2.Contracts;
using VerticalSliceArchitecture._4.Shared;
using VerticalSliceArchitecture.Contracts;
using VerticalSliceArchitecture.DataBase;
using VerticalSliceArchitecture.Entities;
using VerticalSliceArchitecture.Helpers;
using VerticalSliceArchitecture.Shared;
using static VerticalSliceArchitecture._1.Features.Motorcycles.UpdateMotorcycle;

namespace VerticalSliceArchitecture._1.Features.Motorcycles
{
    public static class UpdateMotorcycle
    {
        public class Command : IRequest<Result<MotorcycleResponse>>
        {
            public Guid Id { get; }
            public string? Name { get; } = string.Empty;
            public string? BrandingName { get; } = string.Empty;
            public float? HP { get; }
            public decimal? Price { get; }

            public Command(Guid id, string? name, string? brandingName, float? hP, decimal? price)
            {
                Id = id;
                Name = name;
                BrandingName = brandingName;
                HP = hP;
                Price = price;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
                RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x)).When(x => x.Name != null);
                RuleFor(x => x.BrandingName).Must(x => !string.IsNullOrWhiteSpace(x)).When(x => x.BrandingName != null);
                RuleFor(x => x.HP).GreaterThan(0).When(x => x.HP != null);
                RuleFor(x => x.Price).GreaterThan(0).When(x => x.Price != null);
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<MotorcycleResponse>>
        {
            private readonly MyContext _context;
            private readonly IValidator<Command> _validator;

            public Handler(MyContext context, IValidator<Command> validator)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            }

            public async Task<Result<MotorcycleResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<MotorcycleResponse>(new Error(HttpStatusCode.BadRequest, ErrorMessages.Validation(nameof(UpdateMotorcycle)), validationResult.ToString()));
                }

                var motorcycle = await _context.Set<Motorcycle>()
                    .AsNoTracking()
                    .Where(x => x.Id.Equals(request.Id) && !x.Deleted)
                    .FirstOrDefaultAsync(cancellationToken);

                if (motorcycle == null)
                {
                    return Result.Failure<MotorcycleResponse>(new Error(HttpStatusCode.NotFound, ErrorMessages.NotFound(nameof(UpdateMotorcycle)), "The Motorcycle was not found"));
                }

                motorcycle.Name = request.Name ?? motorcycle.Name;
                motorcycle.BrandingName = request.BrandingName ?? motorcycle.BrandingName;
                motorcycle.HP = request.HP ?? motorcycle.HP;
                motorcycle.Price = request.Price ?? motorcycle.Price;

                _context.Set<Motorcycle>().Update(motorcycle);
                await _context.SaveChangesAsync(cancellationToken);

                return new MotorcycleResponse(motorcycle);
            }
        }
    }

    public class UpdateMotorcycleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/motorcycle", async (UpdateMotorcycleRequest request, ISender sender) =>
            {
                var command = new Command(request.Id, request.Name, request.BrandingName, request.HP, request.Price);

                var result = await sender.Send(command);
                return EndpointHelper.GetReponse(result);
            });
        }
    }
}
