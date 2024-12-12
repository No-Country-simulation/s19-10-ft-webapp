import React, { useState, useRef } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { filerSchema } from "../schemas";
import { uploadFile } from "../actions";

interface Document {
  id: number;
  name: string;
  size: string; // Ejemplo: "2 MB"
}

const Sidebar: React.FC = () => {
  const [document, setDocument] = useState<Document | null>(null); // Cambiado para un solo archivo
  const [dragging, setDragging] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(false);

    const files = Array.from(event.dataTransfer.files);
    if (files.length > 1) {
      toast.error("Only one file can be uploaded at a time.");
    } else {
      validateAndProcessFile(files[0]);
    }
  };

  const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = () => setDragging(false);

  const handleFileInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      validateAndProcessFile(file);
    }
  };

  const handleAreaClick = () => {
    fileInputRef.current?.click();
  };

  const validateAndProcessFile = async (file: File) => {
    const parsed = filerSchema.safeParse({
      name: file.name,
      size: file.size,
      type: file.type,
    });

    if (parsed.success) {
      const newDocument: Document = {
        id: Date.now(),
        name: file.name,
        size: `${(file.size / (1024 * 1024)).toFixed(2)} MB`,
      };

      try {
        await uploadFile(file);
        toast.success(`File "${file.name}" uploaded successfully!`);
        setDocument(newDocument); // Reemplaza el documento existente
      } catch (error) {
        toast.error(`Failed to upload "${file.name}".`);
        console.error("Error uploading file:", error);
      }
    } else {
      const errorMessage = parsed.error.errors[0].message;
      toast.error(`Error with file "${file.name}": ${errorMessage}`);
    }
  };

  return (
    <div className="w-64 h-full bg-gray-100 border-r border-gray-300 flex flex-col">
      <ToastContainer /> {/* Contenedor para las notificaciones */}
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
          className="hidden" // Ocultar input
          onChange={handleFileInputChange}
        />
        <p className="text-gray-600 text-center">
          {dragging ? "Drop to upload" : "Drag document here or click to upload"}
        </p>
      </div>

      {/* Información del documento */}
      <div className="p-4 overflow-y-auto">
        <h3 className="font-semibold text-gray-700 mb-2">Uploaded document</h3>
        {document === null ? (
          <p className="text-gray-500 text-sm">No document has been uploaded.</p>
        ) : (
          <div className="flex items-center justify-between p-2 bg-white rounded-md shadow">
            <div>
              <p className="font-medium text-gray-700">{document.name}</p>
              <p className="text-xs text-gray-500">{document.size}</p>
            </div>
            <button
              onClick={() => setDocument(null)} // Eliminar el archivo actual
              className="text-red-500 text-sm hover:underline"
            >
              Delete
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Sidebar;