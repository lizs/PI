using System.Collections.Generic;
using System.IO;

namespace Pi.Editor
{
    /// <summary>
    ///     用例json文件生成
    /// </summary>
    internal class SampleGenerator
    {
        public static void Gen()
        {
            // 枚举
            FileSys.WriteToFile(Path.Combine(Environment.Ins.ProtosDefPath, "message.proto"),
@"message MyProto
{
    optional string Message = 1;  
}");

            // 枚举
            var pid = new EnumDef()
            {
                Comment = "属性Id",
                Name = "EPid",
                Values = new List<string>()
                {
                    "One",
                    "Two",
                    "Three",
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, pid.Name + ".json"),
                JsonSerializer.Serialize(pid));

            // 枚举
            var componentsId = new EnumDef()
            {
                Comment = "组件Id",
                Name = "EComponentId",
                Values = new List<string>()
                {
                    "One",
                    "Two",
                    "Three",
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, componentsId.Name + ".json"),
                JsonSerializer.Serialize(componentsId));

            // 常量
            var consts = new ConstDef()
            {
                Comment = "Int32常量定义",
                ClassName = "IntConsts",
                ValueType = "int",
                Values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("One", "1"),
                    new KeyValuePair<string, string>("Two", "2"),
                    new KeyValuePair<string, string>("Three", "3"),
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.ConstsDefPath, consts.ClassName + ".json"),
                JsonSerializer.Serialize(consts));
            
            // 属性块
            var blocks = new BlocksDef
            {
                Comment = "属性定义",
                Blocks = new List<BlockDef>()
                {
                    new BlockDef()
                    {
                        PropertyId = "One",
                        Type = "Settable",
                        ItemType = "int",
                        DefaultValue = "0",
                    },
                    new BlockDef()
                    {
                        PropertyId = "Two",
                        Type = "Increasable",
                        ItemType = "int",
                        DefaultValue = "0",
                        Min = "0",
                        Max = "100",
                    },
                    new BlockDef()
                    {
                        PropertyId = "Three",
                        Type = "List",
                        ItemType = "string",
                        DefaultValue = "new string[]{\"1\", \"2\"}",
                    }
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.BlocksDefPath, "blocks.json"),
                JsonSerializer.Serialize(blocks));

            // 实体
            var entity = new EntityDef()
            {
                Comment = "实体定义",
                Name = "MyEntity",
                Components = new List<string>()
                {
                    "Shared.SampleComponentBase, Shared",
                },
                Properties = new List<string>()
                {
                    "One", "Two", "Three"
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EntitiesDefPath, entity.Name + ".json"),
                JsonSerializer.Serialize(entity));
        }
    }
}
