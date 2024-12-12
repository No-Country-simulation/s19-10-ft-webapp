
using Microsoft.ML.Tokenizers;
using Microsoft.KernelMemory.AI;
using System;
using System.Collections.Generic;
using System.IO;
using Sentencepiece;
using Microsoft.ML.Model.OnnxConverter;

namespace NotebookLM.Persistence.Services.KernelMemoryHelpers.GemmaTokenizer
{
#pragma warning disable KMEXP00 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class GemmaSentencePieceTokenizer : ITextTokenizer
#pragma warning restore KMEXP00 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    {
        private readonly SentencePieceBpeTokenizer _tokenizer;

        public GemmaSentencePieceTokenizer()
        {

            string modelPath = "C:\\Users\\tomas\\source\\repos\\ATaleOfMemories\\GeminiKernelMemoryAdapters\\GemmaTokenizer\\tokenizer.model";

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException("El archivo del modelo no existe.", modelPath);
            }
            // Load the model file
            using FileStream modelStream = new FileStream(modelPath, FileMode.Open, FileAccess.Read);

            //Using LlamaTokenizer because is the only way to use SentencePieceTokenizer Implementation from ML.NET
            _tokenizer = LlamaTokenizer.Create(modelStream, addBeginOfSentence: true, addEndOfSentence: false);
           

            // Create the SentencePieceTokenizer
        }

        public int CountTokens(string text)
        {
            return _tokenizer.CountTokens(text);
        }

        public IReadOnlyList<string> GetTokens(string text)
        {
            return _tokenizer.EncodeToTokens(text, out string? _).Select(t => t.Value).ToList();
        }
    }
}
