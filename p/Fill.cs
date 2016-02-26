using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using System.IO;
using Aspose.Pdf.Facades;
using Aspose.Pdf.InteractiveFeatures.Forms;

namespace p
{
    class Fill
    {
        public static void fillForm(string filename, Dictionary<string, string> dict)
        {

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
          
            // Instantiate Document instance
            Document pdfDocument = new Document(fs);



            foreach(KeyValuePair<string, string> kvp in dict)
            {
                pdfDocument.Form.XFA[kvp.Key] = kvp.Value;
            }


            // Save the updated document in save FileStream
            pdfDocument.Save();
            // Close the File Stream object
            fs.Close();
            //System.Diagnostics.Process.Start(filename);
        }

        public static void signForm(string filename)
        {
           
           string newname = Utility.sep(filename, '.')[0];
           newname = newname + "-signed.pdf";//save output PDF file


            ////open document
            //Aspose.Pdf.Facades.Form pdfForm = new Aspose.Pdf.Facades.Form();
            //pdfForm.BindPdf(newname);
            ////flatten fields
            //pdfForm.FlattenAllFields();


            ////save output
            //pdfForm.Save(newname);

            //create PdfFileMend object to add text
            PdfFileMend mender = new PdfFileMend();
            mender.BindPdf(filename);
            AppData ad = new AppData();
            //add image in the PDF file
            mender.AddImage(ad.imm5476Rcic.path, ad.imm5476Rcic.page,ad.imm5476Rcic.x1,ad.imm5476Rcic.y1,ad.imm5476Rcic.x2,ad.imm5476Rcic.y2);
            mender.AddImage(ad.imm5476PA.path, ad.imm5476PA.page, ad.imm5476PA.x1, ad.imm5476PA.y1, ad.imm5476PA.x2, ad.imm5476PA.y2);
            mender.AddImage(ad.imm5476SP.path, ad.imm5476SP.page, ad.imm5476SP.x1, ad.imm5476SP.y1, ad.imm5476SP.x2, ad.imm5476SP.y2);
            mender.Save(newname);
            //close PdfFileMend object
            mender.Close();
            System.Diagnostics.Process.Start(newname);
            Console.WriteLine(filename + " has been signed and saved as " + newname + " !");
        }

        public static void addAnotation(string filename) {
            //open document 
            PdfContentEditor contentEditor = new PdfContentEditor();
            contentEditor.BindPdf(filename);
            //crate rectangle
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(50, 50, 100, 100);
            //create annotation
            contentEditor.CreateFreeText(rect, "Sample content", 1);
            //save updated PDF file
            string newname = Utility.sep(filename, '.')[0];
            newname = newname + "-signed.pdf";
            contentEditor.Save(newname);
            System.Diagnostics.Process.Start(newname);
        }

    }
}
