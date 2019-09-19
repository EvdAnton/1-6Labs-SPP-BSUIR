using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] types = null;
            Assembly getAssembly = LoadAssemblyByPath();
            if (getAssembly != null)
                types = getAssembly.GetTypes().OrderBy(x => x.FullName).ToArray();
            GetPublicTypes(types);
            Console.ReadLine();
        }

        private static void GetPublicTypes(Type[] types)
        {
            foreach (var type in types)
            {
                var typeNamespace = type.Namespace;
                if (type.IsClass)
                {
                    if (type.IsPublic)
                        Console.WriteLine(type.FullName + "---> class");
                    foreach (var t in type.GetFields().OrderBy(x => x.Name))
                    {
                        Console.WriteLine(typeNamespace + '.' + type.Name + '.' + t.FieldType + "---> field " + t.Name);
                    }
                    foreach (var t in type.GetProperties().OrderBy(x => x.Name))
                    {
                        Console.WriteLine(typeNamespace + '.' + type.Name + '.' + t.PropertyType + "---> prop" + t.Name);
                    }
                    foreach (var t in type.GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.Public).OrderBy(x=>x.Name))
                    {
                        Console.WriteLine(typeNamespace + '.' + type.Name + '.' + t.ReturnType + "---> method" + t.Name);
                    }
                }
                
                if(type.GetNestedTypes().Length > 0)
                {
                    GetPublicTypes(type.GetNestedTypes().OrderBy(c => c.FullName).ToArray());
                }
            }
        }

        private static Assembly LoadAssemblyByPath()
        {
            Assembly assembly = null;
            bool isExist = false;

            while (!isExist)
            {
                Console.Write("Enter path to assembly: ");
                string assemblyPath = Console.ReadLine();
                if (File.Exists(assemblyPath))
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(assemblyPath);
                        isExist = true;
                    }
                    catch { }

                }
            }
            return assembly;
        }
    }
}
