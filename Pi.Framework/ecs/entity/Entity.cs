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
using System.Collections;
using System.Collections.Generic;
using socket4net;
#if NET45
using System.Threading.Tasks;
#endif

namespace Pi.Framework
{
    public class EntityArg : UniqueObjArg<long>
    {
        public EntityArg(IObj parent, long key)
            : base(parent, key)
        {
        }
    }

    /// <summary>
    ///     �����Ϣ֪ͨ����
    /// </summary>
    public class Message
    {
    }
    
    /// <summary>
    ///     Entity(ECS)
    /// </summary>
    public interface IEntity : IUniqueObj<long>, IProperty
    {
        /// <summary>
        ///     Get component by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetComponent<T>() where T : Component;

        /// <summary>
        ///     Get component by ComponentId
        /// </summary>
        /// <param name="cpId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetComponent<T>(short cpId) where T : Component;

        /// <summary>
        ///     Listen this entity's property changed event.
        ///     when properties specified by 'pids' chanted,
        ///     entity's EventPropertyChanged event will be raised
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pids"></param>
        void Listen(Action<IEntity, IBlock> handler, params short[] pids);

        /// <summary>
        ///     Unlisten
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pids"></param>
        void Unlisten(Action<IEntity, IBlock> handler, params short[] pids);

        /// <summary>
        ///     Send a message to this entity
        /// </summary>
        /// <param name="msg"></param>
        void SendMessage(Message msg);
    }

    public partial class Entity : UniqueObj<long>, IEntity, IEnumerable<Component>
    {
        /// <summary>
        ///     �������
        /// </summary>
        protected virtual void SpawnComponents()
        {
            AddComponent<PropertyComponent>();
        }
        
        #region ��ʼ����ж��

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <param name="arg"></param>
        protected override void OnInit(ObjArg arg)
        {
            base.OnInit(arg);

            // ������
            SpawnComponents();

            // �����������������Ϊ�գ�
            _property = GetComponent<PropertyComponent>();
        }

        /// <summary>
        ///     ����
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            // �����ʼ
            Components.Start();
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void OnBorn()
        {
            base.OnBorn();

            // �������
            Components.Born();
        }

        /// <summary>
        ///     ж��
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // �������
            Components.Destroy();
        }

        #endregion

        public void SendMessage(Message msg)
        {
            OnMessage(msg);

            foreach (var component in Components)
            {
                component.OnMessage(msg);
            }
        }

        public virtual void OnMessage(Message msg)
        {
        }

#if NET45
        public virtual Task<NetResult> OnRequest(short ops, byte[] data)
        {
            return Task.FromResult(NetResult.Failure);
        }

        public virtual Task<bool> OnPush(short ops, byte[] data)
        {
            return Task.FromResult(false);
        }
#else
        /// <summary>
        ///     Handle request
        /// </summary>
        /// <param name="ops"></param>
        /// <param name="data"></param>
        /// <param name="cb"></param>
        public virtual void OnRequest(short ops, byte[] data, Action<NetResult> cb)
        {
            cb(NetResult.Failure);
        }
        
        /// <summary>
        ///     Handle push
        /// </summary>
        /// <param name="ops"></param>
        /// <param name="data"></param>
        /// <param name="cb"></param>
        public virtual void OnPush(short ops, byte[] data, Action<bool> cb)
        {
            cb(false);
        }
#endif

        #region �����ʵ��

        private ComponentsMgr _components;
        public ComponentsMgr Components
        {
            get
            {
                return _components ??
                       (_components =
                           New<ComponentsMgr>(new UniqueMgrArg(this)));
            }
        }

        public Component GetComponent(short key)
        {
            return Components.Get(key);
        }

        public bool RemoveComponent(short key)
        {
            return Components.Destroy(key);
        }

        public T GetComponent<T>() where T : Component
        {
            return Components.GetFirst<T>();
        }

        public T GetComponent<T>(short cpId) where T : Component
        {
            return Components.Get(cpId) as T;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            return Components.Get<T>();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            return Components.AddComponent<T>();
        }

        public Component AddComponent(Type cpType)
        {
            return Components.AddComponent(cpType);
        }

        public Component AddComponent(short cpId)
        {
            return Components.AddComponent(cpId);
        }

        public List<short> RemoveComponent<T>() where T : Component
        {
            return Components.Destroy<T>();
        }

        public IEnumerator GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator<Component> IEnumerable<Component>.GetEnumerator()
        {
            return (IEnumerator<Component>)GetEnumerator();
        }

        #endregion

        #region ����

        public void Listen(Action<IEntity, IBlock> handler, params short[] pids)
        {
            _property.Listen(handler, pids);
        }

        public void Unlisten(Action<IEntity, IBlock> handler, params short[] pids)
        {
            _property.Unlisten(handler, pids);
        }

        #endregion
    }
}