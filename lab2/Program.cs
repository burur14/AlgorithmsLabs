using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    class Program
    {
        static int limit = 10;

        static void Main(string[] args)
        {
            
            State state = new State(generateBoard());
            printBoard(state.Board);
            var time1 = DateTime.Now;
            var res = RBFS(state, 30, 0);
            var time2 = DateTime.Now;
            printBoard(res.Value.Board);
            Console.WriteLine("RBFS Solution found in " + time2.Subtract(time1).TotalSeconds + " Seconds");

            time1 = DateTime.Now;
            res = LDFS(state, 0, 0);
            time2 = DateTime.Now;
            printBoard(res.Value.Board);
            Console.WriteLine("LDFS Solution found in " + time2.Subtract(time1).TotalSeconds + " Seconds");




        }


        static Optional<State> LDFS(State state, int currLine, int depth)
        {
            if(depth == limit || currLine > 7)
            {
                if (isSolved(state.Board))
                {
                    return new Optional<State>(state);
                }
                else
                {
                    return new Optional<State>();
                }
            }

            var result = LDFS(state, currLine + 1, depth);

            if (result.HasValue) return result;
            
            for(int i = 0; i < 8; ++i)
            {
                if(i != state.Board[currLine])
                {
                    var newState = new State(state.Board);
                    newState.Board[currLine] = (byte) i;
                    result = LDFS(newState, currLine + 1, depth + 1);
                    if (result.HasValue) return result;
                }
            }
            return new Optional<State>();

        }

        static Optional<State> RBFS(State state, int f_limit, int depth)
        {

            if (isSolved(state.Board))
            {
                return new Optional<State>(state);
            }
            if (depth >= limit)
            {
                return new Optional<State>();
            }
            List<State> childStates = state.expandState();
            var f = new List<int>();
            for(int i = 0;i < childStates.Count; ++i)
            {
                f.Add(f2(childStates[i].Board));
            }
            while (true)
            {
                int bestValue = f.Min();
                int bestIndex = f.IndexOf(f.Min());
                State bestState = childStates[bestIndex];
                
                if (bestValue > f_limit)
                {
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
            for (int i = 0; i < board.Length; i++)
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
            }
            
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

