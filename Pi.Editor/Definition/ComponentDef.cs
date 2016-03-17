using Newtonsoft.Json;

namespace Pi.Editor
{
    /// <summary>
    ///     �������
    /// </summary>
    public class ComponentDef
    {
        /// <summary>
        ///     ���Id
        /// </summary>
        public short ComponentId { get; set; }

        /// <summary>
        ///     �����
        ///     ��ʽ�磺"namespace.class, assembly"
        /// </summary>
        public string Class { get; set; }
        
        /// <summary>
        ///     ���л�
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}