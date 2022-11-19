using System;
using System.Collections.Generic;
using System.Text;

namespace lab4
{
    class Item
    {
        public int Weight { get; set; }
        public int Cost { get; set; }
    
        public Item()
        {
            var rand = new Random();
            Weight = rand.Next(2, 11);
            Cost = rand.Next(1, 6);
        }
    }
}
