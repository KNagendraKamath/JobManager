using JobManager.Framework.Presentation.Api.JobSetup;

namespace JobManager.Framework.Presentation.Api;

public static class JobManagerApiEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        JobEndPoints.MapEndpoints(app);
    }
}
