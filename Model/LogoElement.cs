///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/6/2020 2:40:25 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.IO;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Demo.Lottery
{
    public class LogoElement
    {
        #region field

        #endregion

        #region property
        public UserInfo User { get; set; }

        public ModelVisual3D Element { get; set; }

        public TranslateTransform3D TranslateTransform { get; set; }

        public DoubleAnimation OffsetZAnimation { get; set; }
        #endregion

        #region constructor

        #endregion

        #region method
        #endregion
    }

    public class UserInfo : Employee
    {
        string _logoPath = string.Empty;
        public string LogoPath
        {
            get => _logoPath;
            set
            {
                if (File.Exists(value))
                {
                    _logoPath = value;
                    Logo = ImageUtil.ConvertToBitmapImg(value);
                }
            }
        }

        public bool IsChecked { get; set; }

        public BitmapImage Logo { get; set; }
    }

    public class CameraInfo
    {
        public static CameraInfo Instance { get; private set; }

        public Point3D Position { get; set; } = new Point3D(0, 0, 10);
        public Vector3D LookDirection { get; set; } = new Vector3D(0, 0, -1);
        public Vector3D UpDirection { get; set; } = new Vector3D(0, 1, 0);
        public double FieldOfView { get; set; } = 120;

        static CameraInfo()
        {
            Instance = new CameraInfo();
        }

        private CameraInfo() { }
    }
}
