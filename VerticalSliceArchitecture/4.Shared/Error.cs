using System.Net;
using System.Text.Json.Serialization;

namespace VerticalSliceArchitecture.Shared
{
    public class Error(HttpStatusCode statusCode, string code, string message)
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;

        public static readonly Error None = new(HttpStatusCode.OK, string.Empty, string.Empty);
        public static readonly Error NullValue = new(HttpStatusCode.BadRequest, "Error.NullValue", "The specified result value is null.");
    }
}
