using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p
{
    class Outputs
    {
        public static void Output(List<FieldValuePair> op)
        {

            foreach(FieldValuePair str in op)
            {
                Console.WriteLine(str.path + "\t" + str.fieldName.Replace("Aspose.Pdf.InteractiveFeatures.Forms.", "") + "\t" + str.fieldValue + "\t" + str.fieldType);
            }

        }
        public static void Output(List<FieldValuePair> op, string filename)
        {
            using(StreamWriter writer = new StreamWriter(filename))
            {
                foreach(FieldValuePair str in op) writer.WriteLine(str.path + "," + str.fieldName.Replace("Aspose.Pdf.InteractiveFeatures.Forms.", "") + "," + str.fieldValue + "," + str.fieldType);
            }
        }
    }
}
