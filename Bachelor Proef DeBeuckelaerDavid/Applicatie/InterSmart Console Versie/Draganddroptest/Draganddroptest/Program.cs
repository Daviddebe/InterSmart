using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draganddroptest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string file = args[0];
                
                if (File.Exists(file))
                {
                    string FileName = Path.GetFileNameWithoutExtension(file); //zal naam weergeven
                    string FilePath = Path.GetFullPath(file); //zal lokatie weergeven
                    Console.WriteLine(FileName);
                    Console.WriteLine(FilePath);
                    Console.ReadLine();
                }
            }
        }
    }
}
