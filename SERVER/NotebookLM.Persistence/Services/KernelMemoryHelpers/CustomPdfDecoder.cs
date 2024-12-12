using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory.DataFormats;
using Microsoft.KernelMemory.Diagnostics;
using Microsoft.KernelMemory.Pipeline;

using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis;
using UglyToad.PdfPig.Util;

namespace NotebookLM.Persistence.Services.KernelMemoryHelpers
{
    public class CustomPdfDecoder : IContentDecoder
    {
        private readonly ILogger<CustomPdfDecoder> _log;

        public CustomPdfDecoder(ILoggerFactory? loggerFactory = null)
        {
            _log = (loggerFactory ?? DefaultLogger.Factory).CreateLogger<CustomPdfDecoder>();
        }

        /// <inheritdoc />
        public bool SupportsMimeType(string mimeType)
        {
            return mimeType != null && mimeType.StartsWith(MimeTypes.Pdf, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public Task<FileContent> DecodeAsync(string filename, CancellationToken cancellationToken = default)
        {
            using var stream = File.OpenRead(filename);
            return DecodeAsync(stream, cancellationToken);
        }

        /// <inheritdoc />
        public Task<FileContent> DecodeAsync(BinaryData data, CancellationToken cancellationToken = default)
        {
            using var stream = data.ToStream();
            return DecodeAsync(stream, cancellationToken);
        }

        /// <inheritdoc />
        public Task<FileContent> DecodeAsync(Stream data, CancellationToken cancellationToken = default)
        {
            _log.LogDebug("Extracting text from PDF file");

            var result = new FileContent(MimeTypes.PlainText);

            using PdfDocument? pdfDocument = PdfDocument.Open(data);
            if (pdfDocument == null) { return Task.FromResult(result); }

            var options = new ContentOrderTextExtractor.Options
            {
                ReplaceWhitespaceWithSpace = true,
                SeparateParagraphsWithDoubleNewline = false,
            };

            var docDecorations = DecorationTextBlockClassifier.Get(pdfDocument.GetPages().ToList(),
                        DefaultWordExtractor.Instance,
                        DocstrumBoundingBoxes.Instance);

            foreach (var page in pdfDocument.GetPages())
            {
                if (page == null) continue;

                string pageContent = (ContentOrderTextExtractor.GetText(page, options) ?? string.Empty).ReplaceLineEndings(" ");
                var section = new FileSection(page.Number, pageContent, false);

                IReadOnlyList<TextBlock> decorations = docDecorations[page.Number - 1];
                var decorationDescriptions = new List<string>();

                foreach (var decoration in decorations)
                {
                    string decorationText = decoration.Text ?? string.Empty;
                    decorationDescriptions.Add($"[Decoration: {decorationText}]");
                }
                var enrichedContent = pageContent;

                if (decorationDescriptions.Any())
                {
                    enrichedContent += Environment.NewLine + string.Join(Environment.NewLine, decorationDescriptions);
                }

                // Crear una nueva sección con el contenido enriquecido
                var newSection = new FileSection(page.Number, enrichedContent, false);

                result.Sections.Add(section);
            }

            return Task.FromResult(result);




        }
    }
}
