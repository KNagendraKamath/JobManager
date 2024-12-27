using System.Text.Json.Serialization;
using JobManager.Api.ApiResults;
using JobManager.Application.JobSetup.CreateJob;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using MediatR;

namespace JobManager.Api.JobSetup;

internal static class CreateJob
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("job", async (Request request, ISender sender) =>
        {
            Result<long> result = await sender.Send(new CreateJobCommand(request.Description,
                                                                         request.EffectiveDateTime,
                                                                         request.JobType,
                                                                         request.RecurringType,
                                                                         request.JobSteps,
                                                                         request.CreatedById));

            if (result!.IsFailure) return ApiStatus.Failure(result);

            return Results.Ok(result.Value);

        });
    }

    internal class Request
    {
        public string? Description { get; set; }
        public DateTime EffectiveDateTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JobType JobType { get; set; }=JobType.None;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecurringType RecurringType { get; set; }=RecurringType.None;

        public List<Step> JobSteps { get; set; }
        public long CreatedById { get; set; }
    }


}
