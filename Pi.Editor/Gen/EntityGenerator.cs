using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using socket4net;

namespace Pi.Editor
{
    /// <summary>
    ///     实体配置（通过文本定义实体）
    ///     1、组件
    ///     2、属性
    /// </summary>
    internal class EntityDef
    {
        /// <summary>
        ///    注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     属性
        /// </summary>
        public List<string> Properties { get; set; }
        
        /// <summary>
        ///     组件
        /// </summary>
        public List<string> Components { get; set; }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    /// <summary>
    ///     枚举文件生成
    /// </summary>
    internal class EntityGenerator : Generator
    {
        public EntityGenerator(string defPath)
            : base(defPath)
        {
        }

        public override void Gen()
        {
            var sb = new StringBuilder();
            var template = TemplateProvider.Ins.Get(EDefType.Entity);
            var def = JsonSerializer.Deserialize<EntityDef>(DefinitionPath);

            // inject block
            var blocks = new StringBuilder();
            def.Properties.ForEach(x => blocks.AppendFormat("{1}BlockMaker.Create(EPid.{0}),\r\n", x, Spaces16));

            // add component
            var components = new StringBuilder();
            def.Components.ForEach(
                x => components.AppendFormat("{1}AddComponent((short)EComponentId.{0});\r\n", x, Spaces12));

            // property(get/set)
            var properties = new StringBuilder();
            def.Properties.ForEach(x => properties.AppendLine(BlockCache.Ins.Get(x)));


            sb.AppendFormat(template, def.Comment, def.Name, blocks, components, properties);

            FileSys.WriteToFile(Path.Combine(Environment.Ins.ScriptsPath, def.Name + ".cs"), sb.ToString());
        }
    }
}
