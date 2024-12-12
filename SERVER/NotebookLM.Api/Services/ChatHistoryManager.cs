using NotebookLM.Domain.Entities;

namespace NotebookLM.Api.Services
{
    public class ChatHistoryManager
    {
        private readonly List<ChatHistory> _chatHistories = new List<ChatHistory>();

        public ChatHistory? GetChatHistory(int chatHistoryId)
        {
            return _chatHistories.FirstOrDefault(ch => ch.Id == chatHistoryId);
        }

        public void AddChatHistory(ChatHistory chatHistory)
        {
            if (!_chatHistories.Any(ch => ch.Id == chatHistory.Id))
            {
                _chatHistories.Add(chatHistory);
            }
        }
    }

}
