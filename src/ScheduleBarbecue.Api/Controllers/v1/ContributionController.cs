using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleBarbecue.Application.Base.Notifications;
using ScheduleBarbecue.Application.Features.Contributions.Requests;
using ScheduleBarbecue.Application.Features.Contributions.Responses;
using ScheduleBarbecue.Application.Features.Contributions.Services.Contracts;
using ScheduleBarbecue.Application.Features.Participants.Responses;

namespace ScheduleBarbecue.Api.Controllers.v1
{
    [ApiController, Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/contribution")]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _contributionService;

        public ContributionController(IContributionService contributionService)
        {
            _contributionService = contributionService;
        }

        /// <summary>
        /// Realiza o cadastro de uma nova contribuição
        /// </summary>
        /// <returns>Retorna a contribuição cadastrada</returns>
        /// <response code="201">Retorno padrão com dados</response>
        /// <response code="400">Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpPost(Name = "CreateContribution")]
        [ProducesResponseType(typeof(ContributionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> CreateContribution(CreateContributionRequest request)
        {
            var (response, createdContribution) = await _contributionService.CreateContribution(request);

            if (!response.IsValid())
                return createdContribution is null ?
                    UnprocessableEntity(response.ToValidationErrors()) :
                    BadRequest(response.ToValidationErrors());

            return Created($"/{createdContribution.Id}", createdContribution);
        }

        /// <summary>
        /// Realiza a exclusão de uma contribuição tendo o identificador como parâmetro
        /// </summary>
        /// <response code="204">Retorno padrão sem dados</response>
        /// <response code="400">Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpDelete("{id}", Name = "RemoveContribution")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> RemoveContribution(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id da contribuição inválido.");

            var response = await _contributionService.RemoveContribution(id);

            if (!response.IsValid())
                return UnprocessableEntity(response.ToValidationErrors());

            return NoContent();
        }

        /// <summary>
        /// Realiza a obtenção de todos os participantes cadastrados de acordo com o churrasco informado
        /// </summary>
        /// <returns>Retorno padrão que contendo todos os participantes</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        [HttpGet, Route("getAllParticipantByBarbecueId/{id}", Name = "GetAllParticipantByBarbecueId")]
        [ProducesResponseType(typeof(List<ParticipantContributionResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetAllParticipantByBarbecueId(Guid id)
        {
            var (response, returnedParticipant) = await _contributionService.GetAllParticipantByBarbecueId(id);

            if (!response.IsValid() || !returnedParticipant.Any())
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Participantes não encontrados."));

            return Ok(returnedParticipant);
        }
    }
}