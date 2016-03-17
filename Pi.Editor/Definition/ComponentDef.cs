using Newtonsoft.Json;

namespace Pi.Editor
{
    /// <summary>
    ///     组件配置
    /// </summary>
    public class ComponentDef
    {
        /// <summary>
        ///     组件Id
        /// </summary>
        public short ComponentId { get; set; }

        /// <summary>
        ///     组件类
        ///     格式如："namespace.class, assembly"
        /// </summary>
        public string Class { get; set; }
        
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