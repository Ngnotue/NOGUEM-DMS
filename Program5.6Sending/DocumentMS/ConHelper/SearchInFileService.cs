using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Text;

namespace DocumentMS.ConHelper
{
    public static class SearchInFileService
    {
        public static void SearchInFile(string FilePath, List<string> matcher, List<string> listOfFiles, string SearchItem)
        {
            foreach (var searchWord in File.ReadLines(FilePath))
            {
                matcher.Add(searchWord);
            }

            foreach (var fileName in listOfFiles)
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    if (line.Contains(SearchItem))
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }
        public static string GetTextFromWord(string FilePath)
        {
            string _InnerText = "";
            using (var doc = WordprocessingDocument.Open(FilePath, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                _InnerText = body.InnerText;
            }
            return _InnerText;
        }
        public static string GetTextFromPDF(string FilePath)
        {
            StringBuilder _StringBuilder = new();
            using (PdfReader reader = new PdfReader(FilePath))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    _StringBuilder.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }
            return _StringBuilder.ToString();
        }
        public static string GetTextFromText(string FilePath)
        {
            string _ReadAllText = System.IO.File.ReadAllText(FilePath);
            return _ReadAllText.ToString();
        }
        public static List<string> GetTextFromWordTMP(string FilePath)
        {
            List<string> lines = new List<string>();
            using (var doc = WordprocessingDocument.Open(FilePath, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                string totaltext = body.InnerText;

                foreach (var el in doc.MainDocumentPart.Document.Body.Elements().OfType<Paragraph>())
                {
                    lines.Add(el.InnerText);
                }
            }
            return lines;
        }
    }
}
