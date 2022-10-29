using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    public class State
    {
        public byte[] Board = new byte[8];
        public List<State> childStates;
        
        public State(byte[] board)
        {
            for(int i=0;i<8;++i){
                Board[i] = board[i];
            }
            
        }

        public List<State> expandState()
        {
            var childStates = new List<State>();

            for(int i = 0;i < 8; ++i)
            {
                int row = Board[i];
                for(byte j = 0;j < 8; ++j)
                {
                    var newState = new State(Board);
                    if(j != row)
                    {
                        newState.Board[i] = j;
                        childStates.Add(newState);
                    }
                }
            }
            
            return childStates;
        }
    }
}
