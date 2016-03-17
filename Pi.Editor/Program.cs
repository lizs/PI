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

            var entity = new EntityDef()
            {
                Name = "MyEntity",
                Class = "namespace.class, assembly",
                Components = new List<ComponentDef>()
                {
                    new ComponentDef() {Class = "namespace.class1, assembly", ComponentId = 1},
                    new ComponentDef() {Class = "namespace.class2, assembly", ComponentId = 2},
                    new ComponentDef() {Class = "namespace.class3, assembly", ComponentId = 3},
                    new ComponentDef() {Class = "namespace.class4, assembly", ComponentId = 4},
                },

                Properties = new List<PropertyDef>()
                {
                    new PropertyDef() {PropertyId = 1, Type = EPropertyType.Increasable},
                    new PropertyDef() {PropertyId = 2, Type = EPropertyType.Settable},
                    new PropertyDef() {PropertyId = 3, Type = EPropertyType.List},
                    new PropertyDef() {PropertyId = 4, Type = EPropertyType.Increasable},
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
                var deserializedEntity = JsonConvert.DeserializeObject<EntityDef>(fr.ReadToEnd());
            }

            Console.ReadLine();
        }
    }
}
