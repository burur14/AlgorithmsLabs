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
            Console.WriteLine(time2.Subtract(time).Minutes);
        }

        static void adaptiveSort(string path)
        {
            
            while (!isSorted(path))
            {
                StreamReader A_reader = new StreamReader(path);
                File.WriteAllText("B.txt", String.Empty);
                File.WriteAllText("C.txt", String.Empty);
                while (!A_reader.EndOfStream)
                {
                    splitInTwoFiles(path, A_reader);
                }
                A_reader.Close();
                var series_B = findSeries("B.txt");
                var series_C = findSeries("C.txt");

                mergeSeries(series_B, series_C, path);
            }

        }

        static void splitInTwoFiles(string path, StreamReader A_reader)
        {


            List<string> series = new List<string>();
           
            string line = buffLine;
            if(line == null) { line = A_reader.ReadLine(); }
            series.Add(line);
            

            while (true)
            {
                string prevLine = line;
                line = A_reader.ReadLine();
                if(Convert.ToInt32(prevLine) <= Convert.ToInt32(line))
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

        static List<List<int>> findSeries(string path)
        {
            var series = new List<List<int>>();
            var reader = new StreamReader(path);
            int i = 0;

            string line = reader.ReadLine();
            series.Add(new List<int>());
            series[0].Add(Convert.ToInt32(line));
            string prevLine = line;
            while ((line = reader.ReadLine()) != null)
            {
                if (Convert.ToInt32(prevLine) > Convert.ToInt32(line))
                {
                    series.Add(new List<int>());
                    i++;
                }
                series[i].Add(Convert.ToInt32(line));
                prevLine = line;
            }

            reader.Close();
            return series;


        }

        static void mergeSeries(List<List<int>> series_B, List<List<int>> series_C, string path)
        {
            StreamWriter writer = new StreamWriter(path);
            
            for(int i = 0;i < Math.Min(series_B.Count, series_C.Count); i++)
            {
                int j = 0, k = 0;
                while(j < series_C[i].Count && k < series_B[i].Count)
                {
                    if(series_C[i][j] > series_B[i][k])
                    {
                        writer.WriteLine(series_B[i][k]);
                        k++;
                    }
                    else
                    {
                        writer.WriteLine(series_C[i][j]);
                        j++;
                    }
                }
                if (k == series_B[i].Count)
                {
                    while (j < series_C[i].Count)
                    {
                        writer.WriteLine(series_C[i][j]);
                        j++;
                    }
                }
                else
                {
                    while (k < series_B[i].Count)
                    {
                        writer.WriteLine(series_B[i][k]);
                        k++;
                    }
                }
            }
            if(series_B.Count > series_C.Count)
            {
                for(int i = series_C.Count;i < series_B.Count; i++)
                {
                    for(int j = 0;j < series_B[i].Count; j++)
                    {
                        writer.WriteLine(series_B[i][j]);
                    }
                }
            }else if (series_B.Count < series_C.Count)
            {
                for (int i = series_B.Count; i < series_C.Count; i++)
                {
                    for (int j = 0; j < series_C[i].Count; j++)
                    {
                        writer.WriteLine(series_C[i][j]);
                    }
                }
            }
            writer.Flush();
            writer.Close();
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
