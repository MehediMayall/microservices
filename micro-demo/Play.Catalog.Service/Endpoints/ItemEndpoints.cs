namespace Play.Catalog.Service;

public static class ItemEndpoints
{
    public static void AddItemEndpoints(this IEndpointRouteBuilder routes){
        var itemGroup = routes.MapGroup("/items");

        itemGroup.MapGet("/", async () => {
            return Results.Ok("Hello");
        });

    }
}