using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace Intersect_Updater
{
    public class Update
    {
        public string FilePath;
        public File UpdateFile;

        public int SpeedUpdateTime;
        public long BytesDownloaded;
        public long Speed;

        public Update(string path, File file)
        {
            FilePath = path;
            UpdateFile = file;
        }
    }
}
