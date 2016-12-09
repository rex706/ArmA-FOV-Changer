using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldOfViews
{
    public class AspectRatio
    {
        public int width { get; set; }
        public int height { get; set; }
        public string ratio { get; set; }
    }

    class Ratios
    {
        public HashSet<AspectRatio> ratioSet()
        {
            HashSet<AspectRatio> ratios = new HashSet<AspectRatio>();

            ratios.Add(new AspectRatio() { width = 800, height = 600, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 960, height = 540, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1024, height = 576, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1024, height = 600, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 960, height = 720, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1024, height = 768, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1024, height = 600, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1024, height = 640, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1152, height = 720, ratio = "8:5" });
            ratios.Add(new AspectRatio() { width = 1280, height = 720, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1120, height = 832, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1152, height = 864, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1152, height = 720, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1366, height = 768, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1280, height = 800, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1280, height = 768, ratio = "15:9" });
            ratios.Add(new AspectRatio() { width = 1280, height = 960, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1280, height = 1024, ratio = "5:4" });
            ratios.Add(new AspectRatio() { width = 1600, height = 900, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1400, height = 1050, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1440, height = 900, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1440, height = 1080, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1600, height = 1200, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1776, height = 1000, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1600, height = 1280, ratio = "5:4" });
            ratios.Add(new AspectRatio() { width = 1680, height = 1050, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1920, height = 1080, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1920, height = 1200, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 2048, height = 1152, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 1792, height = 1344, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 1856, height = 1392, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 2560, height = 1080, ratio = "21:9" });
            ratios.Add(new AspectRatio() { width = 1920, height = 1440, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 2560, height = 1440, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 2304, height = 1728, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 2560, height = 1920, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 2560, height = 1600, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 3440, height = 1440, ratio = "21:9" });
            ratios.Add(new AspectRatio() { width = 2560, height = 2048, ratio = "5:4" });
            ratios.Add(new AspectRatio() { width = 2800, height = 2100, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 2880, height = 1800, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 3072, height = 1920, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 3200, height = 1800, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 3200, height = 2400, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 3840, height = 2160, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 3840, height = 2400, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 1024, height = 600, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 4096, height = 2304, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 5120, height = 2160, ratio = "21:9" });
            ratios.Add(new AspectRatio() { width = 4096, height = 3072, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 4096, height = 2560, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 5120, height = 2880, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 5120, height = 3200, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 5120, height = 4096, ratio = "5:4" });
            ratios.Add(new AspectRatio() { width = 5670, height = 1080, ratio = "5:4" });
            ratios.Add(new AspectRatio() { width = 6400, height = 4800, ratio = "4:3" });
            ratios.Add(new AspectRatio() { width = 6400, height = 4096, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 7680, height = 4320, ratio = "16:9" });
            ratios.Add(new AspectRatio() { width = 7680, height = 4800, ratio = "16:10" });
            ratios.Add(new AspectRatio() { width = 8192, height = 4608, ratio = "16:9" });

            return ratios;
        }
    }
}
