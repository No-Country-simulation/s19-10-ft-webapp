using Microsoft.KernelMemory;
using NotebookLM.Persistence.Services.KernelMemoryHelpers;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiEmbedding;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiTextGenerator;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GemmaTokenizer;
using Microsoft.KernelMemory.MemoryDb.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotebookLM.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NotebookLM.Domain.Entities;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;

namespace NotebookLM.Persistence.Services
{
    public class KernelMemoryService
    {
        static public string GOOGLE_API_KEY = "AIzaSyDgsQq50eykWz1662D6Xor0-4ANLhldxSw";

        public IKernelMemory _connector;

        private readonly IConfiguration _configuration;


        public KernelMemoryService(IConfiguration configuration)
        {
            _connector = GetMemoryConnector(GOOGLE_API_KEY);
            _configuration = configuration;
        }

     
        
       public async Task<string> GetAnswerFromFile(string fileId, string userId, string prompt)
        {
            try
            {
                // Crear filtros de seguridad
                var filter = new MemoryFilter()
                      .ByTag("fileId", fileId)    // Correct tag filtering
                      .ByTag("userId", userId);

                var searchResult = await _connector.SearchAsync(
                    query: prompt,
                    filters: new List<MemoryFilter> { filter },
                    minRelevance: 0,
                    limit: 1
                );

                // Verificar acceso
                if (searchResult.Results.Count == 0)
                {
                    throw new UnauthorizedAccessException(
                        "No tienes acceso al documento o el documento no existe"
                    );
                }

                return searchResult.Results[0].Partitions[0].Text;

            }
            catch (Exception ex)
            {
                // Manejo de errores
                // Logging de intento de acceso
                Console.WriteLine($"Error accessing document {fileId} for user {userId}: {ex.Message}");
                return "";
            }

        


    }

    public async Task ImportEmbeddingsToDB(string fName, Guid fileId, int chatHistoryId, FileStream file, Guid userId)
        {

            // Obtenemos el Archivo
            // Importar embeddings a la base de datos

            try
            {
                TagCollection _tags = new();

                _tags.Add("userId", userId.ToString()); // Change for IdentityId

                _tags.Add("fileId", fileId.ToString());

                _tags.Add("chatHistoryId", chatHistoryId.ToString());

                await _connector.ImportDocumentAsync(
                        fileName: fName,
                        content: file,
                        tags: _tags

                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
                


        }

        public async Task DeleteFileFromMemories(Guid fileId, Guid userId)
        {
            // Eliminar documento de la memoria
            await _connector.DeleteDocumentAsync(fileId.ToString());
        }


        private static async Task<string> GetLongTermMemory(IKernelMemory memory, string query, bool asChunks)
        {
            if (asChunks)
            {
                // Fetch raw chunks, using KM indexes. More tokens to process with the chat history, but only one LLM request.
                SearchResult memories = await memory.SearchAsync(query, limit: 10);
                return memories.Results.SelectMany(m => m.Partitions).Aggregate("", (sum, chunk) => sum + chunk.Text + "\n").Trim();
            }

            // Use KM to generate an answer. Fewer tokens, but one extra LLM request.
            MemoryAnswer answer = await memory.AskAsync(query);
            return answer.Result.Trim();
        }



        private IKernelMemory GetMemoryConnector(string apiKey)
{
            // Obtener la cadena de conexión desde la configuración
            // Look for the name in the connectionStrings section.
          

        var DataStorage = new SqlServerConfig
        {
            ConnectionString = "Server=tcp:fintechapiserver.database.windows.net,1433;Initial Catalog=NotebookDB;Persist Security Info=False;User ID=Razorxxid;Password=dUG433sbd:jbvJz;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        };

        return new KernelMemoryBuilder()
                    .WithCustomEmbeddingGenerator(new GeminiEmbeddingGenerator(apiKey, new GemmaSentencePieceTokenizer()))
                    .WithCustomTextGenerator(new GeminiCustomModelTextGeneration(apiKey, new GemmaSentencePieceTokenizer()))
                    .WithContentDecoder<CustomPdfDecoder>() // Register a custom PDF decoder
                    .WithSqlServerMemoryDb(DataStorage)
                    .Build<MemoryServerless>();
        }




    }


   
}


