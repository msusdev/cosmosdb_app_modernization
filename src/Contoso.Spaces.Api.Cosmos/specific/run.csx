using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

public static IActionResult Run(HttpRequest req, dynamic doc) 
    => doc is null ? new NotFoundResult() : new OkObjectResult(doc) as IActionResult;