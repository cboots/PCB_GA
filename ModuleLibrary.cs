using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PCB_Layout_GA
{
    class ModuleLibrary
    {
        private Dictionary<string, Module> modules = new Dictionary<string, Module>();
        private Dictionary<string, string> libraryIndex = new Dictionary<string, string>();

        public void AddModule(Module mod)
        {
            if(!modules.ContainsKey(mod.LibName))
            {
                modules.Add(mod.LibName, mod);
            }
        }

        public Module FindModule(string libName)
        {
            Module mod = null;
            bool found = modules.TryGetValue(libName, out mod);
          
            if(found)
                return mod;

            //Module hasn't been loaded yet.  Search Index
            string library;
            found = libraryIndex.TryGetValue(libName, out library);

            if (found)
            {
                AddModuleLibrary(library);
                found = modules.TryGetValue(libName, out mod);
                if (found)
                    return mod;
            }
            //If we got here, total fail
            return null;
        }

        public void RefreshModuleIndex()
        {
            libraryIndex.Clear();
            string pathString = Properties.Settings.Default.LibrarySearchPaths;
            pathString = pathString.Replace("\\\\", "\\"); 
            string[] paths = pathString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string searchPath in paths)
            {
                //find all module libraries in this search path
                string[] libraries = Directory.GetFiles(searchPath, "*.mod");

                foreach (string library in libraries)
                {
                    addLibraryToIndex(library);
                }
            }
        }

        private void addLibraryToIndex(string library)
        {
            //Read index
            System.IO.StreamReader file =
                new System.IO.StreamReader(library);
            string line;
            bool done = false;
            while ((line = file.ReadLine()) != null && !done)
            {
                if (line.StartsWith("$INDEX"))
                {
                    //Read index
                    while ((line = file.ReadLine()) != null && !done)
                    {
                        if (line.StartsWith("$EndINDEX"))
                        {
                            done = true;
                        }
                        else
                        {
                            if (!libraryIndex.ContainsKey(line))
                            { 
                                libraryIndex.Add(line, library); 
                            }
                        }
                    }
                }
            }

            file.Close();
        }

        public void AddModuleLibrary(string library)
        {
            string[] lines = System.IO.File.ReadAllLines(library);
            int currentLine = 0;
            while (currentLine < lines.Length)
            {
                if (lines[currentLine].StartsWith("$MODULE"))
                {
                    Module mod = Module.parse(lines, ref currentLine);
                    AddModule(mod);
                }
                currentLine++;
            }
        }
    }
}
