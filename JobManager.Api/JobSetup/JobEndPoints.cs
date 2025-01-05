namespace JobManager.Api.JobSetup;

public static class JobEndPoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        ScheduleJob.MapEndpoint(app);
        UnScheduleJob.MapEndpoint(app);
    }
}
