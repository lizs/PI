#C# game server framework(based on [socket4net](https://github.com/lizs/Socket4Net))

####Features

* Easy to use
* Distributed
* Automatic codes generation supported
* Configurable

####Framework
Pi consists of 3 parts:`Node`/`ECS`/`Editor`
#####Node
Your server cluster may have multiple processes to work together, every process can perform as a server or an client or both.In pi, We call a server or an client as a `Node`. So, one process may have serveral `Nodes`, they are managed by `NodesMgr`. <br>

`Node` can communicat with others(remote) by a set of `Push/RequestAsyn` interfaces. `Node` and `NodesMgr` has their own configuration(standard xml), for example:
```xml
  <!--single node-->
  <node name="Chat server"
    type="Chat"
    guid="{12B3E06E-6CFF-4BC8-8DF9-67F03273C299}"
    ip="127.0.0.1"
    port="6001"
    pwd="">
  </node>
  
  <!--multiple server nodes-->
  <servers>
    <node .../>
    ...
  </servers>
  
  <!--multiple client nodes-->
  <clients>
    <node .../>
    ...
  </clients>
```
You can also config `Redis` nodes, for example:
```xml
  <redisnodes>
    <node name="Redis client"
    type="Redis"
    guid="{D49A172A-00DD-4E8D-AB21-490F35A560A2}"
    ip="127.0.0.1"
    port="6379"
    pwd="" />
  </redisnodes>
```

#####ECS
#####Editor
######Protocol define
######Entity define

####Question
QQ group：[Click to join](http://jq.qq.com/?_wv=1027&k=VptNja)<br>
e-mail : lizs4ever@163.com
