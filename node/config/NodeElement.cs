﻿#region MIT
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
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace Pi.node
{
    public class ClientNodeElement : NodeElement
    {
        [ConfigurationProperty("autoreconnect",
            DefaultValue = true,
            IsRequired = true)]
        public bool AutoReconnect
        {
            get { return (bool)this["autoreconnect"]; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(base.ToString());
            sb.AppendLine("AutoReconnect : " + AutoReconnect);
            return sb.ToString();
        }
    }

    public class ServerNodeElement : NodeElement
    {
        /// <summary>
        ///     代理Ip
        /// </summary>
        [ConfigurationProperty("proxyip",
            DefaultValue = "",
            IsRequired = false)]
        public string ProxyIp
        {
            get { return (string)this["proxyip"]; }
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(base.ToString());
            sb.AppendLine("ProxyIp : " + ProxyIp);
            return sb.ToString();
        }
    }

    public class NodeElement : ConfigElement
    {
        /// <summary>
        ///     唯一Id
        /// </summary>
        [ConfigurationProperty("guid",
            IsRequired = true)]
        public Guid Guid
        {
            get { return (Guid)this["guid"]; }
        }

        /// <summary>
        ///     节点类型
        /// </summary>
        [ConfigurationProperty("type",
            IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }

        /// <summary>
        ///     节点类
        ///     注： 
        ///     若显示指定，则用指定类创建节点，否则根据上下文自行创建(ServerNode<TSession> or ClientNode<TSession>)
        /// </summary>
        [ConfigurationProperty("class",
            DefaultValue = null,
            IsRequired = false)]
        [TypeConverter(typeof(TypeNameConverter))]
        public Type NodeClass
        {
            get { return (Type)this["class"]; }
        }

        /// <summary>
        ///     ip
        /// </summary>
        [ConfigurationProperty("ip",
            IsRequired = true)]
        public string Ip
        {
            get { return (string)this["ip"]; }
        }

        /// <summary>
        ///     端口
        /// </summary>
        [ConfigurationProperty("port",
            IsRequired = true)]
        public ushort Port
        {
            get { return (ushort)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("pwd",
                DefaultValue = "",
               IsRequired = false)]
        public string Pwd
        {
            get { return (string)this["pwd"]; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("Guid : {0}\r\n", Guid);
            sb.AppendFormat("Type : {0}\r\n", Type);
            sb.AppendFormat("Ip : {0}\r\n", Ip);
            sb.AppendFormat("Port : {0}\r\n", Port);
            sb.AppendFormat("Class : {0}\r\n", NodeClass);
            sb.AppendFormat("Pwd : {0}", Pwd);

            return sb.ToString();
        }
    }
}