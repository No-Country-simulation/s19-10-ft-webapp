using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NotebookLM.Persistence.Services.KernelMemoryHelpers.GeminiEmbedding
{
    public class GoogleEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public EmbeddingData? Embedding { get; set; }

        public class EmbeddingData
        {
            [JsonPropertyName("values")]
            public List<float>? Values { get; set; }
        }
    }
}
