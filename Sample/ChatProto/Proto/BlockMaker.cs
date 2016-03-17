#region MIT
//  /*The MIT License (MIT)
// 
//  Copyright 2016 lizs lizs4ever@163.com
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//   * */
#endregion
using System;
using System.Collections.Generic;
using Pi.Framework;

namespace Shared
{
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
            {EPid.One, new SettableBlock<int>((short)EPid.One, 1, EBlockMode.Synchronizable)},
            {EPid.Two, new IncreasableBlock<int>((short)EPid.Two, 2, EBlockMode.Synchronizable, 0, 10)},
            {EPid.Three, new ListBlock<int>((short)EPid.Three, new []{1, 2, 3}, EBlockMode.Synchronizable)},
        }; 
    }
}
