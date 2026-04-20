using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

[Route("[controller]")] // Define a rota para esse controller
[ApiController] // Informa que este é um controller de uma API.
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUserJson request, 
        [FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);
        
        return Created(string.Empty, result);
    }
}