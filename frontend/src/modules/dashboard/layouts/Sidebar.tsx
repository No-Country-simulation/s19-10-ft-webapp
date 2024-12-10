import React, { useState, useRef } from "react";
import { FaCircleArrowRight, FaCircleArrowLeft } from "react-icons/fa6";
interface Document {
  id: number;
  name: string;
  size: string; // Ejemplo: "2 MB"
}

const Sidebar: React.FC = () => {
  const [documents, setDocuments] = useState<Document[]>([]);
  const [dragging, setDragging] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(false);

    const files = Array.from(event.dataTransfer.files);
    processFiles(files);
  };

  const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = () => setDragging(false);

  const handleFileInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(event.target.files || []);
    processFiles(files);
  };

  const handleAreaClick = () => {
    fileInputRef.current?.click();
  };

  const processFiles = (files: File[]) => {
    const newDocuments = files.map((file, index) => ({
      id: Date.now() + index,
      name: file.name,
      size: `${(file.size / (1024 * 1024)).toFixed(2)} MB`,
    }));
    setDocuments((prevDocuments) => [...prevDocuments, ...newDocuments]);
  };

  return (
    <div
      className={`relative ${
        isOpen ? "w-14" : "w-64"
      } min-h-screen bg-gray-100 border-r border-gray-300 flex flex-col`}
    >
      {isOpen && (
        <>
          <FaCircleArrowRight
            onClick={() => setIsOpen((prev) => !prev)}
            className="text-2xl absolute -right-3 top-2 rounded hover:cursor-pointer"
          />
        </>
      )}
      {!isOpen && (
        <>
          <FaCircleArrowLeft
            onClick={() => setIsOpen((prev) => !prev)}
            className="text-2xl absolute -right-3 top-2 rounded hover:cursor-pointer"
          />
        </>
      )}
      {!isOpen && (
        <>
          <h2 className="font-semibold text-gray-700 m-2">Docs</h2>
          {/* Zona de arrastrar, soltar y clic */}
          <div
            className={`flex-1 p-4 flex items-center justify-center border-dashed border-2 ${
              dragging ? "border-blue-500 bg-blue-50" : "border-gray-300"
            }`}
            onDragOver={handleDragOver}
            onDragLeave={handleDragLeave}
            onDrop={handleDrop}
            onClick={handleAreaClick} // Activar input al hacer clic
          >
            <input
              type="file"
              ref={fileInputRef}
              multiple
              className="hidden" // Ocultar input
              onChange={handleFileInputChange}
            />
            <p className="text-gray-600 text-center">
              {dragging
                ? "Drop to upload"
                : "Drag documents here or click to upload"}
            </p>
          </div>

          {/* Lista de documentos */}
          <div className="p-4 overflow-y-auto">
            <h3 className="font-semibold text-gray-700 mb-2">
              Uploaded documents
            </h3>
            {documents.length === 0 ? (
              <p className="text-gray-500 text-sm">
                No documents have been uploaded.
              </p>
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
                        setDocuments((prev) =>
                          prev.filter((d) => d.id !== doc.id)
                        )
                      }
                      className="text-red-500 text-sm hover:underline"
                    >
                      Delete
                    </button>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </>
      )}
    </div>
  );
};

export default Sidebar;