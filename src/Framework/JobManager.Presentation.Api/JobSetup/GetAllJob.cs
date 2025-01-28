using JobManager.Framework.Application.JobSetup.GetAllJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Presentation.Api.ApiResults;
using MediatR;

namespace JobManager.Framework.Presentation.Api.JobSetup;

internal static class GetAllJob
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("GetAllJob", async (int Page,int PageSize, ISender sender) =>
        {
            Result result = await sender.Send(new GetAllJobQuery(Page, PageSize));
            return result!.IsFailure? ApiStatus.Failure(result): Results.Ok(result);
        });
    } 
}
