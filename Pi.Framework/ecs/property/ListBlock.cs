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
using System.Text;
using socket4net;

namespace Pi.Framework
{
    public class ListBlock<TItem> : Block<List<ListItemRepresentation<TItem>>>,
        IListBlock<TItem>
    {
        private int _uidSeed;
        private int GetUid()
        {
            return ++_uidSeed;
        }

        public ListBlock(short id, List<TItem> value, EBlockMode mode)
            : base(id, null, mode)
        {
            Value = new List<ListItemRepresentation<TItem>>();
            if (value.IsNullOrEmpty()) return;

            MultiAdd(value);
        }

        public override string ToString()
        {
            var lst = Get().Select(x=>x.Item).ToList();
            var sb = new StringBuilder();
            sb.AppendLine(Id.ToString());
            lst.ForEach(x =>
            {
                var tmp = (object) x;
                sb.AppendLine(tmp == null ? "null" : x.ToString());
            });

            var info = sb.ToString();
            if (info.Length == 0)
                info = "���б�";
            return info;
        }

        /// <summary>
        ///     ����ջ�������¼Push֮ǰ�������б����
        /// </summary>
        private readonly List<ListOpsRepresentation> _opsStack =
            new List<ListOpsRepresentation>();

        private bool _dirty;
        public override bool Dirty
        {
            get { return _dirty; }
            set
            {
                if (_dirty == value) return;
                _dirty = value;
                if (!_dirty)
                    _opsStack.Clear();
            }
        }

        public Type ItemType
        {
            get { return typeof (TItem); }
        }

        public override EBlockType EBlockType
        {
            get { return EBlockType.List; }
        }

        private void Record(ListOpsRepresentation ops)
        {
            Ops = Ops;
            Dirty = true;
#if NET45
            var ignore = false;
            switch (ops.Ops)
            {
                case EPropertyOps.Remove:
                {
                    // �ɵ��������ͬ��
                    var cnt = _opsStack.RemoveAll(x => x.Id == ops.Id);
                    ignore = cnt > 0;
                    break;
                }
            }

            if(!ignore)
                _opsStack.Add(ops);
#else
            // �ͻ��˲���¼����
#endif
        }

        public int IndexOf(Predicate<TItem> condition)
        {
            var lst = Get();
            return lst.FindIndex(x => condition(x.Item));
        }

        public int IndexOf(TItem item)
        {
            var lst = Get();
            return lst.FindIndex(x => Equals(x.Item, item));
        }

        private int GetIndexById(int id)
        {
            var lst = Get();
            var item = lst.Find(x => x.Id == id);
            if (item == null) return InvalidIndex;
            return lst.IndexOf(item);
        }

        public void Add(object item)
        {
            Add((TItem) item);
        }

        public void Add(TItem item)
        {
            Insert(Get().Count, item);
        }

        public bool Remove(object item)
        {
            return Remove((TItem) item);
        }

        public bool Remove(TItem item)
        {
            var idx = IndexOf(item);
            return RemoveAt(idx);
        }

        public bool RemoveAt(int idx)
        {
            var lst = Get();
            if (idx < 0 || idx >= lst.Count) return false;

            var removed = lst[idx];
            lst.RemoveAt(idx);

            Record(new ListRemoveOps(removed.Id));
            return true;
        }

        public bool Insert(int idx, object item)
        {
            return Insert(idx, (TItem) item);
        }

        public bool Insert(int idx, TItem item)
        {
            var lst = Get();
            if (idx < 0 || idx > lst.Count) return false;

            var rep = new ListItemRepresentation<TItem> { Id = GetUid(), Item = item };
            var ops = new ListInsertOps(rep.Id, idx);
            lst.Insert(idx, rep);

            Record(ops);
            return true;
        }

#if NET35
        public bool Insert(int idx, int id, TItem item)
        {
            var lst = Get();
            if (idx < 0 || idx > lst.Count) return false;

            var rep = new ListItemRepresentation<TItem> { Id = id, Item = item };
            var ops = new ListInsertOps(rep.Id, idx);
            lst.Insert(idx, rep);

            Record(ops);
            return true;
        }
#endif

        public bool Replace(int idx, TItem item)
        {
            var lst = Get();
            if (idx < 0 || idx >= lst.Count) return false;

            var updated = lst[idx];
            updated.Item = item;

            Record(new ListUpdateOps(updated.Id));
            return true;
        }

        public bool Swap(int idxA, int idxB)
        {
            var lst = Get();
            if (idxA < 0 || idxA >= lst.Count || idxB < 0 || idxB >= lst.Count) return false;

            var updatedA = lst[idxA];
            var updatedB = lst[idxB];

            var item = updatedA.Item;
            updatedA.Item = updatedB.Item;
            updatedB.Item = item;

            Record(new ListUpdateOps(updatedA.Id));
            Record(new ListUpdateOps(updatedB.Id));
            return true;
        }

        public bool Update(int idx)
        {
            var lst = Get();
            if (idx < 0 || idx >= lst.Count) return false;

            var updated = lst[idx];
            Record(new ListUpdateOps(updated.Id));

            return true;
        }

        public void MultiAdd(List<TItem> items)
        {
            if (items.IsNullOrEmpty()) return;
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void MultiRemove(List<TItem> items)
        {
            if (items.IsNullOrEmpty()) return;

            foreach (var idx in items.Select(IndexOf).Where(idx => idx != -1))
            {
                RemoveAt(idx);
            }
        }

        public int RemoveAll(Predicate<TItem> predicate)
        {
            var toBeRemoved = Get().FindAll(x => predicate(x.Item)).Select(x => x.Item).ToList();
            if (toBeRemoved.Count == 0) return 0;

            MultiRemove(toBeRemoved);
            return toBeRemoved.Count;
        }

        public TItem GetByIndex(int idx)
        {
            var lst = Get();
            if (idx >= 0 && idx < lst.Count)
            {
                return lst[idx].Item;
            }
            return default(TItem);
        }

        public int RemoveAll()
        {
            var lst = Get();
            var cnt = lst.Count;

            while (lst.Count > 0)
                RemoveAt(0);

            return cnt;
        }

        public override byte[] Serialize()
        {
            var lst = Get().Select(x => x.Item).ToList();
            return PiSerializer.SerializeValue(lst);
        }

        public override bool Deserialize(byte[] data)
        {
            if (data.IsNullOrEmpty()) return false;

            Get().Clear();
            var lst = PiSerializer.DeserializeValue<List<TItem>>(data);
            if (lst == null || lst.Count <= 0) return false;

            MultiAdd(lst);
            return true;
        }

        /// <summary>
        ///     �б������л�
        /// </summary>zx
        /// <returns></returns>
        public byte[] SerializeDifference()
        {
            if (_opsStack.Count == 0) return null;
            var proto = new ListOpsProto<TItem> { OpsStack = new List<KeyValuePair<EPropertyOps, byte[]>>() };
            _opsStack.ForEach(
                ops =>
                {
                    switch (ops.Ops)
                    {
                        case EPropertyOps.Insert:
                            {
                                var insert = ops as ListInsertOps;
                                var item = Get().Find(x => x.Id == insert.Id);
                                if (item != null)
                                {
                                    proto.OpsStack.Add(new KeyValuePair<EPropertyOps, byte[]>(EPropertyOps.Insert,
                                        (new InsertOpsProto<TItem> { Index = insert.Index, ListItem = item }).Serialize()));
                                }
                                break;
                            }

                        case EPropertyOps.Remove:
                            {
                                var rm = ops as ListRemoveOps;
                                proto.OpsStack.Add(new KeyValuePair<EPropertyOps, byte[]>(EPropertyOps.Remove,
                                    (new RemoveOpsProto { Id = rm.Id }).Serialize()));
                                break;
                            }

                        case EPropertyOps.Update:
                            {
                                var update = ops as ListUpdateOps;
                                var item = Get().Find(x => x.Id == update.Id);
                                proto.OpsStack.Add(new KeyValuePair<EPropertyOps, byte[]>(EPropertyOps.Update,
                                    (new UpdateOpsProto<TItem> { ListItem = item }).Serialize()));
                                break;
                            }
                    }
                });

            return PiSerializer.Serialize(proto);
        }

        /// <summary>
        ///     �б��ַ����л�
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public bool DeserializeDifference(byte[] bytes)
        {
            if (bytes.IsNullOrEmpty()) return true;
            var proto = PiSerializer.Deserialize<ListOpsProto<TItem>>(bytes);
            if (proto.OpsStack == null || proto.OpsStack.Count == 0) return true;

            proto.OpsStack.ForEach(o =>
            {
                switch (o.Key)
                {
                    case EPropertyOps.Insert:
                        {
                            var x = InsertOpsProto<TItem>.Deserialize(o.Value);
                            if (x == null)
                                Logger.Ins.Error("�����itemΪ��");
                            else if (x.ListItem == null)
                                Logger.Ins.Error("�����itemΪ�գ�λ�ã�{0}", x.Index);
                            else
                            {
#if NET35
                                Insert(x.Index, x.ListItem.Id, x.ListItem.Item);
#else
                                Insert(x.Index, x.ListItem.Item);
#endif
                            }
                            break;
                        }

                    case EPropertyOps.Remove:
                        {
                            var x = RemoveOpsProto.Deserialize(o.Value);
                            if (x == null)
                                Logger.Ins.Error("�Ƴ�itemʧ�ܣ����ԣ�{0}", Id);
                            else
                            {
                                var idx = GetIndexById(x.Id);
                                if (idx == InvalidIndex || !RemoveAt(idx))
                                    Logger.Ins.Error("�Ƴ�itemʧ�ܣ�δ�ҵ���̬idΪ {0} ��item", x.Id);
                            }
                            break;
                        }

                    case EPropertyOps.Update:
                        {
                            var x = UpdateOpsProto<TItem>.Deserialize(o.Value);
                            if(x == null || x.ListItem == null)
                                Logger.Ins.Warn("��������Ϊ��");
                            else
                            {
                                var updated = Get().Find(y => y.Id == x.ListItem.Id);
                                if (updated != null)
                                {
                                    updated.Item = x.ListItem.Item;
                                }
                                else
                                    Logger.Ins.Error("���µ�item�����ڣ���̬id : {0}", x.ListItem.Id);
                            }
                          
                            break;
                        }
                }
            });

            return true;
        }
    }
}