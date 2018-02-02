using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace ConsoleApp2
{
    public class Wallpaper
    {
        Wallpaper() { }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;
		const float MIN_IMAGE_WIDTH = 0.7F; 

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		public enum Style : int
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void Set(Image img, Style style, string imageName)
        {
			var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			Console.WriteLine(dir);
			XmlDocument doc = new XmlDocument();
			doc.Load(dir + "/Settings.xml");
			XmlNode xmlNode = doc.DocumentElement.SelectSingleNode("/settings/imagePath");
			Console.WriteLine(xmlNode.InnerText);

			//System.IO.Stream s = new System.Net.WebClient().OpenRead(uri.ToString());

			//System.Drawing.Image img = System.Drawing.Image.FromStream(s);

			//Save new image to Images folder
			string tempPath = Path.Combine(xmlNode.InnerText, imageName + ".bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

			Console.WriteLine(GetDesktopDimensions()[0] + " " + GetDesktopDimensions()[1]);

			var arr = GetDesktopDimensions();

			//If image is to small we don't want to stretch it
			if (PictureIsNarrow((float)arr[0], (float)img.Width, MIN_IMAGE_WIDTH)) {
				style = Style.Centered;
			}
			
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

		/// <summary>
		/// Return Width & Height of current virtual screen
		/// </summary>
		/// <returns>Width & Height</returns>
		public static int[] GetDesktopDimensions() {
			int width = System.Windows.Forms.SystemInformation.VirtualScreen.Width;
			int height = System.Windows.Forms.SystemInformation.VirtualScreen.Height;
			return new int[2] { width, height};
		}

		/// <summary>
		/// Check width of pic return true if under the spcified min image width
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="picWidth"></param>
		/// <param name="minWidthForPic"></param>
		/// <returns></returns>
		public static bool PictureIsNarrow(float screen, float picWidth, float minWidthForPic) {
			float perc = ((float)picWidth/ (float)screen);
			Console.WriteLine("Pic was " + perc + " of screen");
			if (perc < minWidthForPic) {
				return true;
			}
			return false;
		}




    }

}
