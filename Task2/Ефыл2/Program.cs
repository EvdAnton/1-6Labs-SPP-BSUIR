using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Task1;

namespace Task2
{
    class Program
    {
        private static int x = 0;
        const string SOURCE_DIRECTORY_PATH = "Enter the path to the source directory: ";
        const string DESTINATION_DIRECTORY_PATH = "Enter the path to the destination directory: ";
        const int COUNT_OF_THREADS = 5;
        private static TaskQueue taskQueue = new TaskQueue(COUNT_OF_THREADS);

        static void Main(string[] args)
        {
            DirectoryInfo sourceDirectoryPath = GetDirectoryPath(SOURCE_DIRECTORY_PATH);
            DirectoryInfo destinationDirectoryPath = GetDirectoryPath(DESTINATION_DIRECTORY_PATH);

            CopyDirectory(sourceDirectoryPath, destinationDirectoryPath);

            taskQueue.WaitAll();
            Console.WriteLine(x.ToString());
            Console.ReadLine();
        }

        static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                var state = new List<string>
                {
                    destination.FullName,
                    file.FullName
                };
                taskQueue.EnqueueTask(state, CopyFile);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                string destinationDir = Path.Combine(destination.FullName, dir.Name);

                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }

        private static void CopyFile(object state)
        {
            if (state is List<string> data)
            {
                var destination = data[0];
                var file = new FileInfo(data[1]);
                var dest = Path.Combine(destination, file.Name);
                try
                {
                    if (!Directory.Exists(destination))
                        Directory.CreateDirectory(destination);
                    File.Copy(file.FullName, Path.Combine(destination,
                        file.Name), true);
                    Console.WriteLine("Файл {0} скопирован в потоке {1}", file.FullName, Thread.CurrentThread.ManagedThreadId);
                    x++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static DirectoryInfo GetDirectoryPath(string message)
        {
            string directoryPath = string.Empty;
            bool isExist = false;
            while (!isExist) {

                Console.Write(message);
                directoryPath = Console.ReadLine();

                if (Directory.Exists(directoryPath))
                    isExist = true;
            }

            return new DirectoryInfo(directoryPath);

        }
    }
}
