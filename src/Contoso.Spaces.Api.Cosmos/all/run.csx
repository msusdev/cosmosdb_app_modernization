using Microsoft.AspNetCore.Mvc;
using System.Net;

public static IActionResult Run(HttpRequest req, IEnumerable<dynamic> docs) => new OkObjectResult(docs);