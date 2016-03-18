using Pi.Framework;
using System;
using System.Collections.Generic;
namespace Pi.Gen
{
    /// <summary>
    ///     属性定义
    /// </summary>
	public static class BlockMaker
    {
        public static IBlock Create(short pid)
        {
            return Create((EPid) pid);
        }

        public static IBlock Create(EPid epid)
        {
            if (!Enum.IsDefined(typeof(EPid), epid))
                throw new ArgumentException("epid");

            if (Blocks.ContainsKey(epid))
                return Blocks[epid];

            throw new NotImplementedException(epid.ToString());
        }

		private static readonly Dictionary<EPid, IBlock> Blocks = new Dictionary<EPid, IBlock>()
        {
				{EPid.One, new SettableBlock<int>((short)EPid.One, 0, EBlockMode.Synchronizable)},
				{EPid.Two, new IncreasableBlock<int>((short)EPid.Two, 0, EBlockMode.Synchronizable, 0, 0)},
				{EPid.Three, new ListBlock<string>((short)EPid.Three, new string[]{"1", "2"}, EBlockMode.Synchronizable)},

        }; 
    }
}

	