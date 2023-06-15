using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleBarbecue.Application.Base.Notifications;
using ScheduleBarbecue.Application.Features.Participants.Requests;
using ScheduleBarbecue.Application.Features.Participants.Responses;
using ScheduleBarbecue.Application.Features.Participants.Services.Contracts;

namespace ScheduleBarbecue.Api.Controllers.v1
{
    [ApiController, Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/participant")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Realiza o cadastro de um novo participante
        /// </summary>
        /// <returns>Retorna o participante cadastrado</returns>
        /// <response code="201">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpPost(Name = "CreateParticipant")]
        [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> CreateParticipant(CreateParticipantRequest request)
        {
            var (response, createdParticipant) = await _participantService.CreateParticipant(request);

            if (!response.IsValid() || createdParticipant is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return Created($"/{createdParticipant.Id}", createdParticipant);
        }

        /// <summary>
        /// Realiza a obtenção de todos os participantes cadastradas
        /// </summary>
        /// <returns>Retorno padrão que contém lista dos participantes</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        [HttpGet(Name = "GetAllParticipant")]
        [ProducesResponseType(typeof(List<ParticipantResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetAllParticipant()
        {
            var (response, listParticipant) = await _participantService.GetAllParticipant();

            if (!response.IsValid() || !listParticipant.Any())
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Lista de participantes não encontrada."));

            return Ok(listParticipant);
        }
    }
}