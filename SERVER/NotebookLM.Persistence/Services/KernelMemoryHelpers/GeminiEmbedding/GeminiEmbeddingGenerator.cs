using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using System.Text;
using System.Text.Json;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GemmaTokenizer;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiEmbedding;

namespace NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiEmbedding
{
    public class GeminiEmbeddingGenerator : ITextEmbeddingGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly GemmaSentencePieceTokenizer _tokenizer;

        public GeminiEmbeddingGenerator(string apiKey, GemmaSentencePieceTokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public int MaxTokens => 4096; // Configurado según las especificaciones del modelo

        public int CountTokens(string text)
        {
            return _tokenizer.CountTokens(text);
        }

        public IReadOnlyList<string> GetTokens(string text)
        {
            //  Console.WriteLine("Getting tokens from text: " + text);
            return _tokenizer.GetTokens(text);
        }

        public async Task<Embedding> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            // Preparar el payload con los parámetros requeridos, ajustando la estructura de acuerdo con el formato JSON que mencionaste.
            int count = CountTokens(text);
            if (count > MaxTokens)
            {
                Console.WriteLine("El texto excede el límite de tokens permitido por el modelo.");

            }
            var requestPayload = new
            {
                content = new
                {
                    parts = new[] { new { text } }  // Ajustar el formato para que se vea como "parts" con "text"
                },
                taskType = "SEMANTIC_SIMILARITY"  // Especificar el tipo de tarea
            };


            // Serializar el payload a JSON
            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                "application/json"
            );


            // Establecer el endpoint de la API de Gemini (ajusta el endpoint si es necesario)
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/text-embedding-004:embedContent?key={_apiKey}";




            // Realizar la solicitud HTTP POST
            var response = await _httpClient.PostAsync(endpoint, requestContent, cancellationToken);
            Console.WriteLine("Sending Embedding Packs");

            try
            {
                // Asegurarse de que la solicitud fue exitosa
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("From Embedding code: " + e.Message);
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error content: " + errorContent);
            }

            // Leer la respuesta de la API
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserializar la respuesta. Ajusta el formato según la respuesta real de la API.
            var embeddingData = JsonSerializer.Deserialize<GoogleEmbeddingResponse>(responseContent);

            if (embeddingData == null)
            {
                throw new InvalidOperationException("No se pudo obtener el embedding del texto.");
            }
            // Retornar el objeto Embedding con los datos obtenidos
            return new Embedding(embeddingData.Embedding.Values.ToArray());
        }
    }

}
