using JobManager.Framework.Application.JobSetup.CreateJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Presentation.Api.ApiResults;
using MediatR;

namespace JobManager.Framework.Presentation.Api.JobSetup;

internal static class UnScheduleJob
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("UnScheduleJob", async (UnScheduleJobRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new UnscheduleJobCommand(request.JobId, request.JobName));

            if (result!.IsFailure)
                return ApiStatus.Failure(result);

            return Results.Ok(result);
        });
    }
}

internal sealed record UnScheduleJobRequest(long JobId, string? JobName);
