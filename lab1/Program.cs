using System;
using System.Collections.Generic;
using System.IO;

namespace lab1
{
    class Program
    {
        static int counter = 1;
        static string buffLine = null;
        static void Main(string[] args)
        {
            string path = "A.txt";
            DateTime time = DateTime.Now;
            adaptiveSort(path);
            DateTime time2 = DateTime.Now;
            Console.WriteLine(time2.Subtract(time).Minutes + "minutes");
        }

        static void adaptiveSort(string path)
        {
            
            while (!isSorted(path))
            {
                StreamReader A_reader = new StreamReader(path);
                File.WriteAllText("B.txt", "");
                File.WriteAllText("C.txt", "");
                
                splitInTwoFiles(A_reader);
                A_reader.Close();
                merge("A.txt", "B.txt", "C.txt");
            }

        }

        static void splitInTwoFiles(StreamReader A_reader)
        {

            while (!A_reader.EndOfStream)
            {
                List<string> series = new List<string>();

                string line = buffLine;
                if (line == null) { line = A_reader.ReadLine(); }
                series.Add(line);


                while (!A_reader.EndOfStream)
                {
                    string prevLine = line;
                    line = A_reader.ReadLine();
                    int lineInt = Convert.ToInt32(line);
                    int prevLineInt = Convert.ToInt32(prevLine);
                    if (prevLineInt <= lineInt)
                    {
                        series.Add(line);
                    }
                    else
                    {
                        buffLine = line;
                        break;
                    }

                }
                writeToFile(series);
            }
        }

        static void writeToFile(List<string> series)
        {
            if(counter % 2 == 1)
            {
                StreamWriter writer = new StreamWriter("B.txt", true);
                foreach(string line in series)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
                writer.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter("C.txt", true);
                foreach (string line in series)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
                writer.Close();
            }
            counter++;
        }

        static void merge(string pathA, string pathB, string pathC)
        {
            StreamReader reader_B = new StreamReader(pathB);
            StreamReader reader_C = new StreamReader(pathC);
            StreamWriter writer_A = new StreamWriter(pathA);
            int prevNumC, prevNumB;
            int numB = Convert.ToInt32(reader_B.ReadLine());
            int numC = Convert.ToInt32(reader_C.ReadLine());
            while (!reader_B.EndOfStream && !reader_C.EndOfStream)
            {
                
                if(numB > numC)
                {
                    writer_A.WriteLine(numC);
                    prevNumC = numC;
                    numC = Convert.ToInt32(reader_C.ReadLine());
                    if (numC < prevNumC)
                    {
                        do
                        {
                            writer_A.WriteLine(numB);
                            prevNumB = numB;
                            numB = Convert.ToInt32(reader_B.ReadLine());
                        } while (numB >= prevNumB);
                    }
                }
                else
                {
                    writer_A.WriteLine(numB);
                    prevNumB = numB;
                    numB = Convert.ToInt32(reader_B.ReadLine());
                    if(numB < prevNumB)
                    {
                        do
                        {
                            writer_A.WriteLine(numC);
                            prevNumC = numC;
                            numC = Convert.ToInt32(reader_C.ReadLine());
                        } while (numC >= prevNumC);
                    }
                }
            }
            writer_A.WriteLine(Math.Min(numB, numC));
            writer_A.WriteLine(Math.Max(numB, numC));
            if (reader_B.EndOfStream)
            {
                while (!reader_C.EndOfStream)
                {
                    writer_A.WriteLine(Convert.ToInt32(reader_C.ReadLine()));
                }
            }
            else
            {
                while (!reader_B.EndOfStream)
                {
                    writer_A.WriteLine(Convert.ToInt32(reader_B.ReadLine()));
                }
            }
            reader_B.Close();
            reader_C.Close();
            writer_A.Flush();
            writer_A.Close();
        }

        static bool isSorted(string path)
        {
            StreamReader reader = new StreamReader(path);

            int num1 = Convert.ToInt32(reader.ReadLine());
            int num2 = Convert.ToInt32(reader.ReadLine());
            while(!reader.EndOfStream)
            {
                
                if(num1 > num2)
                {
                    reader.Close();
                    return false;
                }
                num1 = num2;
                num2 = Convert.ToInt32(reader.ReadLine());

            }
            reader.Close();
            return true;
        }
    }  
}
