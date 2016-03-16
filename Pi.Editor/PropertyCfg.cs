using Newtonsoft.Json;

namespace Pi.Editor
{
    public enum EPropertyType
    {
        Settable,
        Increasable,
        List,
    }

    /// <summary>
    ///     属性配置
    /// </summary>
    public class PropertyCfg
    {
        /// <summary>
        ///     属性Id
        /// </summary>
        public short PropertyId { get; set; }

        /// <summary>
        ///     属性类型
        /// </summary>
        public EPropertyType Type { get; set; }
        
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