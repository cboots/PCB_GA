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
        public int[,] Connections { get { return mConnections; } }

        public GANet(string netName, int[,] connections)
        {
            mConnections = connections;
            mNetName = netName;
        }

    }
}
