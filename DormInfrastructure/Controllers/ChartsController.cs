using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DormInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ChartsController : ControllerBase
    {
        private record CountByFacultyResponseItem(string Faculty, int Count);

        private record CountByTariffResponseItem(string Tariff, int Count);

        private readonly Do2Context _context;

        public ChartsController(Do2Context _context)
        {
            this._context = _context;
        }

        [HttpGet("countByFaculty")]
        public async Task<JsonResult> GetCountByFacultyAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.Students
            .GroupBy(student => student.Department.Faculty.Name) 
            .Select(group => new CountByFacultyResponseItem(group.Key.ToString(), group.Count()) 
            )
            .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);


        }
        [HttpGet("roomsByTariff")]
        public async Task<JsonResult> GetRoomsByTariffAsync(CancellationToken cancellationToken)
        {
          
            var responseItems = await _context.Rooms
                .GroupBy(room => room.Tariffs.TariffsName)
                .Select(group => new CountByTariffResponseItem(group.Key.ToString(),group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    }

}
