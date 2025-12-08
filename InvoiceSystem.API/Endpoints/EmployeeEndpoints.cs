using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Application.Validation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvoiceSystem.API.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees").WithTags("Employees");

        group.MapPost("/", async (EmployeeCreationDTO dto, IEmployeeService service) =>
        {

            var validate = AttributesValidationHelper.ValidateObjectManually(dto);
            if (validate.IsFailure)
                return Results.BadRequest(validate.Error);

            var result = await service.CreateEmployeeAsync(dto);
            return result.IsSuccess
            ? Results.Created($"api/employees/{result.Value.Id}", result.Value)
            : Results.BadRequest(result.Errors);
        }).WithSummary("Valid EmployeeRole: Clerk, FO, FM");

        group.MapGet("/", async (IEmployeeService service) =>
        {
            var results = await service.GetAllEmployeesAsync();
            return results.IsSuccess
            ? Results.Ok(results.Value)
            : Results.BadRequest(results.Errors);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEmployeeService service) =>
        {
            var result = await service.GetEmployeeByIdAsync(id);
            return result.IsSuccess
            ? Results.Ok(result.Value) 
            : Results.BadRequest(result.Errors);
        });

        group.MapPatch("/{id:guid}", async (Guid id, EmployeeUpdateDTO dto, IEmployeeService service)=>
        {
            var validate = AttributesValidationHelper.ValidateObjectManually(dto);
            if (validate.IsFailure)
                return Results.BadRequest(validate.Error);

            var result = await service.UpdateEmployeeAsync(id, dto);
            return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
        });


    }
}
