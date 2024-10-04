namespace Play.Catalog.Services;

public record ItemDto(Guid Id, string name, string Description, decimal Price, DateTimeOffset CreatedDate);

