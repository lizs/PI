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
using System.Linq;
using socket4net;

namespace Pi.Framework
{
    /// <summary>
    ///     E(ECS)
    ///     ���ݲ���
    /// </summary>
    public partial class Entity
    {
        private PropertyComponent _property;

        public IReadOnlyCollection<IBlock> Blocks
        {
            get { return _property.Blocks.ToList(); }
        }

        public IBlock GetBlock(short key)
        {
            return _property.GetBlock(key);
        }

        public void Inject(IEnumerable<IBlock> blocks)
        {
            _property.Inject(blocks);
        }

        public bool Apply(IReadOnlyCollection<IBlock> blocks)
        {
            return _property.Apply(blocks);
        }
        
        /// <summary>
        ///     Ӧ������
        /// </summary>
        /// <param name="properties"></param>
        public void Apply(IEnumerable<Pair<short, byte[]>> properties)
        {
            if (properties == null) return;

            foreach (var kv in properties.Where(kv => kv.Value != null))
            {
                var block = GetBlock(kv.Key);
                if (block != null)
                {
                    block.Deserialize(kv.Value);
                }
                else
                    Logger.Ins.Error("Block of {0} not injected for {1}", kv.Key, Name);
            }
        }


        public T Get<T>(short id)
        {
            return _property.Get<T>(id);
        }

        public bool Get<T>(short id, out T value)
        {
            return _property.Get<T>(id, out value);
        }

        public List<T> GetList<T>(short id)
        {
            return _property.GetList<T>(id);
        }

        public bool Inject(IBlock block)
        {
            return _property.Inject(block);
        }

        public bool Inc<T>(short id, T delta)
        {
            return _property.Inc(id, delta);
        }

        public bool Inc(short id, object delta)
        {
            return _property.Inc(id, delta);
        }

        public bool Inc<T>(short id, T delta, out T overflow)
        {
            return _property.Inc(id, delta, out overflow);
        }

        public bool Inc(short id, object delta, out object overflow)
        {
            return _property.Inc(id, delta, out overflow);
        }

        public bool IncTo<T>(short id, T target)
        {
            return _property.IncTo(id, target);
        }

        public bool IncTo(short id, object target)
        {
            return _property.IncTo(id, target);
        }

        public bool Set<T>(short id, T value)
        {
            return _property.Set(id, value);
        }

        public bool Set(short id, object value)
        {
            return _property.Set(id, value);
        }

        public int IndexOf<T>(short pid, T item)
        {
            return _property.IndexOf(pid, item);
        }

        public int IndexOf<T>(short pid, Predicate<T> condition)
        {
            return _property.IndexOf(pid, condition);
        }

        public T GetByIndex<T>(short pid, int idx)
        {
            return _property.GetByIndex<T>(pid, idx);
        }

        public bool Add<T>(short id, T value)
        {
            return _property.Add(id, value);
        }

        public bool Add(short id, object value)
        {
            return _property.Add(id, value);
        }

        public bool AddRange<T>(short id, List<T> items)
        {
            return _property.AddRange(id, items);
        }

        public bool Remove<T>(short id, T item)
        {
            return _property.Remove(id, item);
        }

        public bool Remove(short id, object item)
        {
            return _property.Remove(id, item);
        }

        public bool RemoveAll<T>(short id, Predicate<T> predicate)
        {
            return _property.RemoveAll(id, predicate);
        }

        public bool RemoveAll<T>(short id, Predicate<T> predicate, out int count)
        {
            return _property.RemoveAll(id, predicate, out count);
        }

        public bool RemoveAll(short id)
        {
            return _property.RemoveAll(id);
        }

        public bool RemoveAll(short id, out int count)
        {
            return _property.RemoveAll(id, out count);
        }

        public bool Insert<T>(short id, int idx, T item)
        {
            return _property.Insert(id, idx, item);
        }

        public bool Insert(short id, int idx, object item)
        {
            return _property.Insert(id, idx, item);
        }

        public bool Replace<T>(short id, int idx, T item)
        {
            return _property.Replace(id, idx, item);
        }

        public bool Swap<T>(short id, int idxA, int idxB)
        {
            return _property.Swap<T>(id, idxA, idxB);
        }
    }
}