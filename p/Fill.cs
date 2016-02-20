using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using System.IO;


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
            System.Diagnostics.Process.Start(filename);
           
            
        }
      
    }
}
