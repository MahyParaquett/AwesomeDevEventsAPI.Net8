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
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _dbContext;
        public DevEventsController(DevEventsDbContext context)
        {
            _dbContext = context; 
        }

        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code ="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll() 
        {//retorne todos os objetos que não estão deletados
            var devEvents = _dbContext.DevEvents.Include(de => de.Speakers).Where(x => !x.IsDeleted).ToList();
            return Ok(devEvents);
        }

        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _dbContext.DevEvents.Include(de => de.Speakers).SingleOrDefault(x => x.Id == id);
            if (devEvent == null)
            {
                return NotFound("Id não encontrado");
            }

            return Ok(devEvent);
        }

        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",  "title": "string",  "description": "string",  "startDate": "2024-03-27T16:16:45.049Z",  "endDate": "2024-03-27T16:16:45.049Z",
        ///"speakers": [{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "talkTitle": "string","talkDescription": "string",
        ///"linkedInProfile": "string","devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"}],"isDeleted": true}
        /// </remarks>
        /// <param name="devEvent">Dados do evento</param>
        /// <returns>Objeto recem criado</returns>
        /// <response code ="201">Criado</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEvent devEvent)
        {
            _dbContext.DevEvents.Add(devEvent);

            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);

        }

        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        /// {"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",  "title": "string",  "description": "string",  "startDate": "2024-03-27T16:16:45.049Z",  "endDate": "2024-03-27T16:16:45.049Z",
        ///"speakers": [{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "talkTitle": "string","talkDescription": "string",
        ///"linkedInProfile": "string","devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"}],"isDeleted": true}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="404">Não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">dentificador do evento</param>
        /// <returns>Nada</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="404">Não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Cadastrar palestrante
    /// </summary>
    /// <remarks>
    /// {"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "talkTitle": "string","talkDescription": "string","linkedInProfile": "string","devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"}
    /// </remarks>
    /// <param name="id">Identificador do evento</param>
    /// <param name="speaker">Dados do palestrante</param>
    /// <returns>Nada</returns>
    /// /// <response code ="204">Sucesso</response>
    /// <response code ="404">Não encontrado</response>
    [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
