using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aspose.Pdf;
using Aspose.Pdf.InteractiveFeatures.Forms;
using System.IO;

namespace p
{
    class Program
    {
        protected static string[] Args;
        protected static List<string> output = new List<string>();

        static void Main(string[] args)
        {
            // Get license
            Aspose.Pdf.License license = new Aspose.Pdf.License();
            license.SetLicense(@"c:\references\Aspose.Pdf.lic");
            
            Args = args;

            if(args.Length < 1) gethelp();
            else
            {
                switch(args[0].ToUpper())
                {
                    case "-UL":
                        if(args.Length>1) Console.WriteLine(resize());
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    case "-LF":
                    case "-LS":
                        if(args.Length>1) Console.WriteLine(getlist());
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    default:
                        break;

                }
            }
        }





        private static string resize()
        {
            // Open a document
            Document pdfDocument = new Document(Args[1]);
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
            return "File " + Args[1] + " has been changed to US letter size!";
        }

      

        private static string getlist()
        {
            //open document
            bool isXFAForm = true;
            
            Document pdfDocument = new Document(Args[1]);
            Console.WriteLine(pdfDocument.Form.Type); //there are: Standard 0, Static 1, Dynamic 2

            List<Field> listFields = new List<Field>();
            List<string> listValues = new List<string>();
            List<string> listTypes = new List<string>();

            
            if(!isXFAForm) //if not a XFA form
            {
                foreach(Field formField in pdfDocument.Form)
                {
                    // Recursively iterate to get all fields
                    getField(formField, ref listFields, ref listValues, ref listTypes);

                }

                foreach(Field str in listFields)
                {

                    output.Add(str.FullName + "," + str.Value + "," + str.GetType().ToString());

                }

                Output();

            }
            else  // if it's a XFA form
            {
                if(Args.Length==3)
                foreach(string formField in pdfDocument.Form.XFA.FieldNames)
                {
                    output.Add(formField + "," + pdfDocument.Form.XFA[formField]); // + "," + pdfDocument.Form.XFA[formField].GetType().ToString());
                }

                else if(Args.Length ==4 && Args[3].ToUpper() == "-XML" && pdfDocument.Form.XFA != null) {
                    //get field data
                    XmlNode data = pdfDocument.Form.XFA.Datasets;
                    //enumerate fields
                    enumFields(data, "");

                }

            }

           

            return "successfully explored the fields";


         }

        private static void Output()
        {
            if(Args[0].ToUpper() == "-LF")
            {
                if(Args.Length > 2) using(StreamWriter writer = new StreamWriter(Args[2])) foreach(string str in output) writer.WriteLine(str.Replace("Aspose.Pdf.InteractiveFeatures.Forms.", ""));
                else Console.WriteLine("You have to input output filename");

            }
            else if(Args[0].ToUpper() == "-LS")
            {
                foreach(string str in output) Console.WriteLine(str.Replace("Aspose.Pdf.InteractiveFeatures.Forms.", ""));
            }
        }

        //method to check all the fields
        public static void getField(Field field, ref List<Field> listFields, ref List<string> listValues, ref List<string> listTypes)
        {

            if(field.IsGroup)
            {
                foreach(Field childField in field)
                {
                    getField(childField, ref listFields, ref listValues, ref listTypes);
                }
            }
            else
            {
                listFields.Add(field);
            }

        }

        private static void enumFields(XmlNode node, string path) {
            //if this node has subnodes then call this routine recruively
            if(node.NodeType == XmlNodeType.Element && node.HasChildNodes)
            {
                string subPath;
                //path for the subfield
                if(path == "") subPath = node.Name;
                else subPath = path + "/" + node.Name;

                foreach(XmlNode subNode in node.ChildNodes) enumFields(subNode, subPath);
            }
            //if this text node then show field information
            else if(node.NodeType==XmlNodeType.Text){
                output.Add(path+","+node.Value+","+node.NodeType);
            }

            Output();
        }

        private static void gethelp()
        {

            Console.WriteLine("use: p -ul filename.pdf to convert pages to US letter size");
            Console.WriteLine("use: p -ls filename.pdf to display all fields, values and types in form fields on screen");
            Console.WriteLine("use: p -lf filename.pdf output.csv to get all fields, values and types in form fields to file");
          
            Console.WriteLine("use: p -ls filename.pdf output.csv -xml to get all XML Nodes, values and types in form fields to screen");
            Console.WriteLine("use: p -lf filename.pdf output.csv -xml to get all XML Nodes, values, and types in form fields to file");
        }


    }

    }

