import { useState } from "react";

interface ChatInputProps {
  onSendMessage: (text: string) => void;
}

const ChatInput: React.FC<ChatInputProps> = ({ onSendMessage }) => {
  const [input, setInput] = useState<string>("");

  const handleSend = () => {
    if (input.trim() !== "") {
      onSendMessage(input);
      setInput("");
    }
  };

  return (
    <div className="flex items-center p-4 bg-white border-t border-gray-300">
      <input
        type="text"
        placeholder="Escribe tu mensaje..."
        value={input}
        onChange={(e) => setInput(e.target.value)}
        className="flex-1 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-300"
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
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
