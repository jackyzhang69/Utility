using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace p
{
    public class DiffPDFs
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
    }
}
