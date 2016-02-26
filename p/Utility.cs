using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using System.IO;
using p.Database;

namespace p
{
    class Utility
    {
        public static void gethelp()
        {

            Console.WriteLine("use: p -ul filename.pdf to convert pages to US letter size");
            Console.WriteLine("use: p -ls filename.pdf to display all fields, values and types in form fields on screen");  //xfm types??? value member and display member
            Console.WriteLine("use: p -lf filename.pdf output.csv to get all fields, values and types in form fields to file");  //same as above
            Console.WriteLine("use: p -lf filename.pdf output.csv -xml to get all XML Nodes, values, and types in form fields to file"); //same as above
            Console.WriteLine("use: p -c source.pdf destination.pdf to copy a original pdf to another same structure pdf");
            Console.WriteLine("use: p -cp source.pdf destination.pdf to copy a original pdf to another different structure pdf, but may have same elements");
            Console.WriteLine("use: p -sf "); // should be developed more
            Console.WriteLine("use: p -o filename "); // Open a file, any kind


        }


        public static string[] getTrimed(string[] input)
        {
            string[] output = new string[4];
            foreach(string str in input)
            {
                int i = 0;
                if(str[0] == '\"' && str[str.Length - 1] == '\"')
                {
                    output[i] = str.Substring(1, str.Length - 2);
                }
                else
                {
                    output[i] = str;
                }
                Console.WriteLine(output[i]);
                i++;
            }

            return output;
        }
        public static void getSelectedList(string str1, string str2)
        {  // this one should be double check tomorrow
            string filepath = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo(filepath);
            Console.WriteLine("Current path: " + filepath.ToString());

            foreach(var file in d.GetFiles("*.pdf"))
            {
                //Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
                //Open document
                Document pdfDocument = new Document(file.ToString());

                Console.WriteLine(file.ToString());
                //Get document information
                DocumentInfo docInfo = pdfDocument.Info;

                //Show document information
                if(docInfo.Author == str2)
                {
                    Console.WriteLine("Author: {0}", docInfo.Author);
                    Console.WriteLine("Subject: {0}", docInfo.Subject);
                    Console.WriteLine("Title: {0}", docInfo.Title);
                    Console.WriteLine("Keywords: {0}", docInfo.Keywords);
                    Console.WriteLine("Creation Date: {0}", docInfo.CreationDate);
                    Console.WriteLine("Modify Date: {0}", docInfo.ModDate);
                }
            }

        }


        public static string[] sep(string s, char c)  // seperate one string to two with a char
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

    public static class Country
    {

        public static int countryToCode(this string value)
        {

            using(CommonDataContext cd = new CommonDataContext())
            {
                return cd.tblCountries.Where(x => x.Country == value).Select(x => x.CountryCode).FirstOrDefault();


            }

        }
        public static string countryToString(this int value)
        {
            using(CommonDataContext cd = new CommonDataContext())
            {
                return cd.tblCountries.Where(x => x.CountryCode == value).Select(x => x.Country).FirstOrDefault();


            }
        }
    }

    public static class individule
    {

        public static void showme(int id)
        {
            using(CommonDataContext cd = new CommonDataContext())
            {

                tblPerson p= cd.tblPersons.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                Console.WriteLine(p.Id + "\t" + p.FirstName + "\t" + p.LastName + "\t" + p.DOB);

            }
        }


    }
}
