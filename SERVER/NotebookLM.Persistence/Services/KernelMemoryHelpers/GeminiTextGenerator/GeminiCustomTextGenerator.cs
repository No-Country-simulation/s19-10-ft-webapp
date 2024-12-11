using NotebookLM.Persistence.Services.KernelMemoryHelpers.GemmaTokenizer;
using Microsoft.KernelMemory.AI;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;


namespace NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiTextGenerator
{
    public class GeminiCustomModelTextGeneration : ITextGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly GemmaSentencePieceTokenizer _tokenizer;
        public GeminiCustomModelTextGeneration(string apiKey, GemmaSentencePieceTokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public int MaxTokenTotal => 8192; // Configurado según las especificaciones del modelo

        public int CountTokens(string text)
        {
            return _tokenizer.CountTokens(text);

        }

        public IReadOnlyList<string> GetTokens(string text)
        {

            return _tokenizer.GetTokens(text);
        }


        public async IAsyncEnumerable<string> GenerateTextAsync(
            string prompt,
            TextGenerationOptions options,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // ... generate and return the text from the given prompt ...

            int count = CountTokens(prompt);
            if (count > MaxTokenTotal)
            {
                Console.WriteLine("El texto excede el límite de tokens permitido por el modelo.");

            }
            var requestPayload = new
            {
                contents = new
                {
                    parts = new[] { new { text = prompt } }
                },
            };


            // Serializar el payload a JSON
            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                "application/json"
            );

          //   Console.WriteLine("\n Request text content: " + requestPayload.contents.parts[0] + "\n");

            // Establecer el endpoint de la API de Gemini (ajusta el endpoint si es necesario)
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";



            // Realizar la solicitud HTTP POST
            var response = await _httpClient.PostAsync(endpoint, requestContent, cancellationToken);

            try
            {
                // Asegurarse de que la solicitud fue exitosa

                response.EnsureSuccessStatusCode();
                // Intentar obtener el mensaje de error desde el contenido de la respuesta
                var errorContent = await response.Content.ReadAsStringAsync();
              //  Console.WriteLine("Error content: " + errorContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            var responseContent = await response.Content.ReadAsStringAsync();


            // Leer la respuesta de la API
            var data = JsonSerializer.Deserialize<GeminiTextGeneratorResponse>(responseContent);



            yield return data.Candidates[0].Content.Parts[0].Text;
        }

    }
}
