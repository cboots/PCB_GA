using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class Selection
    {
        private static Random sRand = new Random();
        public static GALayout[] GenerateNextGeneration(GeneticAlgorithm ga, GALayout[] currentGeneration)
        {
            //Elitist Selection
            GALayout[] elites = ElitistSelection(currentGeneration);

            //Normal selection
            GALayout[] selected = Select(currentGeneration, currentGeneration.Length - elites.Length);

            //Mutation
            Mutation.Mutate(ga, selected);

            //Crossover
            Crossover.CrossOver(ga, selected);

            //Assemble final generation
            GALayout[] nextGen = new GALayout[currentGeneration.Length];

            int i = 0;
            for (int j = 0; j < elites.Length; j++)
            {
                nextGen[i] = elites[j];
                i++;
            }

            for (int j = 0; j < selected.Length; j++)
            {
                nextGen[i] = selected[j];
                i++;
            }

            return nextGen;
        }

        private static GALayout[] ElitistSelection(GALayout[] currentGeneration)
        {
            //Simple elitism for now.  Look into Pareto Optimal Elitism
            GALayout[] elites = new GALayout[1];
            elites[0] = currentGeneration[0];
            for (int i = 1; i < currentGeneration.Length; i++)
            {
                if (currentGeneration[i].Fitness < elites[0].Fitness)
                    elites[0] = currentGeneration[i];
            }
            return elites;
        }

        public static GALayout[] Select(GALayout[] currentGeneration, int selectionCount)
        {

            double mean;
            double std;
            AnalyzeStats(currentGeneration, out mean, out std);

            //Use SUS algorithm
            double ptr = sRand.NextDouble();
            int j = 0;

            GALayout[] nextGeneration = new GALayout[selectionCount];
            double sum = 0;
            int i;
            for (sum = i = 0; i < currentGeneration.Length; i++)
            {
                for (sum += ExpVal(currentGeneration[i].VariedFitness, mean, std); sum > ptr; ptr++)
                {
                    //Select oldgen[i] as newgen[j]
                    if (j < selectionCount)
                    {
                        nextGeneration[j] = currentGeneration[i];
                        j++;
                    }
                }
            }

            return nextGeneration;

        }

        private static void AnalyzeStats(GALayout[] currentGeneration, out double mean, out double std)
        {
            double sum1 = 0.0;
            double sum2 = 0.0;

            for (int i = 0; i < currentGeneration.Length; i++)
            {
                sum1 += currentGeneration[i].VariedFitness;
            }

            mean = sum1 / currentGeneration.Length;

            for (int i = 0; i < currentGeneration.Length; i++)
            {
                sum2 += (currentGeneration[i].VariedFitness - mean) * (currentGeneration[i].VariedFitness - mean);
            }

            std = Math.Sqrt(sum2 / (currentGeneration.Length - 1));
        }

        public static double ExpVal(double fitness, double mean, double std)
        {
            double val = 0.0;
            if (std > 0)
            {
                val = 1.0 + (mean - fitness) / (2 * std);
            }
            else
            {
                val = 1.0;
            }

            if (val <= 0)
            {
                val = 0.1;
            }

            return val;
        }
    }
}
