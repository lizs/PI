using System.Text;
using ProtoBuf;
using socket4net;

namespace Pi.Framework
{
    [ProtoContract]
    public class DataProtocol : IDataProtocol
    {
        [ProtoMember(1)]
        public byte TargetNode { get; set; }

        [ProtoMember(2)]
        public short Ops { get; set; }

        [ProtoMember(3)]
        public long PlayerId { get; set; }

        [ProtoMember(4)]
        public long ObjId { get; set; }

        [ProtoMember(5)]
        public short ComponentId { get; set; }

        [ProtoMember(6)]
        public byte[] Data { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("TargetNode : " + TargetNode);
            sb.AppendLine("Ops : " + Ops);
            sb.AppendLine("PlayerId : " + PlayerId);
            sb.AppendLine("ObjId : " + ObjId);
            sb.AppendLine("ComponentId : " + ComponentId);
            sb.AppendFormat("Data : {0}bytes\r\n", Data.IsNullOrEmpty() ? 0 : Data.Length);

            return sb.ToString();
        }
    }
}