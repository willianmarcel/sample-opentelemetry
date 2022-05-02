using Microsoft.AspNetCore.Mvc;
using Sample.OpenTelemetry.Api.Model;
using System.Text.Json;

namespace Sample.OpenTelemetry.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogger<LogsController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public LogsController(ILogger<LogsController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("thread")]
    public IActionResult GetThreadAsync()
    {
        Task.Delay(3000);
        return Ok();
    }

    [HttpGet("cidades")]
    public async Task<IEnumerable<CidadeViewModel>?> GetCitiesAsync()
    {
        IList<CidadeViewModel>? cidades = null;
        var client = _httpClientFactory.CreateClient("google");
        var response = await client.GetAsync("https://servicodados.ibge.gov.br/api/v1/localidades/estados/RJ/municipios");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            cidades = JsonSerializer.Deserialize<IList<CidadeViewModel>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return cidades;
    }

    [HttpGet("exception")]
    public IActionResult GetException()
    {
        throw new ArgumentException("Erro");
    }
}
