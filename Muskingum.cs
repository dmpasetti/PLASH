using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plash
{
    public class Muskingum
    {

        public double[] FLT_Arr_InputFlow;
        public double[] FLT_Arr_OutputFlow;
        
        public double FLT_K; // > Timestep/2
        public double FLT_X; // <= 0.5
        public double FLT_Timestep;

        private double C1()
        {
            return (FLT_Timestep - 2 * FLT_K * FLT_X) / (2 * FLT_K * (1 - FLT_X) + FLT_Timestep);
        }

        private double C2()
        {
            return (FLT_Timestep + 2 * FLT_K * FLT_X) / (2 * FLT_K * (1 - FLT_X) + FLT_Timestep);
        }

        private double C3()
        {
            return (2 * FLT_K * (1 - FLT_X) - FLT_Timestep) / (2 * FLT_K * (1 - FLT_X) + FLT_Timestep);
        }

        private bool ValidParameters()
        {
            if(FLT_X >= 1)
            {
                return false;
            }

            if(FLT_X > (FLT_Timestep / (2 * FLT_K)))
            {
                return false;
            }

            if((1-FLT_X) < (FLT_Timestep / (2 * FLT_K)))
            {
                return false;
            }

            return true;

        }
        public static double[] ProcessDamping(Muskingum Sim)
        {
            double[] Outflow = new double[Sim.FLT_Arr_InputFlow.Length];
            if (Sim.ValidParameters())
            {
                double C1 = Sim.C1();
                double C2 = Sim.C2();
                double C3 = Sim.C3();
                
                for(int i = 0; i < Sim.FLT_Arr_InputFlow.Length; i++)
                {
                    if(i == 0)
                    {
                        Outflow[i] = Sim.FLT_Arr_InputFlow[i];
                    }
                    else
                    {
                        Outflow[i] = C1 * Sim.FLT_Arr_InputFlow[i] + C2 * Sim.FLT_Arr_InputFlow[i - 1] + C3 * Outflow[i - 1];
                    }
                }
                return Outflow;
            }
            else
            {                
                return Outflow;
            }
        }



    }
}
