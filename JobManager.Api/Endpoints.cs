using JobManager.Framework.Infrastructure;

namespace JobManager.Api;

public static class Endpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        JobManagerModule.MapEndpoints(app);
    }
}
