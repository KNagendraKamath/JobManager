namespace JobManager.Api.JobSetup;

public static class JobEndPoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateJob.MapEndpoint(app);
    }
}
