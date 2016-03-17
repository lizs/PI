using System;
using System.Collections.Generic;

namespace Pi.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            //var x = new BlockMakerDef()
            //{
            //    Comment = "属性定义",
            //    Blocks = new List<PropertyDef>()
            //    {
            //        new PropertyDef()
            //        {
            //            PropertyId = "One",
            //            DefaultValue = "1",
            //            ItemType = "int",
            //            Type = "Settable",
            //        },
            //        new PropertyDef()
            //        {
            //            PropertyId = "Tow",
            //            DefaultValue = "1",
            //            ItemType = "int",
            //            Type = "Increasable",
            //        }
            //    }
            //};

            //FileSys.WriteToFile("Block.json", JsonSerializer.Serialize(x));
            //return;


            // generic cs
            Console.WriteLine("Generate cs...");
            CsharpGenerator.Gen();

            // generic pb
            Console.WriteLine("Generate protos...");
            ProtoGenerator.Gen();

            // compile
            Console.WriteLine("Compiling to Pi.Gen.dll...");
            Compiler.Compile();

            Console.WriteLine("Success, Press any key to exit!");
            Console.ReadKey();
        }
    }
}
