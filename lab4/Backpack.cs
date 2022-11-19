using System;
using System.Collections.Generic;
using System.Text;

namespace lab4
{
    class Backpack
    {
        public bool[] items;
        public Backpack(int amount)
        {
            items = new bool[amount];
        }
        public Backpack(Backpack backpack)
        {
            this.items = backpack.items;
        }

        public void print()
        {
            Console.Write("[");
            for(int i = 0;i < this.items.Length;++i)
            {
                if(this.items[i]) Console.Write("1");
                else Console.Write("0");
                if (i != this.items.Length - 1) Console.Write(",");
            }
            Console.WriteLine("] Cost: " + Program.getTotalCost(this) + " Weight: " + Program.getTotalWeight(this));
        }

        public static bool operator ==(Backpack bp1, Backpack bp2)
        {
            for(int i = 0;i < bp1.items.Length; ++i)
            {
                if (bp1.items[i] != bp2.items[i]) return false;
            }
            return true;
        }
        public static bool operator !=(Backpack bp1, Backpack bp2)
        {
            return !(bp1 == bp2);
        }

    }
}
