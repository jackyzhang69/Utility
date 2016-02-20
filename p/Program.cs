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
           
            // Create a PDF license object
            Aspose.Pdf.License license = new Aspose.Pdf.License();
            // Instantiate license file
            license.SetLicense("p.Aspose.Pdf.lic");
            // Set the value to indicate that license will be embedded in the application
            license.Embedded = true;

            Args = args;
            
            if(args.Length ==0) gethelp();
            else
            {
                switch(args[0].ToUpper())
                {
                    case "-UL":
                        if(args.Length > 1) Console.WriteLine(resize());
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    case "-LF":  //List fields to file
                        switch(args.Length) {
                            case 1:
                            case 2:
                                Console.WriteLine("You have to assign a source pdf and an output file name!");
                                Console.WriteLine("Useage: p -lf input.pdf output.txt");
                                Console.WriteLine("Or: p -lf input.pdf output.txt -xml");
                                break;
                          
                            case 3:
                            case 4:
                                try { getlist(args[1]); Output(); Console.WriteLine(Args[2] + " was created!"); }
                                catch(Exception e) { Console.WriteLine(e.Message); }
                                break;
                            default:
                                Console.WriteLine("Unnecessarily parameter entered...");
                                break;       

                         }
                        
                        break;
                    
                    case "-LS":  //List fields to screen
                        if(args.Length == 2) { getlist(args[1]); Output(); }
                        else if (args.Length>2) Console.WriteLine("You entered too many parameters!\nUsage: p -ls input.pdf");
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    case "-GI":
                        if(args.Length == 2) {
                            try
                            {
                                getPDFInfo(args[1]);
                            }
                            catch(Exception e)
                            {

                                Console.WriteLine(e.Message);
                            }

                        }
                        else Console.WriteLine("You should input source PDF file.");
                        break;
                    case "-SI":
                        if(args.Length < 6) Console.WriteLine("You should input source PDF file.");
                        else if(args.Length >= 6) {
                            if(args.Length>6)Console.WriteLine("You entered too many parameters, after sixth will be trimed");
                            string[] info = new string[4];
                            info[0] = args[2];
                            info[1] = args[3];
                            info[2] = args[4];
                            info[3] = args[5];
                            setPDFInfo(args[1],info);
                            Console.WriteLine("File " + args[1] + " has been set a new set information!\nUse p -gi "+args[1]+" to check the update");
                        }
                        break;
                    case "-C":
                        if(args.Length != 3) Console.WriteLine("Wrong parameter numbers.");
                        else copyPDFs(args[1], args[2]);
                        break;
                    case "-CP":
                        DiffPDFs.copy(args[1],args[2]);
                        break;
                    case "-SF": // Keep working ....
                        getSelectedList("Author","Jacky");
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

      

        private static void getlist(string filename)
        {
            //open document

            Document pdfDocument = new Document(Args[1]);
            // Check form tyep. There are: Standard 0, Static 1, Dynamic 2 (1 & 2 is XFA)
            bool isXFAForm = (pdfDocument.Form.Type == FormType.Static || pdfDocument.Form.Type == FormType.Dynamic) ? true:false;
            
            Console.WriteLine("Form "+filename+" is a "+ pdfDocument.Form.Type+" PDF file"); 

            List<Field> listFields = new List<Field>();
                  
            if(!isXFAForm) //if not a XFA form
            {
                foreach(Field formField in pdfDocument.Form)
                {
                    // Recursively iterate to get all fields
                    getField(formField, ref listFields);

                }

                foreach(Field str in listFields)
                {

                    output.Add(str.FullName + "," + str.Value + "," + str.GetType().ToString());

                }

            }
            else  // if it's a XFA form
            {

                if(Args.Length == 4 && Args[3].ToUpper() == "-XML" && pdfDocument.Form.XFA != null)
                {
                    //get field data
                    XmlNode data = pdfDocument.Form.XFA.Datasets;
                    //enumerate fields
                    enumFields(data, "");
                }
                else {
                    foreach(string formField in pdfDocument.Form.XFA.FieldNames)
                        output.Add(formField + "," + pdfDocument.Form.XFA[formField]); // + "," + pdfDocument.Form.XFA[formField].GetType().ToString());
                }

            }

           

            //return "successfully explored the fields";


         }

        private static List<string> getlist1(string filename)
        {
            //open document
            List<string> returnValue = new List<string>();
            Document pdfDocument = new Document(Args[1]);
            // Check form tyep. There are: Standard 0, Static 1, Dynamic 2 (1 & 2 is XFA)
            bool isXFAForm = (pdfDocument.Form.Type == FormType.Static || pdfDocument.Form.Type == FormType.Dynamic) ? true : false;

            Console.WriteLine("Form " + filename + " is a " + pdfDocument.Form.Type + " PDF file");

            List<Field> listFields = new List<Field>();

            if(!isXFAForm) //if not a XFA form
            {
                foreach(Field formField in pdfDocument.Form)
                {
                    // Recursively iterate to get all fields
                    getField(formField, ref listFields);

                }

                foreach(Field str in listFields)
                {

                    returnValue.Add(str.FullName + "," + str.Value + "," + str.GetType().ToString());

                }

            }
            else  // if it's a XFA form
            {

                if(Args.Length == 4 && Args[3].ToUpper() == "-XML" && pdfDocument.Form.XFA != null)
                {
                    //get field data
                    XmlNode data = pdfDocument.Form.XFA.Datasets;
                    //enumerate fields
                    returnValue=enumFields1(data, "");
                }
                else
                {
                    foreach(string formField in pdfDocument.Form.XFA.FieldNames)
                        returnValue.Add(formField + "," + pdfDocument.Form.XFA[formField]); // + "," + pdfDocument.Form.XFA[formField].GetType().ToString());
                }

            }
            return returnValue;
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
        public static void getField(Field field, ref List<Field> listFields)
        {

            if(field.IsGroup)
            {
                foreach(Field childField in field)
                {
                    getField(childField, ref listFields);
                }
            }
            else
            {
                listFields.Add(field);
            }

        }
        // mothod toget all xml nodes 
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
        }

        private static List<string> enumFields1(XmlNode node, string path)
        {
            List<string> returnValue = new List<string>();
            //if this node has subnodes then call this routine recruively
            if(node.NodeType == XmlNodeType.Element && node.HasChildNodes)
            {
                string subPath;
                //path for the subfield
                if(path == "") subPath = node.Name;
                else subPath = path + "/" + node.Name;

                foreach(XmlNode subNode in node.ChildNodes) enumFields1(subNode, subPath);
            }
            //if this text node then show field information
            else if(node.NodeType == XmlNodeType.Text)
            {
                returnValue.Add(path + "," + node.Value + "," + node.NodeType);
            }

            return returnValue;
        }

        private static void gethelp()
        {

            Console.WriteLine("use: p -ul filename.pdf to convert pages to US letter size");
            Console.WriteLine("use: p -ls filename.pdf to display all fields, values and types in form fields on screen");
            Console.WriteLine("use: p -lf filename.pdf output.csv to get all fields, values and types in form fields to file");
            Console.WriteLine("use: p -lf filename.pdf output.csv -xml to get all XML Nodes, values, and types in form fields to file");
        }

        private static void getPDFInfo(string filename) {

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
        private static void setPDFInfo(string filename, string[] info) {
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
        private static string[] getTrimed(string[] input) {
            string[] output = new string[4];
            foreach(string str in input) {
                int i = 0;
                if(str[0] == '\"' && str[str.Length - 1] == '\"')
                {
                    output[i] = str.Substring(1, str.Length - 2);
                }
                else {
                    output[i] = str;
                }
                Console.WriteLine(output[i]);
                i++;
            }

            return output;
        }
        private static void getSelectedList(string str1, string str2) {  // this one should be double check tomorrow
            string filepath = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo(filepath);
            Console.WriteLine("Current path: "+filepath.ToString());

            foreach(var file in d.GetFiles("*.pdf"))
            {
                //Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
                //Open document
                Document pdfDocument=new Document(file.ToString());
               
                Console.WriteLine(file.ToString());
                //Get document information
                DocumentInfo docInfo = pdfDocument.Info;

                //Show document information
                if(docInfo.Author == str2) { 
                        Console.WriteLine("Author: {0}", docInfo.Author);
                        Console.WriteLine("Subject: {0}", docInfo.Subject);
                        Console.WriteLine("Title: {0}", docInfo.Title);
                        Console.WriteLine("Keywords: {0}", docInfo.Keywords);
                        Console.WriteLine("Creation Date: {0}", docInfo.CreationDate);
                        Console.WriteLine("Modify Date: {0}", docInfo.ModDate);
                }
            }

        }
        private static void copyPDFs(string source, string dest) {

            Dictionary<string, string> sourcePDF = new Dictionary<string, string>();
            Dictionary<string, string> DestPDF = new Dictionary<string, string>();
            List<string> destList = new List<string>();

            getlist(source);  // get source field names and values to dictionary sourcePDF
            foreach(string str in output) {
                sourcePDF.Add(sep(str, ',')[0],sep(str, ',')[1]);
                //Console.WriteLine(sep(str,',')[0] + "\t\t\t" + sep(str, ',')[1]); //split to two string with first ','
            }

            destList= getlist1(dest);  // get destination pdf field names and values to the global string list: output

            int i = 0;
            foreach(string str in destList) {   //output list is now loaded destination pdf file fields and values
                foreach(KeyValuePair<string, string> kvpsource in sourcePDF) {
                    //copy source field name and value to dest dictionary
                    if(sep(str, ',')[0] == kvpsource.Key) DestPDF.Add(kvpsource.Key, kvpsource.Value); 
                    
                }
            }

            try
            {
                Fill.fillForm(dest, DestPDF);
                Console.WriteLine("Has completed to copy {0} to {1} !",source,dest);
            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);
            }
            

        }

        private static string[] sep(string s, char c)  // seperate one string to two with a char
        {
            string[] outnew = new string[2];
            int l = s.IndexOf(c);
            if(l > 0)
            {
                outnew[0] = s.Substring(0, l);
                if(s.Length > l) outnew[1] = s.Substring(l + 1, s.Length - l - 1);
                else outnew[1] = "";
                return outnew;
            }
            else
            {
                outnew[0] = "";
                if(s.Length > 0) outnew[1] = s.Substring(l + 1, s.Length - l - 1);
                else outnew[1] = "";
                return outnew;
            }
        }

    }

    }

