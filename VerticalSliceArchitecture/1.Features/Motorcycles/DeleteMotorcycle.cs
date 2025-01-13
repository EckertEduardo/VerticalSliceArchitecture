using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VerticalSliceArchitecture._4.Shared;
using VerticalSliceArchitecture.DataBase;
using VerticalSliceArchitecture.Entities;
using VerticalSliceArchitecture.Helpers;
using VerticalSliceArchitecture.Shared;

namespace VerticalSliceArchitecture._1.Features.Motorcycles
{
    public static class DeleteMotorcycle
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; init; }
            public Command(Guid id)
            {
                Id = id;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result>
        {
            private readonly IValidator<Command> _validator;
            private readonly MyContext _context;

            public Handler(IValidator<Command> validator, MyContext context)
            {
                _validator = validator ?? throw new ArgumentNullException(nameof(validator));
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure(new Error(HttpStatusCode.BadRequest, ErrorMessages.Validation(nameof(DeleteMotorcycle)), validationResult.ToString()));
                }

                var item = await _context.Set<Motorcycle>()
                    .AsNoTracking()
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (item == null)
                    return Result.Success();

                item.Delete();

                _context.Set<Motorcycle>().Update(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }

            public class DeleteMotorcylceEndpoint : ICarterModule
            {
                public void AddRoutes(IEndpointRouteBuilder app)
                {
                    app.MapDelete("api/motorcycle/{id}", async (Guid id, ISender sender) =>
                    {
                        var request = new Command(id);

                        var result = await sender.Send(request);
                        return EndpointHelper.GetReponse(result);
                    });
                }
            }
        }
    }
}
