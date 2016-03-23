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
            // google protobuf message
            FileSys.WriteToFile(Path.Combine(Environment.Ins.ProtosDefPath, "echo.proto"),
@"message EchoProto
{
    optional string Message = 1;  
}");
            FileSys.WriteToFile(Path.Combine(Environment.Ins.ProtosDefPath, "broadcast.proto"),
@"message BroadcastProto
{
    optional string Message = 1;  
}");

            // 枚举
            var pid = new EnumDef()
            {
                Comment = "属性Id",
                Name = "EPid",
                Values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("One", ""),
                    new KeyValuePair<string, string>("Two", ""),
                    new KeyValuePair<string, string>("Three", ""),
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, pid.Name + ".json"),
                JsonSerializer.Serialize(pid));

            // 枚举
            var componentsId = new EnumDef()
            {
                Comment = "组件Id",
                Name = "EComponentId",
                Values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("SampleComponent", "1"),
                    new KeyValuePair<string, string>("ModifierComponent", "2"),
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, componentsId.Name + ".json"),
                JsonSerializer.Serialize(componentsId));
            
            // ops
            var ops = new EnumDef()
            {
                Comment = "玩家操作码",
                Name = "EOps",
                Values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Create", ""),
                    new KeyValuePair<string, string>("Destroy", ""),
                    new KeyValuePair<string, string>("Echo", ""),
                    new KeyValuePair<string, string>("Broadcast", ""),
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, ops.Name + ".json"),
                JsonSerializer.Serialize(ops));


            // non-player ops
            var nonPlayerOps = new EnumDef()
            {
                Comment = "非玩家操作码",
                Name = "ENonPlayerOps",
                Values = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("CreatePlayer", "-1"),
                }
            };
            FileSys.WriteToFile(Path.Combine(Environment.Ins.EnumsDefPath, nonPlayerOps.Name + ".json"),
                JsonSerializer.Serialize(nonPlayerOps));

            // 常量
            var consts = new ConstDef()
            {
                Comment = "Int32常量定义",
                ClassName = "Ints",
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
                Name = "Ship",
                Components = new List<string>()
                {
                    "ModifierComponent",
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
