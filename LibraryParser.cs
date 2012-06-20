using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCB_Layout_GA
{
    class LibraryParser
    {

        public static List<String> SplitBySpaces(string line)
        {
            List<string> strings = new List<string>();
            StringBuilder builder = new StringBuilder();

            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (inQuotes)
                {
                    if (line[i] == '"')
                    {
                        strings.Add(builder.ToString());
                        builder.Clear();
                        inQuotes = false;
                    }
                    else
                    {
                        builder.Append(line[i]);
                    }
                }
                else
                {
                    if (line[i] == ' ')
                    {
                        if (builder.Length > 0)
                        {
                            strings.Add(builder.ToString());
                            builder.Clear();
                        }
                    }
                    else if (line[i] == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        builder.Append(line[i]);
                    }
                }
            }
            if (builder.Length > 0)
            {
                strings.Add(builder.ToString());
                builder.Clear();
            }
            return strings;
        }

    }
}
