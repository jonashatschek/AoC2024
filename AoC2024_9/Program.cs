using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace AoC2024_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //The disk map uses a dense format to represent the layout of files and free space on the disk.
            //The digits alternate between indicating the length of a file and the length of free space.

            //So, a disk map like 12345 would represent a one-block file, two blocks of free space, a three-block file,
            //four blocks of free space, and then a five-block file. A disk map like 90909 would represent three nine-block files in a row (with no free space between them).

            var test = true;
            var path = test ? "Test" : "Real";
            var diskMap = File.ReadAllText($"{path}/input.txt").ToArray();
            var nlaha = "";
            var id = 0;
            string[] oldStuff = [];

            //todo sort string
            for (int i = 0; i < diskMap.Length; i++)
            {
                //var newStuffLength = int.Parse(diskMap[i].ToString());

                ////init new array with right length 
                //var newStuff = new string[newStuffLength];

                //for (int j = 0; j < newStuffLength; j++)
                //{

                //    newStuff[j] += i % 2 == 0 ? id : ".";
                //}

                //string[] tempOldStuff = new string[newStuffLength + oldStuff.Length];

                //tempOldStuff.CopyTo(oldStuff, 0);
                //tempOldStuff.CopyTo(newStuff, tempOldStuff.Length);

                //if (i % 2 == 0)
                //{
                //    id++;
                //}

                oldStuff = GetNewStuff(oldStuff, diskMap[i].ToString(), i, id);

                //oldStuff = new string[length + ];
                Console.Write("sfga");
                //if (i % 2 == 0)
                //{
                //    //this means it is a file


                //}
                //else
                //{
                //    //this means it is a free space

                //}
                //oldStuff.CopyTo(tempOldStuff, 0);

            }



            //todo find free space to replace
            //todo find array index to replace it with

            Console.WriteLine(nlaha);
        }

        public static string[] GetNewStuff(string[] oldStuff, string toAdd, int currentIndex, int id)
        {

            var newStuffLength = int.Parse(toAdd);

            var newStuff = new string[oldStuff.Length + newStuffLength];

            if (oldStuff.Length > 0)
            {
                newStuff.CopyTo(oldStuff, 0);

            }

            int whereToStart = oldStuff.Length > 0 ? oldStuff.Length : 0;

            for (; whereToStart < newStuffLength; whereToStart++)
            {
                newStuff[whereToStart] += currentIndex % 2 == 0 ? id : ".";
            }

            return newStuff;
        }

    }

}
