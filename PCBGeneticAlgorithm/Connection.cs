using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelaunayTriangulator;

namespace PCBGeneticAlgorithm
{
    public class Connection
    {
        public Vertex P1 { get; set; }
        public Vertex P2 { get; set; }

        public Connection(Vertex p1, Vertex p2)
        {
            P1 = p1;
            P2 = p2;
        }


        public bool Intersects(Connection other)
        {
            return linesIntersect(P1.x, P1.y, P2.x, P2.y,
                                  other.P1.x, other.P1.y, other.P2.x, other.P2.y);
        }

        public static bool linesIntersect(double x1, double y1,
                                        double x2, double y2,
                                        double x3, double y3,
                                        double x4, double y4)
        {
            return ((relativeCCW(x1, y1, x2, y2, x3, y3) *
                     relativeCCW(x1, y1, x2, y2, x4, y4) <= 0)
                    && (relativeCCW(x3, y3, x4, y4, x1, y1) *
                        relativeCCW(x3, y3, x4, y4, x2, y2) <= 0));
        }

        public static int relativeCCW(double x1, double y1,
                                  double x2, double y2,
                                  double px, double py)
        {
            x2 -= x1;
            y2 -= y1;
            px -= x1;
            py -= y1;
            double ccw = px * y2 - py * x2;
            if (ccw == 0.0)
            {
                // The point is colinear, classify based on which side of
                // the segment the point falls on.  We can calculate a
                // relative value using the projection of px,py onto the
                // segment - a negative value indicates the point projects
                // outside of the segment in the direction of the particular
                // endpoint used as the origin for the projection.
                ccw = px * x2 + py * y2;
                if (ccw > 0.0)
                {
                    // Reverse the projection to be relative to the original x2,y2
                    // x2 and y2 are simply negated.
                    // px and py need to have (x2 - x1) or (y2 - y1) subtracted
                    //    from them (based on the original values)
                    // Since we really want to get a positive answer when the
                    //    point is "beyond (x2,y2)", then we want to calculate
                    //    the inverse anyway - thus we leave x2 & y2 negated.
                    px -= x2;
                    py -= y2;
                    ccw = px * x2 + py * y2;
                    if (ccw < 0.0)
                    {
                        ccw = 0.0;
                    }
                }
            }
            return (ccw < 0.0) ? -1 : ((ccw > 0.0) ? 1 : 0);
        }


    }
}
