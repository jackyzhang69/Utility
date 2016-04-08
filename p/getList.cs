using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using Aspose.Pdf.InteractiveFeatures.Forms;
using System.Xml;
using Aspose.Pdf.Facades;

namespace p
{
    public class FieldValuePair {
            public string path { get; set; }
            public string fieldName { get; set; }
            public string fieldValue { get; set; }
            public string fieldType { get; set; }
        }

    public class getList
    {
        static int i = 0;
        static FieldValuePair newpair = new FieldValuePair();

        public static List<FieldValuePair> getlist(string filename)
        {
            //open document
            List<FieldValuePair> returnValue = new List<FieldValuePair>();
            Document pdfDocument = new Document(filename);
            // Check form tyep. There are: Standard 0, Static 1, Dynamic 2 (1 & 2 is XFA)
            bool isXFAForm = (pdfDocument.Form.Type == FormType.Static || pdfDocument.Form.Type == FormType.Dynamic) ? true : false;

            Console.WriteLine("Form " + filename + " is a " + pdfDocument.Form.Type + " PDF file");

            

            if(!isXFAForm) //if not a XFA form
            {
                List<Field> listFields = new List<Field>();
                foreach(Field formField in pdfDocument.Form)
                {
                    // Recursively iterate to get all fields
                    getField(formField, ref listFields);

                }

                foreach(Field str in listFields)
                {
                    FieldValuePair newpair = new FieldValuePair();
                    newpair.fieldName = str.FullName;
                    newpair.fieldValue = str.Value;
                    returnValue.Add(newpair); 
                }

            }
            else  // if it's a XFA form
            {

                foreach(string formField in pdfDocument.Form.XFA.FieldNames)
                {

                    FieldValuePair newpair = new FieldValuePair();
                    newpair.fieldName = formField;
                    newpair.fieldValue = pdfDocument.Form.XFA[formField];
                    
                    returnValue.Add(newpair); 
                }
            }
            return returnValue;
        }

        private static void getField(Field field, ref List<Field> listFields)
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

        public static List<FieldValuePair> getXMLNodelist(string filename)
        {
            List<FieldValuePair> returnValue = new List<FieldValuePair>();
           
            using(Document pdfDocument = new Document(filename))
            {
                if(pdfDocument.Form.XFA != null)
                {
                    //get field data
                    XmlNode data = pdfDocument.Form.XFA.Datasets;
                    //enumerate fields
                    enumFields(data, "", ref returnValue);
                   
                }
            }
            return returnValue;    

        }
        private static void enumFields(XmlNode node, string path, ref List<FieldValuePair> rv)
        {
            
            //if this node has subnodes then call this routine recruively
            if(node.NodeType == XmlNodeType.Element && node.HasChildNodes)
            {
                string subPath;
                //path for the subfield
                if(path == "") subPath = node.Name;
                else subPath = path + "/" + node.Name;

                foreach(XmlNode subNode in node.ChildNodes) enumFields(subNode, subPath,ref rv);
            }
            //if this text node then show field information
            else if(node.NodeType == XmlNodeType.Text)
            {
                Console.WriteLine(i);
                i++;
                newpair.path = path;
                newpair.fieldName = node.Name;
                newpair.fieldValue = node.Value;
                newpair.fieldType = node.InnerXml;
                rv.Add(newpair);
            }

          
        }

    

    }
}
