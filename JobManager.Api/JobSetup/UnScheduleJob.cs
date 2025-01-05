using JobManager.Api.ApiResults;
using JobManager.Application.JobSetup.CreateJob;
using JobManager.Domain.Abstractions;
using MediatR;

namespace JobManager.Api.JobSetup;

internal static class UnScheduleJob
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("UnScheduleJob", async (UnScheduleJobRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new UnscheduleJobCommand(request.JobId,request.JobName));

            if (result!.IsFailure)
                return ApiStatus.Failure(result);

            return Results.Ok(result);
        });
    }
}

public sealed record UnScheduleJobRequest(long JobId, string? JobName);

