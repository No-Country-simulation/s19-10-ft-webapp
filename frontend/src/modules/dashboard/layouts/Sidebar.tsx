import React, { useState } from "react";

interface Document {
  id: number;
  name: string;
  size: string; // Ejemplo: "2 MB"
}

const Sidebar: React.FC = () => {
  const [documents, setDocuments] = useState<Document[]>([]);
  const [dragging, setDragging] = useState(false);

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(false);

    const files = Array.from(event.dataTransfer.files);
    const newDocuments = files.map((file, index) => ({
      id: documents.length + index + 1,
      name: file.name,
      size: `${(file.size / (1024 * 1024)).toFixed(2)} MB`,
    }));

    setDocuments((prevDocuments) => [...prevDocuments, ...newDocuments]);
  };

  const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = () => setDragging(false);

  return (
    <div className="w-64 h-full bg-gray-100 border-r border-gray-300 flex flex-col">
      {/* Zona de arrastrar y soltar */}
      <div
        className={`flex-1 p-4 flex items-center justify-center border-dashed border-2 ${
          dragging ? "border-blue-500 bg-blue-50" : "border-gray-300"
        }`}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
      >
        <p className="text-gray-600 text-center">
          {dragging ? "Suelta para cargar" : "Arrastra documentos aquí"}
        </p>
      </div>

      {/* Lista de documentos */}
      <div className="p-4 overflow-y-auto">
        <h3 className="font-semibold text-gray-700 mb-2">Documentos cargados</h3>
        {documents.length === 0 ? (
          <p className="text-gray-500 text-sm">No se han cargado documentos.</p>
        ) : (
          <ul className="space-y-2">
            {documents.map((doc) => (
              <li
                key={doc.id}
                className="flex items-center justify-between p-2 bg-white rounded-md shadow"
              >
                <div>
                  <p className="font-medium text-gray-700">{doc.name}</p>
                  <p className="text-xs text-gray-500">{doc.size}</p>
                </div>
                <button
                  onClick={() =>
                    setDocuments((prev) => prev.filter((d) => d.id !== doc.id))
                  }
                  className="text-red-500 text-sm hover:underline"
                >
                  Eliminar
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default Sidebar;