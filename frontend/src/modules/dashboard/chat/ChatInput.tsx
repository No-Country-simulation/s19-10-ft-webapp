// components/ChatInput.tsx
import { useState } from "react";
import { sendPrompt } from "../actions";

interface ChatInputProps {
  onSendMessage: (sender: "user" | "ai", text: string) => void;
}

const ChatInput: React.FC<ChatInputProps> = ({ onSendMessage }) => {
  const [input, setInput] = useState<string>("");

  const handleSend = async () => {
    if (input.trim() === "") return;

    try {
      // Envía el mensaje al frontend para mostrarlo inmediatamente
      onSendMessage("user", input);
      setInput("");  // **Limpia el input después de enviar el mensaje**

      // Envía el mensaje al backend y espera la respuesta
      const response = await sendPrompt(input);

      if (response && response.text) {
        onSendMessage("ai", response.text);
      }
    } catch (error) {
      console.error("Failed to send message", error);
    }
  };

  return (
    <div className="flex items-center p-4 bg-white border-t border-gray-300">
      <input
        type="text"
        placeholder="Escribe tu mensaje..."
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
        onChange={(e) => setInput(e.target.value)}
        value={input}
        className="flex-1 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-300"
      />
      <button
        onClick={handleSend}
        className="ml-2 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600"
      >
        Enviar
      </button>
    </div>
  );
};

export default ChatInput;
