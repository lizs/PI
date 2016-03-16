using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pi.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "test.json";

            var entity = new EntityCfg()
            {
                Name = "MyEntity",
                Components = new List<ComponentCfg>()
                {
                    new ComponentCfg() {Class = "namespace.class1, assembly", ComponentId = 1},
                    new ComponentCfg() {Class = "namespace.class2, assembly", ComponentId = 2},
                    new ComponentCfg() {Class = "namespace.class3, assembly", ComponentId = 3},
                    new ComponentCfg() {Class = "namespace.class4, assembly", ComponentId = 4},
                },

                Properties = new List<PropertyCfg>()
                {
                    new PropertyCfg() {PropertyId = 1, Type = EPropertyType.Increasable},
                    new PropertyCfg() {PropertyId = 2, Type = EPropertyType.Settable},
                    new PropertyCfg() {PropertyId = 3, Type = EPropertyType.List},
                    new PropertyCfg() {PropertyId = 4, Type = EPropertyType.Increasable},
                }
            };

            using (var sm = new FileStream(path, FileMode.Create))
            using(var fw = new StreamWriter(sm))
            {
                fw.Write(JsonConvert.SerializeObject(entity, Formatting.Indented));
            }

            using (var sm = new FileStream(path, FileMode.Open))
            using (var fr = new StreamReader(sm))
            {
                var deserializedEntity = JsonConvert.DeserializeObject<EntityCfg>(fr.ReadToEnd());
            }

            Console.ReadLine();
        }
    }
}
