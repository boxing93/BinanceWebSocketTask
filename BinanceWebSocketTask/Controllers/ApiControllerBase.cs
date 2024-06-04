using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BinanceWebSocketTask.API.Controllers;

[ApiController]
[Route("api/")]
public class ApiControllerBase : Controller
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}

