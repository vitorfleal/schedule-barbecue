using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleBarbecue.Application.Base.Notifications;
using ScheduleBarbecue.Application.Features.Barbecues.Requests;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;
using ScheduleBarbecue.Application.Features.Barbecues.Services.Contracts;

namespace ScheduleBarbecue.Api.Controllers.v1
{
    [ApiController, Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/barbecue")]
    public class BarbecueController : ControllerBase
    {
        private readonly IBarbecueService _barbecueService;

        public BarbecueController(IBarbecueService barbecueService)
        {
            _barbecueService = barbecueService;
        }

        /// <summary>
        /// Realiza o cadastro de um novo churrasco
        /// </summary>
        /// <returns>Retorna o churrasco cadastrado</returns>
        /// <response code="201">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpPost(Name = "CreateBarbecue")]
        [ProducesResponseType(typeof(BarbecueResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> CreateBarbecue(CreateBarbecueRequest request)
        {
            var (response, createdBarbecue) = await _barbecueService.CreateBarbecue(request);

            if (!response.IsValid() || createdBarbecue is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return Created($"/{createdBarbecue.Id}", createdBarbecue);
        }

        /// <summary>
        /// Realiza a obtenção de todos os churrascos cadastradas
        /// </summary>
        /// <returns>Retorno padrão que contém lista dos churrascos</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        [HttpGet(Name = "GetAllBarbecue")]
        [ProducesResponseType(typeof(List<BarbecueResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetAllBarbecue()
        {
            var (response, listBarbecue) = await _barbecueService.GetAllBarbecue();

            if (!response.IsValid() || !listBarbecue.Any())
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Lista de churrascos não encontrada."));

            return Ok(listBarbecue);
        }

        /// <summary>
        /// Realiza a obtenção do resumo de um churrasco cadastrado
        /// </summary>
        /// <returns>Retorno padrão que contendo o resumo do churrasco</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        [HttpGet, Route("getOneBarbecueCompendious/{id}", Name = "GetOneBarbecueCompendious")]
        [ProducesResponseType(typeof(List<BarbecueCompendiousResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetOneBarbecueCompendious(Guid id)
        {
            var (response, returnedBarbecue) = await _barbecueService.GetAllOrOneBarbecueCompendious(id);

            if (!response.IsValid() || !returnedBarbecue.Any())
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Resumo do churrasco não encontrado."));

            return Ok(returnedBarbecue);
        }

        /// <summary>
        /// Realiza a obtenção do resumo de todos os churrascos cadastrados
        /// </summary>
        /// <returns>Retorno padrão que contendo o resumo de todos os churrascos</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="401">Retorno padrão informando que não está autenticado para acessar o recurso de destino</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        [HttpGet, Route("getAllBarbecueCompendious", Name = "GetAllBarbecueCompendious")]
        [ProducesResponseType(typeof(List<BarbecueCompendiousResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetAllBarbecueCompendious()
        {
            var (response, returnedBarbecue) = await _barbecueService.GetAllOrOneBarbecueCompendious();

            if (!response.IsValid() || !returnedBarbecue.Any())
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Resumos dos churrascos não encontrados."));

            return Ok(returnedBarbecue);
        }
    }
}