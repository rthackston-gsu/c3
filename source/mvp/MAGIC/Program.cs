using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magic.gsu.edu
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateS3.GetAvailableBucket();
            CreateEc2.CreateInstance();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
