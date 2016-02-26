using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;

namespace p
{
    class FileInfo
    {
        public static void getPDFInfo(string filename)
        {

            //Open document
            Document pdfDocument = new Document(filename);

            //Get document information
            DocumentInfo docInfo = pdfDocument.Info;

            //Show document information
            Console.WriteLine("Author: {0}", docInfo.Author);
            Console.WriteLine("Subject: {0}", docInfo.Subject);
            Console.WriteLine("Title: {0}", docInfo.Title);
            Console.WriteLine("Keywords: {0}", docInfo.Keywords);
            Console.WriteLine("Creation Date: {0}", docInfo.CreationDate);
            Console.WriteLine("Modify Date: {0}", docInfo.ModDate);

        }
        public static void setPDFInfo(string filename, string[] info)
        {
            // Open document
            Document pdfDocument = new Document(filename);
            // Specify document information
            DocumentInfo docInfo = new DocumentInfo(pdfDocument);
            docInfo.Author = info[0];
            docInfo.Subject = info[1];
            docInfo.Title = info[2];
            docInfo.Keywords = info[3];
            docInfo.CreationDate = DateTime.Now;
            docInfo.ModDate = DateTime.Now;
            // Save output document
            pdfDocument.Save(filename);

        }
    }
}
