///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/7/2020 1:09:53 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;

namespace Demo.Lottery
{
    internal static class ImageUtil
    {
        #region field

        #endregion

        #region property

        #endregion

        #region constructor

        #endregion

        #region method
        internal static BitmapImage ConvertToBitmapImg(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            var bi = new BitmapImage();
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    bi.BeginInit();
                    bi.StreamSource = fs;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                }
                bi.Freeze();
            }
            catch
            {
            }
            return bi;
        }

        internal static IEnumerable<BitmapImage> LoadImgs(string folder)
        {
            var files = Directory.GetFiles(folder, "*.png|*.jpg|*.jpeg");
            if (null == files || 0 == files.Length)
            {
                yield break;
            }
            foreach (var file in files)
            {
                yield return ConvertToBitmapImg(file);
            }
        }
        #endregion
    }
}
