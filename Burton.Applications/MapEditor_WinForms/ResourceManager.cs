using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizerTest
{
    public class ResourceManager
    {
        public List<string> ResourceFileNames;
        public List<Image> PngFiles;

        public ResourceManager()
        {

        }

        public void LoadResources(string ResourceDirectory)
        {
            if (ResourceFileNames == null)
                ResourceFileNames = new List<string>();

            if (PngFiles == null)
                PngFiles = new List<Image>();

            PngFiles.Clear();
            ResourceFileNames.Clear();

            GetAllPngFilesInResourceDirectory(ResourceDirectory);

            foreach (var FileName in ResourceFileNames)
            {
             //   PngFiles.Add(Image.FromFile(FileName));
            }
        }

        private void GetAllPngFilesInResourceDirectory(string RootDir)
        {
            try
            {
                foreach (string File in Directory.GetFiles(RootDir, "*.png"))
                {
                    ResourceFileNames.Add(File);
                }

                foreach (string ChildDir in Directory.GetDirectories(RootDir))
                {
                    GetAllPngFilesInResourceDirectory(ChildDir);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

        }
    }
}
