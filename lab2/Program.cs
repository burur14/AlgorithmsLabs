using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    class Program
    {
        static int limit = 10;
        /*public static int[] glukhiyKut = new int[20];
        public static int[] iterations = new int[20];
        public static int[] totalStates = new int[20];
        public static int[] statesInMemory = new int[20];*/

        static void Main(string[] args)
        {
            for (int i = 0; i < 20; ++i)
            {
                State state = new State(generateBoard());
                printBoard(state.Board);
                var time1 = DateTime.Now;
                var res = RBFS(state, 30, 0);
                var time2 = DateTime.Now;
                Console.WriteLine("Solution found in " + time2.Subtract(time1).TotalSeconds + " Seconds");
               // Console.WriteLine(iterations[i] + " " + glukhiyKut[i] + " " + totalStates[i] + " " + statesInMemory[i]+ "\n");
            }
            
            
        }

        static void LDFS()
        {

            State startState = new State(generateBoard());
            printBoard(startState.Board);

            var time1 = DateTime.Now;
            var result = DLS(startState, 0);
            var time2 = DateTime.Now;

            if (result.HasValue)
            {
                var solution = result.Value;
                Console.WriteLine("\nSolution found: ");
                printBoard(solution.Board);
            }
            else
            {
                Console.WriteLine("Solution not found;\tIncrease the limit");
            }
            Console.WriteLine(time2.Subtract(time1).TotalSeconds + " seconds");
        }

        static Optional<State> RBFS(State state, int f_limit, int depth)
        {

            if (isSolved(state.Board))
            {
                //statesInMemory[num] = depth + 55;
                return new Optional<State>(state);
            }
            if (depth >= limit)
            {
                //glukhiyKut[num]++;
                return new Optional<State>();
            }
            List<State> childStates = state.expandState();
            //totalStates[num] += 56;
            var f = new List<int>();
            for(int i = 0;i < childStates.Count; ++i)
            {
                f.Add(f2(childStates[i].Board));
            }
            while (true)
            {
                //iterations[num]++;
                int bestValue = f.Min();
                int bestIndex = f.IndexOf(f.Min());
                State bestState = childStates[bestIndex];
                
                if (bestValue > f_limit)
                {
                    //glukhiyKut[num]++;
                    return new Optional<State>();
                }
                childStates.Remove(bestState);
                f.Remove(bestValue);

                int alt = f.Min();
                var result = RBFS(bestState, Math.Min(f_limit, alt), depth+1);
                if (result.HasValue)
                {
                    return result;
                }
                
            }
        }

       
        
        static int f2(byte[] board)
        {
            int result = 0;
            for(int i = 1;i < 8; ++i)
            {
                for(int j = 0;j < i; ++j)
                {
                    if (board[i] + i == board[j] + j)
                    {
                        result++;
                    }
                    if (board[i] - i == board[j] - j)
                    {
                        result++;
                    }
                    if (board[i] == board[j])
                    {
                        result++;
                    }
                }
            }
            return result;
        }

      
        static Optional<State> DLS(State state, int depth)
        {
            if (isSolved(state.Board))
            {
                return new Optional<State>(state);
            }
            if (depth == limit)
            {
                return new Optional<State>();

            }
            for(int i = 0;i < 8; ++i)
            {
                state.Board[i] = (byte)(state.Board[i] == 0 ? 7 : state.Board[i] - 1);
                if (DLS(state, depth + 1).HasValue) return new Optional<State>(state);
                state.Board[i] = (byte)(state.Board[i] == 7 ? 0 : state.Board[i] + 1);
            }
            return new Optional<State>();
        }
        

        static byte[] generateBoard()
        {
            byte[] board = new byte[8];
            Random rand = new Random();
            for(int i = 0;i < board.Length; i++)
            {
                board[i] = (byte)rand.Next(0, 8);
            }
            
            return board;
        }
        static void printBoard(byte[] board)
        {
            Console.Write("[");
            for (int i = 0; i < board.Length; i++)
            {
                Console.Write($"{board[i]},");
            }
            Console.WriteLine("]");
            /*for (int i = 0; i < board.Length; i++)
            {
                Console.Write("|");
                for (int j = 0;j < board.Length; j++)
                {
                    if(board[j] == i)
                    {
                        Console.Write("X|");
                    }
                    else
                    {
                        Console.Write("o|");
                    }
                }
                Console.WriteLine();
            }*/
            
        }
        static bool isSolved(byte[] board)
        {
            for(int i = 0; i < board.Length; ++i)
            {
                for(int j = 0; j < board.Length; ++j)
                {
                    if (i == j) continue;
                    if(board[i] == board[j] || i + board[i] == j + board[j] || i - board[i] == j - board[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
    }
}

