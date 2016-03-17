using System.Collections.Generic;
using Newtonsoft.Json;

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
        ///     名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     类
        ///     如："namespace.class, assembly"
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        ///     属性
        /// </summary>
        public List<PropertyDef> Properties { get; set; }

        /// <summary>
        ///     组件
        /// </summary>
        public List<ComponentDef> Components { get; set; }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
