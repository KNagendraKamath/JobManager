using JobManager.Api.JobSetup;

namespace JobManager.Api;

public static class Endpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        JobEndPoints.MapEndpoints(app);
    }
}
