using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using System.Net.Mime;
using System.Threading.Tasks;

namespace InvoiceSystem.API.Endpoints;

public static class WorkflowstepEndpoints
{

    public static void MapWorkflowstepEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/workflowsteps").WithTags("Workflow Steps");
        group.MapPost("/", CreateWorkflowStep)
           .WithName("CreateWorkflowStep")
           .Produces<WorkflowstepsDetailsDTO>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
           .Produces<ErrorResponse>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
           .WithOpenApi();

        // --- GET: Get Workflow Step Details (A placeholder Query) ---
        group.MapGet("/{id:guid}", GetWorkflowstepDetails)
            .WithName("GetWorkflowStepDetails")
            .Produces<WorkflowstepsDetailsDTO>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

    }

    private static async Task<IResult> CreateWorkflowStep(WorkflowstepsCreationDTO dto, IWorkflowstepService service)
    {
        var result = await service.CreateWorkflowstepAsync(dto);
        if (result.IsSuccess) { return Results.Created($"/api/workflowsteps/{result.Value.Id}", result.Value); }

        return Results.BadRequest();

    }

    private static async Task<IResult> GetWorkflowstepDetails(Guid id, IWorkflowstepService service)
    {
        // not implemented yet. night not need it
        //var result=  await service.
        return Results.NotFound($"Workflow step with ID {id} not found (handler is a placeholder).");
    }

}
