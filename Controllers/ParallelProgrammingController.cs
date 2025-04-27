using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace ParallelProgramming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParallelProgrammingController : ControllerBase
    {
        [HttpPost("processa")]
        public IActionResult Processa([FromBody] MeuInput input)
        {
            // Dispara a tarefa em background com os parâmetros
            Task.Run(() => ChamarOutraApi(input.Id, input.Nome));

            return Ok();
        }

        // Seu método com parâmetros
        private async Task ChamarOutraApi(int id, string nome)
        {
            try
            {
                using var httpClient = new HttpClient();

                var payload = new
                {
                    Id = id,
                    Nome = nome
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://sua-outra-api.com/endpoint", content);

                var resposta = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Resposta da outra API: {resposta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar outra API: {ex.Message}");
            }
        }

        // Classe que representa os parâmetros recebidos no body
        public class MeuInput
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

    }
}
