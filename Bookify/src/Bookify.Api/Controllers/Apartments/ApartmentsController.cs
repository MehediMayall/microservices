using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api;

[ApiController]
[Route("api/apartments")]
public class AparmentsController : ControllerBase {

    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default){
        
        var query = new SearchApartmentsQuery(startDate, endDate);
    }

}