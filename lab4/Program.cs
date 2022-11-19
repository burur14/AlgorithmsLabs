using System;

namespace lab4
{
    class Program
    {
        public const int CAPACITY = 150;
        public const int AMOUNT = 100;
        public const int NUMBER_OF_ITERATIONS = 1000;
        public static Item[] itemList = createItems(AMOUNT);
        static void Main(string[] args)
        {
           
            var backpacks = initBackpacks(AMOUNT);
            printItems(itemList);
            backpacks = geneticAlgorithm(backpacks);
            Console.WriteLine("\n\n\nBEST ITEM:");
            backpacks[getBestIndex(backpacks)].print();
        }

        static Backpack[] geneticAlgorithm(Backpack[] backpacks)
        {
            for(int i = 0;i < NUMBER_OF_ITERATIONS; ++i)
            {
                backpacks = geneticIteration(backpacks);
            }
            return backpacks;
        }

        static Backpack[] geneticIteration(Backpack[] backpacks)
        {
            (var bestBP, var randomBP) = chooseAncestors(backpacks);

            var child = evenSex(bestBP, randomBP);
            var mutated = mutate(child);
            
            if (getTotalWeight(child) > CAPACITY && getTotalWeight(mutated) > CAPACITY) return backpacks;
            var improved = localImprovement(child, mutated);

            var best = chooseBest(child, mutated, improved);
            int worstIndex = getWorstIndex(backpacks);
            backpacks[worstIndex] = best;
            return backpacks;
        }

        static int getWorstIndex(Backpack[] backpacks)
        {
            int worstCost = int.MaxValue;
            int worstIndex = -1;
            for(int i = 0;i < AMOUNT; ++i)
            {
                if(getTotalCost(backpacks[i]) < worstCost)
                {
                    worstCost = getTotalCost(backpacks[i]);
                    worstIndex = i;
                }
            }
            return worstIndex;
        }

        static Backpack chooseBest(Backpack child, Backpack mutated, Backpack improved)
        {
            if(getTotalWeight(child) > CAPACITY)
            {
                if(getTotalWeight(improved) > CAPACITY)
                {
                    return mutated;
                }
                if (getTotalCost(mutated) > getTotalCost(improved)) return mutated;
                else return improved;
            }else if(getTotalWeight(mutated) > CAPACITY)
            {
                if(getTotalWeight(improved) > CAPACITY)
                {
                    return child;
                }
                if (getTotalCost(child) > getTotalCost(improved)) return child;
                else return improved;
            }else if (getTotalWeight(improved) > CAPACITY)
            {
                if (getTotalCost(mutated) > getTotalCost(child)) return mutated;
                else return child;
            }
            else
            {
                int maxCost = Math.Max(getTotalCost(child), Math.Max(getTotalCost(mutated), getTotalCost(improved)));
                if (maxCost == getTotalCost(improved)) return improved;
                else if (maxCost == getTotalCost(mutated)) return mutated;
                else return child;
            }
        }
        static Backpack localImprovement(Backpack child, Backpack mutated)
        {
            if (getTotalWeight(mutated) > CAPACITY)
            {
                if (getTotalWeight(child) <= CAPACITY)
                {
                    return improve(child);
                }
            }
            else
            {
                if (getTotalWeight(child) <= CAPACITY)
                {
                    if (getTotalCost(child) >= getTotalCost(mutated))
                    {
                        return improve(child);
                    }
                    else
                    {
                        return improve(mutated);
                    }
                }
                else
                {
                    return improve(mutated);
                }
            }
            return null;
        }

        static Backpack improve(Backpack bp)
        {
            var result = new Backpack(bp);
            if (result.items[getBestItemIndex()] == false)
            {
                result.items[getBestItemIndex()] = true;
            }
            return result;
        }

        static int getBestItemIndex()
        {
            double bestBenefit = 0;
            int bestIndex = -1;
            for(int i = 0;i < AMOUNT; ++i)
            {
                if(itemList[i].Cost / itemList[i].Weight > bestBenefit)
                {
                    bestBenefit = itemList[i].Cost / itemList[i].Weight;
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        static Backpack mutate(Backpack bp)
        {
            var rand = new Random();
            if(rand.NextDouble() > 0.1)
            {
                int randIndex1 = rand.Next(0, bp.items.Length);
                int randIndex2 = rand.Next(0, bp.items.Length);
                while(randIndex1 == randIndex2)
                {
                    randIndex2 = rand.Next(0, bp.items.Length);
                }

                bool tmp = bp.items[randIndex1];
                bp.items[randIndex1] = bp.items[randIndex2];
                bp.items[randIndex2] = tmp;
            }
            return bp;
        }

        static Backpack evenSex(Backpack bp1, Backpack bp2)
        {
            var rand = new Random();
            var bpRes = new Backpack(AMOUNT);
            for(int i = 0;i < bp1.items.Length; ++i)
            {
                if(rand.NextDouble() < 0.5)
                {
                    bpRes.items[i] = bp1.items[i];
                }
                else
                {
                    bpRes.items[i] = bp2.items[i];
                }
            }
            return bpRes;
        }

        static (Backpack, Backpack) chooseAncestors(Backpack[] backpacks)
        {
            int bestIndex = getBestIndex(backpacks);
            var bestBP = backpacks[bestIndex];                          // getting best backpack

            var rand = new Random();
            int randomIndex = rand.Next(0, backpacks.Length);
            while (randomIndex == bestIndex)
            {
                randomIndex = rand.Next(0, backpacks.Length);           //getting random backpack except best one
            }
            var randomBP = backpacks[randomIndex];

            return (bestBP, randomBP);
        }

        static int getBestIndex(Backpack[] bps)
        {
            int bestIndex = -1;
            int bestCost = 0;
            for(int i = 0;i < bps.Length; ++i)
            {
                if(getTotalCost(bps[i]) > bestCost)
                {
                    bestCost = getTotalCost(bps[i]);
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        static Item[] createItems(int amount)
        {
            Item[] items = new Item[amount];
            for(int i = 0;i < items.Length; ++i)
            {
                items[i] = new Item();
            }
            return items;
        }

        static Backpack[] initBackpacks(int amount)
        {
            Backpack[] backpacks = new Backpack[amount];
            for(int i = 0;i < backpacks.Length; ++i)
            {
                backpacks[i] = new Backpack(AMOUNT);
                backpacks[i].items[i] = true;
            }
            return backpacks;
        }

        static void printItems(Item[] items)
        {
            Console.Write("Weights: ");
            foreach(var item in items)
            {
                Console.Write(item.Weight + " ");
            }
            Console.Write("\nCosts: ");
            foreach (var item in items)
            {
                Console.Write(item.Cost + " ");
            }
            Console.WriteLine();
        }

        static void printBackpacks(Backpack[] backpacks)
        {
            foreach(var item in backpacks)
            {
                item.print();
            }
            Console.WriteLine();
        }

        public static int getTotalCost(Backpack bp)
        {
            int totalCost = 0;
            for(int i = 0;i < bp.items.Length; ++i)
            {
                if (bp.items[i]) totalCost += itemList[i].Cost;
            }
            return totalCost;
        }

        public static int getTotalWeight(Backpack bp)
        {
            int totalWeight = 0;
            for (int i = 0; i < bp.items.Length; ++i)
            {
                if (bp.items[i]) totalWeight += itemList[i].Weight;
            }
            return totalWeight;
        }

    }
}
