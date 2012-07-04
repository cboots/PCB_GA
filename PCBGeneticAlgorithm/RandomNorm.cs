using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    class RandomNorm
    {
        private Random random;
        public RandomNorm()
        {
            random = new Random();
        }

        
        /// <summary>
        /// Returns two normal random variables with mean 0 and 
        /// </summary>
        /// <param name="Z0"></param>
        /// <param name="Z1"></param>
        public void GetNextTwo(out double Z0, out double Z1)
        {
            double U1 = 0.0;
            double U2 = 0.0;
            while(U1 == 0.0 || U2 == 0.0)
            {
                U1 = random.NextDouble();
                U2 = random.NextDouble();
            }
            Z0 = Math.Sqrt(-2 * Math.Log(U1)) * Math.Cos(2 * Math.PI * U2);
            Z1 = Math.Sqrt(-2 * Math.Log(U2)) * Math.Cos(2 * Math.PI * U1);

        }

        public void GetNextTwo(out double Z0, out double Z1, double mean0, double mean1, double deviation0, double deviation1)
        {
            GetNextTwo(out Z0, out Z1);
            Z0 = mean0 + deviation0 * Z0;
            Z1 = mean1 + deviation1 * Z1;
        }
    }
}
