using JobManager.Framework.Presentation.Api;

namespace JobManager.Api;

internal static class Endpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        JobManagerApiEndpoint.MapEndpoints(app);
    }
}
