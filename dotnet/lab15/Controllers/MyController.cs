using Microsoft.AspNetCore.Mvc;

namespace lab15.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayUntilController
    {
        [HttpGet]
        public IResult Get(DateTime date)
        {
            var today = DateTime.UtcNow.Date;
            var targetDate = date.Date;
            int daysLeft = (targetDate - today).Days;

            if (daysLeft < 0)
                return Results.BadRequest("Указанная дата уже прошла.");

            return Results.Ok(new { daysLeft });
        }
    }
}
