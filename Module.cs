using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCB_Layout_GA
{
    class Module
    {
        public List<Pad> mPads;
        public List<Drawing> mDrawings;


        public class Pad
        {
            public enum PadType {CIRCLE, RECTANGULAR, OBLONG, TRAPEZE};
            public enum PadAttr { STD, SMD, CONN, HOLE, MECA };

            public int XSize { get; set; }
            public int YSize { get; set; }
            public int Orientation { get; set; }
            public int XDelta { get; set; }
            public int YDelta { get; set; }

            public int NetNumber { get; set; }
            public String NetName { get; set; }

            public PadType Type { get; set; }
            public PadAttr Attr { get; set; }
            public int DrillSize { get; set; }
            public int DrillXOffset { get; set; }
            public int DrillYOffset { get; set; }

        }


    }
}
