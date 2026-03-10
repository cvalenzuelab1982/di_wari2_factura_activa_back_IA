using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using Directo.Wari.Application.Features.HttpUtil.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Directo.Wari.Infrastructure.Services.FacturaActiva
{
    public class HttpUtilRepository : IHttpUtilRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpUtilRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ConsultaDniResponseDto?> ConsultaDni(string dni)
        {
            var urlTemplate = _configuration["FacturacionActiva:URL_CONSULTA_DNI"];
            var url = urlTemplate!.Replace("@DNI", dni);

            var token = await RequestAccessTokenConsultaRucDni();

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("No se pudo obtener token");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
                return null;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ConsultaDniResponseDto>(body, options);
        }

        private async Task<string?> RequestAccessTokenConsultaRucDni()
        {
            var url = _configuration["FacturacionActiva:FACTURACION_ACTIVA_URL_TOKEN_CONSULTA_RUC_DNI"];
            var key = _configuration["FacturacionActiva:FACTURACION_ACTIVA_KEY_CONSULTA_RUC_DNI"];
            var secret = _configuration["FacturacionActiva:FACTURACION_ACTIVA_SECRET_CONSULTA_RUC_DNI"];

            var keySecret = $"{key}:{secret}";
            var base64KeySecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(keySecret));

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", base64KeySecret);

            request.Content = new StringContent(
                @"{ ""grant_type"": ""client_credentials"" }",
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var body = await response.Content.ReadAsStringAsync();

            var token = JsonSerializer.Deserialize<TokenResponse>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return token?.access_token;
        }
    }
}
