using System;
using System.Collections.Generic;

namespace Pi.Framework
{
    /// <summary>
    /// Message handler
    /// </summary>
    public class MessageHandler
    {
        private readonly Dictionary<Type, Action<Message>> _messageHandlers = new Dictionary<Type, Action<Message>>();

        /// <summary>
        ///  register a handler for Message : T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Register<T>(Action<Message> handler) where T : Message
        {
            if (!_messageHandlers.ContainsKey(typeof(T)))
            {
                _messageHandlers.Add(typeof(T), handler);
            }
            else
                throw new Exception("Handler already exist for " + typeof(T));
        }

        /// <summary>
        ///  register a handler for Message : T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Deregister<T>(Action<Message> handler) where T : Message
        {
            if (_messageHandlers.ContainsKey(typeof(T)))
            {
                _messageHandlers.Remove(typeof(T));
            }
        }

        /// <summary>
        ///     clear the registered handlers
        /// </summary>
        public void Clear()
        {
            _messageHandlers.Clear();
        }

        /// <summary>
        ///     dispatch the comming message
        /// </summary>
        /// <param name="msg"></param>
        public void Dispatch(Message msg)
        {
            if(msg == null) return;
            var type = msg.GetType();
            if (_messageHandlers.ContainsKey(type))
            {
                _messageHandlers[type](msg);
            }
        }
    }
}