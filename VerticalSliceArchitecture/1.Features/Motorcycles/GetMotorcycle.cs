using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VerticalSliceArchitecture._4.Shared;
using VerticalSliceArchitecture.Contracts;
using VerticalSliceArchitecture.DataBase;
using VerticalSliceArchitecture.Entities;
using VerticalSliceArchitecture.Helpers;
using VerticalSliceArchitecture.Shared;

namespace VerticalSliceArchitecture.Features.Motorcycles
{
    public static class GetMotorcycle
    {
        public class Query : IRequest<Result<MotorcycleResponse>>
        {
            public Guid Id { get; set; }
            public Query(Guid id) => Id = id;
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().NotNull();
            }
        }

        internal sealed class Handler : IRequestHandler<Query, Result<MotorcycleResponse>>
        {
            private readonly MyContext _context;
            private readonly IValidator<Query> _validator;

            public Handler(MyContext context, IValidator<Query> validator)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            }

            public async Task<Result<MotorcycleResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<MotorcycleResponse>(new Error(HttpStatusCode.BadRequest, ErrorMessages.Validation(nameof(GetMotorcycle)), validationResult.ToString()));
                }

                var motorcycleResponse = await _context.Set<Motorcycle>()
                    .AsNoTracking()
                    .Where(x => x.Id.Equals(request.Id) && !x.Deleted)
                    .Select(x => new MotorcycleResponse(x))
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (motorcycleResponse == null)
                {
                    return Result.Failure<MotorcycleResponse>(new Error(HttpStatusCode.NotFound, ErrorMessages.NotFound(nameof(GetMotorcycle)), "The Motorcycle was not found"));
                }

                return motorcycleResponse;
            }
        }
    }

    public class GetMotorcylceEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/motorcycle/{id}", async (Guid id, ISender sender) =>
            {
                var request = new GetMotorcycle.Query(id);

                var result = await sender.Send(request);
                return EndpointHelper.GetReponse(result);
            });
        }
    }
}
