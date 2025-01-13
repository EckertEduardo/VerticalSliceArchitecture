using System.Net;
using VerticalSliceArchitecture.Shared;

namespace VerticalSliceArchitecture.Helpers
{
    public static class EndpointHelper
    {
        public static IResult GetReponse<T>(Result<T> result)
        {
            if (result.IsFailure && result.Error.StatusCode == HttpStatusCode.BadRequest)
                return Results.BadRequest(result.Error);

            if (result.IsFailure && result.Error.StatusCode == HttpStatusCode.NotFound)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        }

        public static IResult GetReponse(Result result)
        {
            if (result.IsFailure && result.Error.StatusCode == HttpStatusCode.BadRequest)
                return Results.BadRequest(result.Error);

            if (result.IsFailure && result.Error.StatusCode == HttpStatusCode.NotFound)
                return Results.NotFound(result.Error);

            return Results.Ok();
        }
    }
}
