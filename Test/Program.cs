using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dora;

namespace Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Mysterio s = new Mysterio(2048);

            Mysterio c = new Mysterio(2048);

            string sPub = Mysterio.ConvertKeyToString(s.PubKey);
            string sPri = Mysterio.ConvertKeyToString(s.PriKey);

            Console.WriteLine(sPub);
            Console.WriteLine(sPri);

            string cPub = Mysterio.ConvertKeyToString(c.PubKey);
            string cPri = Mysterio.ConvertKeyToString(c.PriKey);

            Console.WriteLine(cPub);
            Console.WriteLine(cPri);

            string plain = "Gửi từ server";

            string s_to_c_base64 = c.CreateCypherBase64String(plain);

            string in_c_result = c.DecryptCypherToString(s_to_c_base64);

            Console.WriteLine("in_c_result" + in_c_result);

            plain = "gưởi từ client";

            string c_to_s_base64 = s.CreateCypherBase64String(plain);

            string in_s_result = s.DecryptCypherToString(c_to_s_base64);

            Console.WriteLine(in_s_result);


            Console.ReadLine();
        }
    }
}
