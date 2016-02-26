using Aspose.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace p
{
    public class PDFs
    {

        public static void copy(string source, string dest)
        {

            List<FieldValuePair> sourcePDF = new List<FieldValuePair>();
            List<FieldValuePair> sourceInterpreation = new List<FieldValuePair>();
            List<FieldValuePair> destInterpreation = new List<FieldValuePair>();

            Dictionary<string, string> destDict = new Dictionary<string, string>();

           // List<FieldValuePair> destPDF = new List<FieldValuePair>();
           
            
            sourcePDF = getList.getlist(source);
            sourceInterpreation = getList.getlist(source);  // have to work out it.
            destInterpreation= getList.getlist(source);     // have to work out it.
            //destPDF = getList.getlist(dest);


            var intMe = from s in sourcePDF         // join source and source interpreation table, output intermediary table,which includes expression and value
                    join si in sourceInterpreation
                    on s.fieldName equals si.fieldName
                    select new
                    {
                        newkey = si.fieldValue,  // intermediater expression such as FamilyName
                        newvalue=s.fieldValue,  // source value pass to a new temp object
                    };

            var finalObj = from ss in intMe   //join intermediary table and final table expression to set destination field name and value
                         join di in destInterpreation
                         on ss.newkey equals di.fieldName
                         select new
                         {
                             finalKey=di.fieldName,
                             finalValue=ss.newvalue
                         };

            foreach(var x in finalObj)
            {
                destDict.Add(x.finalKey, x.finalValue);
               // Console.WriteLine(x.newkey + "\t" + x.newvalue);

            }
            Fill.fillForm(dest, destDict);

            //Console.ReadKey();
        }
        public static string resize(string filename)
        {
            // Open a document
            Document pdfDocument = new Document(filename);
            // Get page collection
            PageCollection pageCollection = pdfDocument.Pages;
            // Get every page
            foreach(Page page in pageCollection)
            {
                //Page pdfPage = pageCollection[2];
                // Set the page size as A4 (11.7 x 8.3 in) and in Aspose.Pdf, 1 inch = 72 points
                // so A4 dimensions in points will be (842.4, 597.6)
                page.SetPageSize(597.6, 842.4);
            }
            // Save the updated document
            pdfDocument.Save("Updated.pdf");
            return "File " + filename + " has been changed to US letter size!";
        }
        public static void copyPDFs(string source, string dest)
        {

            List<FieldValuePair> sourcePDF = new List<FieldValuePair>();
            Dictionary<string, string> destPDF = new Dictionary<string, string>();
            List<FieldValuePair> destList = new List<FieldValuePair>();

            sourcePDF = getList.getlist(source);  // get source field names and values to List sourcePDF
            destList = getList.getlist(dest).ToList();
            //destPDF = getList.getlist(dest).Select(x => new { x.fieldName, x.fieldValue }).ToDictionary(x => x.fieldName, x => x.fieldValue);  // get source field names and values to dictionary destPDF

            var newlist = from s in sourcePDF                   // construct new list including desitination field name and source value
                          join d in destList
                          on s.fieldName equals d.fieldName
                          select new { d.fieldName, s.fieldValue };

            foreach(var str in newlist) destPDF.Add(str.fieldName, str.fieldValue);  // build up the destination dictionary


            try
            {
                Fill.fillForm(dest, destPDF);
                Console.WriteLine("Has completed to copy {0} to {1} !", source, dest);
            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);
            }


        }
    }
}
