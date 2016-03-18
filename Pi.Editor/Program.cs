using System;
using CommandLine;
using CommandLine.Text;

namespace Pi.Editor
{
    internal class Options
    {
        [Option('a', "assembly", Required = false,
          HelpText = "Components'assembly")]
        public string ComponentsAssembly { get; set; }

        [Option('o', "output",
          HelpText = "Output assembly name")]
        public string OutputFileName { get; set; }

        [Option('x', "clear", DefaultValue = false,
          HelpText = "Clear generated files")]
        public bool Clear { get; set; }

        [Option('s', "sample",  DefaultValue = false,
          HelpText = "Generate sample json files")]
        public bool GenerateSample { get; set; }

        [Option('c', "compile", DefaultValue = false,
          HelpText = "Compile to generate assembly")]
        public bool Compile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    class Program
    {
        static void Main(string[] args)
       {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Incorrect command line arguments!");
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(options.ComponentsAssembly))
                {
                    Environment.Ins.ComponentsAssembly = options.ComponentsAssembly;
                }

                if (!string.IsNullOrEmpty(options.OutputFileName))
                {
                    Environment.Ins.OutputFileName = options.OutputFileName;
                }

                if (options.GenerateSample)
                {
                    SampleGenerator.Gen();
                }
                else if (options.Clear)
                {
                    FileSys.DeleteDirectory(Environment.Ins.ScriptsPath);
                    FileSys.DeleteDirectory(Environment.Ins.DllOutputPath);
                    FileSys.DeleteDirectory(Environment.Ins.DefRoot);
                }
                else if (options.Compile)
                {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Failed, Press any key to exit!");
                Console.ReadKey();
            }
        }
    }
}
