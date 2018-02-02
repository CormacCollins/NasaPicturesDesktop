using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static ConsoleApp2.Wallpaper;

namespace ConsoleApp2
{
    class Program
    {

        static void Main(string[] args)
        {
            string api_key = "DEMO_KEY";
            string url = String.Format("https://api.nasa.gov/planetary/apod?api_key=" + api_key);

			// Bitmap newImage = (Bitmap)Image.FromFile(@"C:\Users\corma\PycharmProjects\untitled\CloudsintheLMC.bmp");
			Console.WriteLine("Fetching image");			
            RestRequest r = new RestRequest();
            Bitmap newImage = (Bitmap)r.GetRequestImage(url);
            Style s = Style.Stretched;
            Wallpaper.Set(newImage, s, r.lastImageName);

        }
    }
}
