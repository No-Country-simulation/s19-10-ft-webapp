using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Features;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Domain.Entities;
using NotebookLM.Persistence.Services;

namespace NotebookLM.Api.Services
{
    public class SemanticKernelChatService
    {
        private readonly IKernelBuilder builder;

        private readonly Kernel kernel;

        private readonly IGenericRepository<Domain.Entities.ChatHistory> _chatHistoryRepository;

        private readonly ChatHistoryManager _historyManager; // Singleton para manejar ChatHistory en memoria

        private readonly KernelMemoryService _memoryService;


        public SemanticKernelChatService(IGenericRepository<Domain.Entities.ChatHistory> chatHistoryRepository, 
            ChatHistoryManager historyManager, KernelMemoryService memoryService)
        {
            _historyManager = historyManager;

            _chatHistoryRepository = chatHistoryRepository;

            _memoryService = memoryService;
            var GOOGLE_API_KEY = "AIzaSyDgsQq50eykWz1662D6Xor0-4ANLhldxSw";
            var GeminiTextGeneratorModelId = "gemini-1.5-flash";

            #pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            builder = Kernel.CreateBuilder()
                           .AddGoogleAIGeminiChatCompletion(GeminiTextGeneratorModelId, GOOGLE_API_KEY

                           );
             kernel = builder.Build();

          
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        

          
        }

        public async Task<int> GetNewChatHistoryId()
        {
            var chatHistory = new Domain.Entities.ChatHistory();
            await _chatHistoryRepository.AddAsync(chatHistory);
            await _chatHistoryRepository.SaveChangesAsync();

            return chatHistory.Id;
        }

        public async Task<string> AskFromFile(string userId, int chatHistoryId, string userPrompt, string fileId)
        {



            // Configuracion de la LLM para el Chat.
            var systemPrompt = """
                           Eres un asistente útil que responde a las preguntas del usuario utilizando información de tu memoria.
                           Responde de forma muy breve y concisa (a no ser que el usuario requiera un poco mas de contenido para expandir la preunta)
                           , y ve directo al punto. No des explicaciones largas a menos que sea necesario.
                           A veces no tienes recuerdos relevantes, en ese caso, responde diciendo que no sabes o que no tienes la información.
                            Solamente puedes responder cosas de los documentos que tienes cargado. Si el usuario pide titulos y sub titulos tambien puedes 

                           """
            ;

            var history = new Microsoft.SemanticKernel.ChatCompletion.ChatHistory(systemPrompt);
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Intentar recuperar ChatHistory de memoria
            var chatHistory = _historyManager.GetChatHistory(chatHistoryId);

            if (chatHistory == null)
            {
                // Si no está en memoria, buscar en base de datos
                chatHistory = await _chatHistoryRepository.GetByIdAsync(chatHistoryId) ;
                if (chatHistory == null)
                {
                    // Si no está en base de dato y no está en memoria, retornar error
                    throw new Exception("ChatHistory no encontrado ni en memoria ni en DB (get a DB == NULL)");

                }
                _historyManager.AddChatHistory(chatHistory);
             
            }
            else
            {
                // Si está en memoria, actualizar el sistema de memoria
                FetchChatFromMemory(_historyManager, history, chatHistoryId);
            }

            if (string.IsNullOrWhiteSpace(userPrompt)) { return "No has dicho nada o algo salio mal, userPrompt vacio o nulo"; }
            else { history.AddUserMessage(userPrompt); }

            var longTermMemory = await _memoryService.GetAnswerFromFile(fileId, userId, userPrompt);

            // Add user input
            history[0].Content = $"{systemPrompt}\n\nLong term memory:\n{longTermMemory}";


#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            GeminiPromptExecutionSettings settings = new()
            {
                Temperature = 0.8,
                MaxTokens = 8192,
                //  ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
            };
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                                 // Get the response from the AI
            try
            {
                var result = await chatCompletionService.GetChatMessageContentAsync(
                 history,
                 executionSettings: settings,
                 kernel: kernel);



                // Add the message from the agent to the chat history

                history.AddMessage(result.Role, result.Content ?? string.Empty);

                return result.ToString();


            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<List<Domain.Entities.ChatHistory>> GetChatsOfUser(Guid userId)
        {
            var chatHistories = await _chatHistoryRepository.GetAllAsync();

            return chatHistories.ToList();
        }

        private void FetchChatFromMemory(ChatHistoryManager manager, Microsoft.SemanticKernel.ChatCompletion.ChatHistory chatSK, int chatHistoryId)
        {
            var chatHistory = manager.GetChatHistory(chatHistoryId);

            if (chatHistory == null)
            {
                foreach (Message m in chatHistory.Messages)
                {
                    if(chatHistory is null)
                    {
                        return;
                    }

                    if (m.Author == AuthorRole.User.ToString())
                    {
                        chatSK.AddMessage(AuthorRole.User, m.Content);
                    }
                    else if(m.Author == AuthorRole.System.ToString())
                    {
                        chatSK.AddMessage(AuthorRole.System, m.Content);
                    }
                    else if (m.Author == AuthorRole.Assistant.ToString())
                    {
                        chatSK.AddMessage(AuthorRole.Assistant, m.Content);
                    }
                   
                }
            }


        }



    }
}
