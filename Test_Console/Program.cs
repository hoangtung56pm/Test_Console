using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test_Console.Content;

namespace Test_Console
{
    class Program
    {       
        static void Main(string[] args)
        {

            Console.WriteLine("============= Start ===============");
            //UpdateGameportal.Updatedata();
            //Update94x.Updatedata_WithpartnerID(29);
            //Update94x.Updatedata();
            //UpdateVisport.Updatedata();
            BetFolk.process();
            Console.WriteLine("============= Done ===============");
            Console.Read();
        }
    }
}
