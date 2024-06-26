using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BinaryFormatterTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //This will fail because _xmlReader is not marked as [NonSerialized]
            CaseWithFailClass();
            //This will pass because _xmlReader is marked as [NonSerialized]
            CaseWIthFixClass();
        }

        private static void CaseWithFailClass()
        {
            FailClass failClass = Deserialize(new FailClass { Name = "Hello", Id = 123 });
            failClass.Test();
        }

        private static void CaseWIthFixClass()
        {
            FixClass fixClass = Deserialize(new FixClass { Name = "Hello", Id = 123 });
            fixClass.Test();
        }

        private static T Deserialize<T>(T target)
        {
            using (var ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, target);
                ms.Flush();
                ms.Position = 0;
                return (T)binaryFormatter.Deserialize(ms);
            }
        }
    }

    [Serializable]
    public class FixClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
        [NonSerialized]
        private XmlReader _xmlReader;

        public void Test()
        {
            Console.WriteLine($"_xmlReader is null:{_xmlReader is null}");
            Console.WriteLine("Pass");
        }
    }

    [Serializable]
    public class FailClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
        //[NonSerialized]
        private XmlReader _xmlReader;

        public void Test()
        {
            Console.WriteLine($"_xmlReader is null:{_xmlReader is null}");
            Console.WriteLine("Pass");
        }
    }
}
