using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab5
{
    class Program
    {
        const int AMOUNT_OF_NODES = 300;
        const int MIN_CONNECTIONS = 2;
        const int MAX_CONNECTIONS = 30;
        const int SCOUTS = 200;
        const int WORKERS = 300;
        static void Main(string[] args)
        {
            var graph = createGraph(AMOUNT_OF_NODES, MAX_CONNECTIONS, MIN_CONNECTIONS);  
            greedyCapture(ref graph);
            Console.WriteLine("Solution rate before = " + rateSolution(graph));
            beeAlghorithm(ref graph);
            Console.WriteLine("Solution rate after  = " + rateSolution(graph));

        }

        static void beeAlghorithm(ref Node[] graph)
        {
            int rate = rateSolution(graph);                 
                for(int i =0;i < SCOUTS; ++i)
                {
                    var rand = new Random();
                    int randIndex = rand.Next(0,AMOUNT_OF_NODES);
                    graph = nearSearch(graph, randIndex);
                }
        }

        static Node[] nearSearch(Node[] graph, int index)
        {
            Node node = graph[index];
            Node[] backCopy = DeepCopy(graph);
            //Console.WriteLine("BackCopy: " + rateSolution(backCopy));
            for (int i = 0;i < WORKERS; ++i)
            {
                
                int maxDegree = node.neighbours.Count;
                int maxIndex = index;
                foreach(var neigh in node.neighbours)
                {
                    if(graph[neigh].neighbours.Count > maxDegree)
                    {
                        maxDegree = graph[neigh].neighbours.Count;
                        maxIndex = neigh;
                    }
                }
                graph[maxIndex].captured = true;
                for(int j = 0;j < graph[maxIndex].neighbours.Count; ++j)
                {
                    graph[graph[maxIndex].neighbours[j]].captured = false;
                }
                greedyCapture(ref graph);
                
            }
            //Console.WriteLine("Graph :" + rateSolution(graph));
            if (rateSolution(graph) > rateSolution(backCopy)) return backCopy;
            else return graph;


        }

        public static T DeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        static int rateSolution(Node[] graph)
        {
            int res = 0;
            foreach(var node in graph)
            {
                if (node.captured) res++;
            }
            return res;
        }
        static bool isCovered(Node[] nodes, Node node)
        {
            if (node.captured) return true;
            for(int i = 0;i < node.neighbours.Count; ++i)
            {
                if (nodes[node.neighbours[i]].captured) return true;
            }
            return false;
        }

        static void outputGraph(Node[] nodes)
        {
            for(int i = 0;i < nodes.Length; ++i)
            {
                Console.WriteLine($"Node {nodes[i].number} Colored={nodes[i].captured} {nodes[i].neighbours.Count} connections: ");
                for(int j = 0;j < nodes[i].neighbours.Count; ++j)
                {
                    Console.WriteLine($"\tNode {nodes[i].neighbours[j]}");
                }
            }
            Console.WriteLine();
        }

        static Node[] createGraph(int amount, int maxConnections, int minConnections)
        {
            Node[] nodes = new Node[amount];
            for(int i = 0;i < amount; ++i)
            {
                nodes[i] = new Node();
                nodes[i].number = i;
            }
            Random rand = new Random();

            for(int i = 0;i < nodes.Length;++i)
            {
                int numConnections = rand.Next(minConnections, maxConnections);
                
                for(int j = nodes[i].neighbours.Count;j < numConnections; ++j)
                {
                    int randIndex = rand.Next(0, nodes.Length);
                    if(randIndex == i)
                    {
                        if(randIndex == nodes.Length - 1)
                        {
                            randIndex = 0;
                        }
                        else
                        {
                            randIndex++;
                        }
                    }
                    if (!nodes[i].connect(nodes[randIndex], maxConnections)) j--;
                }
            }
            return nodes;
            
        }

        static void greedyCapture(ref Node[] nodes)
        {
            foreach(var node in nodes)
            {
                if(!isCovered(nodes, node))
                {
                    node.captured = true;
                }
            }
        }
    }
}
