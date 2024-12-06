import ChatMessage from "./ChatMessage";
import ChatInput from "./ChatInput";
import { useState } from "react";

interface Message {
  id: number;
  sender: "user" | "ai";
  text: string;
}

const ChatWindow: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([
    { id: 1, sender: "ai", text: "Hello, how can I assist you?" },
    { id: 2, sender: "user", text: "Can you explain document 1 to me?" },
  ]);

  const handleSendMessage = (text: string) => {
    if (text.trim() !== "") {
      setMessages((prevMessages) => [
        ...prevMessages,
        { id: prevMessages.length + 1, sender: "user", text },
      ]);
    }
  };

  return (
    <div className="flex flex-col h-full bg-gray-100 border border-gray-300 rounded-md">
      <div className="flex-1 overflow-y-auto p-4 space-y-4">
        {messages.map((msg) => (
          <ChatMessage key={msg.id} sender={msg.sender} text={msg.text} />
        ))}
      </div>
      <ChatInput onSendMessage={handleSendMessage} />
    </div>
  );
};

export default ChatWindow;