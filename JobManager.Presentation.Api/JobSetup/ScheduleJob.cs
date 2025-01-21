using System.Text.Json.Serialization;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;
using JobManager.Framework.Presentation.Api.ApiResults;
using MediatR;
using RecurringDetail = JobManager.Framework.Application.JobSetup.ScheduleJob.RecurringDetail;

namespace JobManager.Framework.Presentation.Api.JobSetup;

internal static class ScheduleJob
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("ScheduleJob", async (ScheduleJobRequest request, ISender sender) =>
        {
            Result<long> result = await sender.Send(new ScheduleJobCommand(request.Description,
                                                                         request.EffectiveDateTime,
                                                                         request.JobType,
                                                                         request.JobSteps,
                                                                         request.RecurringDetail));

            if (result!.IsFailure)
                return ApiStatus.Failure(result);

            return Results.Ok(result.Value);

        });
    }

    internal sealed record ScheduleJobRequest
    {
        public string? Description { get; set; }
        public DateTime EffectiveDateTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JobType JobType { get; set; }

        public RecurringDetail? RecurringDetail { get; set; }

        public List<Step> JobSteps { get; set; }
    }

}
