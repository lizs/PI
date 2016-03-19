
namespace Pi.Editor
{
    /// <summary>
    ///     cs文件生成器
    /// </summary>
    internal abstract class Generator
    {
        public const string Spaces8 = "        ";
        public const string Spaces4 = "    ";
        public const string Spaces6 = "      ";
        public const string Spaces12 = "            ";
        public const string Spaces16 = "                ";
        public string DefinitionPath { get; private set; }
        protected Generator(string defPath)
        {
            DefinitionPath = defPath;
        }

        public abstract void Gen();
    }

    internal class GeneratorFactory
    {
        public static Generator Create(EDefType type, string defPath)
        {
            switch (type)
            {
                case EDefType.Enum:
                    return new EnumGenerator(defPath);

                case EDefType.Const:
                    return new ConstGenerator(defPath);

                case EDefType.Block:
                    return new BlocksGenerator(defPath);

                case EDefType.Entity:
                    return new EntityGenerator(defPath);

                default:
                    return null;
            }
        }
    }
}