using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public struct GANet
    {
        private string mNetName;
        public string NetName { get { return mNetName; } }

        private int[,] mConnections;

        /// <summary>
        /// 2xn array holding connection indexes.  Connections[n,0] stores the module index, Connections[n,1] stores the pin index
        /// </summary>
        public int[,] Connections { get { return mConnections; } }

        public GANet(string netName, int[,] connections)
        {
            mConnections = connections;
            mNetName = netName;
        }

    }
}
