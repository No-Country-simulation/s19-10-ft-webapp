import ChatMessage from "./ChatMessage";
import ChatInput from "./ChatInput";
import { useState, useEffect, useRef } from "react";

interface Message {
  id: number;
  sender: "user" | "ai";
  text: string;
}

const ChatWindow: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([
    { id: 1, sender: "ai", text: "Hello, how can I assist you?" },
  ]);

  const messagesEndRef = useRef<HTMLDivElement>(null);

  const handleSendMessage = (sender: "user" | "ai", text: string) => {
    setMessages((prevMessages) => [
      ...prevMessages,
      { id: prevMessages.length + 1, sender, text },
    ]);
  };

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  return (
    <div className="flex flex-col h-full">
      {/* Contenedor de mensajes */}
      <div className="flex-1 overflow-y-auto p-4 space-y-4 bg-gray-100">
        {messages.map((msg) => (
          <ChatMessage key={msg.id} sender={msg.sender} text={msg.text} />
        ))}
        {/* Punto de referencia para desplazar */}
        <div ref={messagesEndRef} />
      </div>
      {/* Entrada de mensaje */}
      <ChatInput onSendMessage={handleSendMessage} />
    </div>
  );
};

export default ChatWindow;
