using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p
{
    public class AppData
    {
        public SignatureData imm5476Rcic = new SignatureData() {
            path = @"c:\vba\signature-jacky.png",
            page = 2,
            x1 = 220f,
            y1 = 470f,
            x2 = 340f,
            y2 = 500f
        };
        public SignatureData imm5476PA = new SignatureData()
        {
            path = @"c:\vba\signature-xiangliang.png",
            page = 2,
            x1 = 220f,
            y1 = 220f,
            x2 = 340f,
            y2 = 250f
        };
        public SignatureData imm5476SP = new SignatureData()
        {
            path = @"c:\vba\Signature- Geng.png",
            page = 2,
            x1 = 220f,
            y1 = 170f,
            x2 = 340f,
            y2 = 200f
        };
        
    }

    public class SignatureData {
        public string path { get; set; }
        public int page { get; set; }
        public float x1 { get; set; }
        public float y1 { get; set; }
        public float x2 { get; set; }
        public float y2 { get; set; }
    }
}
