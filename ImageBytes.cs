using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Image img = Image.FromFile(@"C:\Users\ecghikv\Desktop\15370007a1ec1c7ce02f.jpg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();

                // Convert byte[] to Base64 String
                //string base64String = Convert.ToBase64String(arr);
                //Console.WriteLine(base64String);
            }

            MemoryStream m2s = new MemoryStream(arr);
            Image i = Image.FromStream(m2s);
        }
    }
}
