using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dora;

namespace Test
{
    class ConsoleTest
    {
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Mysterio s = new Mysterio(2048);

            Mysterio c = new Mysterio(2048);

            byte[] sPri = Mysterio.ConvertKeyToBytes(s.PriKey);

            byte[] cPri = Mysterio.ConvertKeyToBytes(c.PriKey);

            string plain = "Gửi từ server";

            byte[] s_to_c = Mysterio.Encrypt(c.PubKey, plain);

            string in_c_result = Mysterio.Decrypt(c.PriKey, s_to_c);

            Console.WriteLine("in_c_result: " + in_c_result);

            plain = "gưởi từ client";

            byte[] c_to_s = Mysterio.Encrypt(s.PubKey, plain);

            string in_s_result = Mysterio.Decrypt(s.PriKey, c_to_s);

            Console.WriteLine("in_s_result: " + in_s_result);

            Console.ReadLine();
        }
    }
}
