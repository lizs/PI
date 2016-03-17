
namespace Pi.Editor
{
    /// <summary>
    ///     cs文件生成器
    /// </summary>
    internal abstract class CodeGenerator
    {
        public string DefinitionPath { get; private set; }
        protected CodeGenerator(string defPath)
        {
            DefinitionPath = defPath;
        }

        public abstract void Gen();
    }

    internal class CodeGeneratorFactory
    {
        public static CodeGenerator Create(EDefType type, string defPath)
        {
            switch (type)
            {
                case EDefType.Enum:
                    return new EnumGenerator(defPath);

                case EDefType.Const:
                    return new ConstGenerator(defPath);

                case EDefType.Block:
                    return new BlockMakerGenerator(defPath);

                default:
                    return null;
            }
        }
    }
}