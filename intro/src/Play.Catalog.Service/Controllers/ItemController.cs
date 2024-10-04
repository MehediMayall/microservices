using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Services;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase {

    private static readonly List<ItemDto> Items = new(){
        new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Antidote", "Restores a small amount of HP", 7, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Bronze sword", "Restores a small amount of HP", 20, DateTimeOffset.UtcNow),
    };

    public ItemController()
    {

    }

}