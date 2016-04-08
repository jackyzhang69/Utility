using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aspose.Pdf;
using Aspose.Pdf.InteractiveFeatures.Forms;
using System.IO;
using Aspose.Pdf.Facades;
using p.Database;
using System.Diagnostics;

namespace p
{
    class Program
    {
        protected static string[] Args;
        protected static List<FieldValuePair> output = new List<FieldValuePair>();

        static void Main(string[] args)
        {
            args = new string[4];
            args[0] = "-lf";
            args[1] = "emp5593.pdf";
            args[2] = "e.csv";
            args[3] = "-xml";
           
            // Create a PDF license object
            Aspose.Pdf.License license = new Aspose.Pdf.License();
            // Instantiate license file
            license.SetLicense("p.Aspose.Pdf.lic");
            // Set the value to indicate that license will be embedded in the application
            license.Embedded = true;

            if(args.Length == 0) Utility.gethelp();
            else
            {
                switch(args[0].ToUpper())
                {
                    case "-UL":
                        if(args.Length > 1) Console.WriteLine(PDFs.resize(args[1]));
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    case "-LF":  //List fields to file
                        switch(args.Length)
                        {
                            case 1:
                            case 2:
                                Console.WriteLine("You have to assign a source pdf and an output file name!");
                                Console.WriteLine("Useage: p -lf input.pdf output.txt");
                                Console.WriteLine("Or: p -lf input.pdf output.txt -xml");
                                break;
                            case 3:
                                try { output = getList.getlist(args[1]); Outputs.Output(output, args[2]); Console.WriteLine(Args[2] + " was created!"); }
                                catch(Exception e) { Console.WriteLine(e.Message); }
                                break;
                            case 4:
                                if(args[3].ToUpper() == "-XML")
                                    try {
                                        output = getList.getXMLNodelist(args[1]);
                                        Outputs.Output(output, args[2]);
                                        Console.WriteLine(Args[2] + " was created!");
                                    }
                                    catch(Exception e) { Console.WriteLine(e.Message); }
                                break;
                            default:
                                Console.WriteLine("Unnecessarily parameters entered...");
                                break;
                        }
                        break;

                    case "-LS":  //List fields to screen
                        if(args.Length == 2) { output = getList.getlist(args[1]); Outputs.Output(output); }
                        else if(args.Length > 2) Console.WriteLine("You entered too many parameters!\nUsage: p -ls input.pdf");
                        else Console.WriteLine("You have to assign a pdf file name!");
                        break;
                    case "-GI":
                        if(args.Length == 2)
                        {
                            try
                            {
                                FileInfo.getPDFInfo(args[1]);
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
                        else if(args.Length >= 6)
                        {
                            if(args.Length > 6) Console.WriteLine("You entered too many parameters, after sixth will be trimed");
                            string[] info = new string[4];
                            info[0] = args[2];
                            info[1] = args[3];
                            info[2] = args[4];
                            info[3] = args[5];
                            FileInfo.setPDFInfo(args[1], info);
                            Console.WriteLine("File " + args[1] + " has been set a new set information!\nUse p -gi " + args[1] + " to check the update");
                        }
                        break;
                    case "-SF": // Keep working ....
                        Utility.getSelectedList("Author", "Jacky");
                        break;
                    case "-C":
                        if(args.Length != 3) Console.WriteLine("Wrong parameter numbers.");
                        else PDFs.copyPDFs(args[1], args[2]);
                        break;
                    case "-CP":
                        PDFs.copy(args[1], args[2]);
                        break;
                    case "-SG":
                        if(args.Length == 2) Fill.signForm(args[1]);
                        else Console.WriteLine("Wrong parameters! \nUse: p -sg source.pdf to sign");
                        break;
                    case "-SD":
                        // if(args.Length == 2)Console.WriteLine(int.Parse(args[1]).countryToString());/*DataOps.search(int.Parse(args[1]));*/
                        if(args.Length == 2) individule.showme(int.Parse(args[1]));
                        else Console.WriteLine("Wrong parameters! \nUse: p -sg source.pdf to sign");
                        break;
                    case "-O":
                        if(args.Length == 2)
                            try
                            {
                                Process.Start(args[1]);
                            }
                            catch(Exception e)
                            {

                                Console.WriteLine(args[1]+": "+e.Message);
                            }
                        else Console.WriteLine("Use: p -o to open a file");
                        break;
                    default:
                        break;

                }
            }
        }

   }

}

