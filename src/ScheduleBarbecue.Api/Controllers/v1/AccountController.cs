using Microsoft.AspNetCore.Mvc;
using ScheduleBarbecue.Application.Base.Notifications;
using ScheduleBarbecue.Application.Features.Users;
using ScheduleBarbecue.Application.Features.Users.Requests;
using ScheduleBarbecue.Application.Features.Users.Services.Contracts;

namespace ScheduleBarbecue.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(
            IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Realiza o cadastro de um usuário para fator de autenticação
        /// </summary>
        /// <returns>Retorno padrão que contendo os dados do usuário cadastrado</returns>
        /// <response code="201">Retorno padrão com dados</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpPost(Name = "CreateAccount")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> CreateAccount([FromBody] CreateUserRequest request)
        {
            var (response, createduser) = await _userService.CreateUser(request);

            if (!response.IsValid() || createduser is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return Created($"/{createduser.Id}", createduser);
        }

        /// <summary>
        /// Realiza o login do usuário
        /// </summary>
        /// <returns>Retorno padrão contendo o token de autenticação gerado</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="404">Retorno padrão informando que não localizou o(s) dado(s)</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        [HttpPost, Route("login", Name = "Login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Notification?>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult?> Login([FromBody] UserLoginRequest request)
        {
            var (response, token) = await _userService.GetToken(request);

            if (!response.IsValid() && token == string.Empty)
                return NotFound(new Notification(StatusCodes.Status404NotFound.ToString(), "Usuário não localizado."));

            if (!response.IsValid() && token is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return Ok(token);
        }
    }
}