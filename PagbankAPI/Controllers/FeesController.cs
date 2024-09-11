using Microsoft.AspNetCore.Mvc;

namespace PagbankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public FeesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateFees(
   )
        {
            // Valores fixos definidos diretamente no código
            string payment_methods = "PIX";
            decimal value = 10000;  // Valor em centavos (R$ 100,00)
            int max_installments = 10;
            int max_installments_no_interest = 4;
            string credit_card_bin = "552100";

            // URL da API com os valores fixos
            string url = $"https://sandbox.api.pagseguro.com/charges/fees/calculate?payment_methods={payment_methods}&value={value}&max_installments={max_installments}&max_installments_no_interest={max_installments_no_interest}&credit_card_bin={credit_card_bin}";

            // Configura o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer cd715d92-452c-4234-99e9-11ff0e95f7dbe8a3b7994bafa98b8f2b848592cc9bde8a56-e816-492d-a242-e4918ceecdb0");



            try
            {
                // Faz a requisição à API externa
                var response = await _httpClient.GetAsync(url);

                // Verifica se a resposta foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                else
                {
                    // Se não for bem-sucedido, retorna o código de status e a mensagem de erro
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new { Message = "Error from external API", Details = errorContent });
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { Message = "Request failed", Error = ex.Message });
            }
        }
    }
}
