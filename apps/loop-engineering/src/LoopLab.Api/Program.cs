using LoopLab.Application;
using LoopLab.Application.Orders;
using LoopLab.Domain.Common;
using LoopLab.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

// Domain hatalarını HTTP durum kodlarına çevirir
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (OrderNotFoundException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (DomainException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

var orders = app.MapGroup("/orders");

orders.MapPost("/", async (CreateOrderRequest request, OrderService service) =>
{
    var id = await service.CreateAsync(request);
    return Results.Created($"/orders/{id}", new { id });
});

orders.MapPost("/{id:guid}/lines", async (Guid id, AddOrderLineRequest request, OrderService service) =>
{
    await service.AddLineAsync(id, request);
    return Results.NoContent();
});

orders.MapPost("/{id:guid}/coupon/{code}", async (Guid id, string code, OrderService service) =>
{
    await service.ApplyCouponAsync(id, code);
    return Results.NoContent();
});

orders.MapGet("/{id:guid}", async (Guid id, OrderService service) =>
    Results.Ok(await service.GetSummaryAsync(id)));

orders.MapPost("/{id:guid}/place", async (Guid id, OrderService service) =>
    Results.Ok(await service.PlaceAsync(id)));

orders.MapPost("/{id:guid}/cancel", async (Guid id, string reason, OrderService service) =>
{
    await service.CancelAsync(id, reason);
    return Results.NoContent();
});

app.Run();
