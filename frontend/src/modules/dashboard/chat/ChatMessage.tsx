interface ChatMessageProps {
    sender: "user" | "ai";
    text: string;
  }
  
  const ChatMessage: React.FC<ChatMessageProps> = ({ sender, text }) => {
    const isUser = sender === "user";
  
    return (
      <div
        className={`flex ${isUser ? "justify-end" : "justify-start"} items-center`}
      >
        <div
          className={`max-w-xs p-3 rounded-lg text-sm ${
            isUser
              ? "bg-blue-500 text-white self-end"
              : "bg-gray-200 text-gray-800 self-start"
          }`}
        >
          {text}
        </div>
      </div>
    );
  };
  
  export default ChatMessage;
  