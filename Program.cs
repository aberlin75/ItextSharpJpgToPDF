using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;

namespace ConvertJPGToPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime _start = DateTime.Now;
            Console.WriteLine("Ingrese la ruta donde se localizan los arhivos JPG:");
            String _path = Console.ReadLine();
            List<String> fileList = getFileList(_path);

            int count = 0;
            DateTime _startFiles = DateTime.Now;
            foreach (string fileName in fileList)
            {
                Document document = new Document(PageSize.LEGAL, 0, 0, 0, 0);
                string _pdfFileName = $"{fileName.Substring(0, fileName.Length - 4)}.pdf";
                using (FileStream stream = new FileStream(_pdfFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    writer.SetFullCompression();
                    document.Open();
                    document.SetMargins(0, 0, 0, 0);
                    using (FileStream imageStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Image image = Image.GetInstance(imageStream);
                        image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                        image.SetAbsolutePosition(0, document.PageSize.Height - image.ScaledHeight);
                        writer.DirectContent.AddImage(image);
                    }
                    document.Close();
                }
                count += 1;
            }

            DateTime _finishFiles = DateTime.Now;
            TimeSpan _tsFiles = _finishFiles - _startFiles;
            Console.WriteLine($"promedio de conversión por archivo {(_tsFiles.TotalMinutes / count).ToString("N", CultureInfo.InvariantCulture)} minutos.");

            DateTime _finish = DateTime.Now;
            TimeSpan _ts = _finish - _start;
            Console.WriteLine($"Proceso finalizado en {_ts.TotalMinutes.ToString("N", CultureInfo.InvariantCulture)} minutos.");
            Console.WriteLine("Presione cualquier tecla para salir");
            Console.ReadKey();
        }

        static List<String> getFileList(String directoryName)
        {
            DateTime _start = DateTime.Now;
            Console.WriteLine($"{_start.ToString("G", CultureInfo.CreateSpecificCulture("es-ES"))} - Iniciando lectura de imágenes en el directorio.");

            List<String> _result = Directory.GetFileSystemEntries(directoryName, "*.jpg", SearchOption.AllDirectories).ToList();

            DateTime _finish = DateTime.Now;
            Console.WriteLine($"{_finish.ToString("G", CultureInfo.CreateSpecificCulture("es-ES"))} - Se terminó la lectura de imágenes en el directorio.");
            TimeSpan _ts = _finish - _start;
            Console.WriteLine($"Duración: {_ts.TotalMinutes.ToString("N", CultureInfo.InvariantCulture)} minutos.");
            Console.WriteLine($"Total de archivos encontrados: {_result.Count.ToString("N0", CultureInfo.InvariantCulture)}.");
            Console.Write("");
            return _result;
        }
    }
}
