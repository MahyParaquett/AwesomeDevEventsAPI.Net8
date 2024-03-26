using AwesomeDevEventsAPI.Etities;
using AwesomeDevEventsAPI.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEventsAPI.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEvantsController : ControllerBase
    {
        private readonly DevEventsDbContext _dbContext;
        public DevEvantsController(DevEventsDbContext context)
        {
            _dbContext = context; 
        }

        [HttpGet]
        public IActionResult GetAll() 
        {//retorne todos os objetos que não estão deletados
            var devEvents = _dbContext.DevEvents.Where(x => !x.IsDeleted).ToList();
            return Ok(devEvents);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _dbContext.DevEvents.Include(de => de.Speakers).SingleOrDefault(x => x.Id == id);
            if (devEvent == null)
            {
                return NotFound("Id não encontrado");
            }

            return Ok(devEvent);
        }

        [HttpPost]
        public IActionResult Post(DevEvent devEvent)
        {
            _dbContext.DevEvents.Add(devEvent);

            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);

        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _dbContext.DevEvents.SingleOrDefault(x => x.Id == id);
            if (devEvent == null)
            {
                return NotFound("Id não encontrado");
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _dbContext.DevEvents.Update(devEvent);
            _dbContext.SaveChanges();

            return Ok(devEvent);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _dbContext.DevEvents.SingleOrDefault(x => x.Id == id);
            if (devEvent == null)
            {
                return NotFound("Id não encontrado");
            }

            devEvent.Delete();

            _dbContext.SaveChanges();

            return Ok(devEvent);
        }

        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;
            
            var devEvent = _dbContext.DevEvents.Any(x => x.Id == id);

            if(!devEvent)
            {
                return NotFound();
            }

            _dbContext.DevEventSpeakers.Add(speaker);  
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
