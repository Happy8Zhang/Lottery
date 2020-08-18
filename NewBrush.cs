//=======================================================
///	Copyright (c) 2020 Happy. All Rights Reserved
/// Author:			Happy
/// Time:			2019\2\1 星期五 17:12:17
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Demo.Lottery
{
    public static class NewBrush
    {
        public static SolidColorBrush Blue => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 0,
            G = 116,
            B = 217,
        });

        public static SolidColorBrush Aqua => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 127,
            G = 219,
            B = 255,
        });

        public static SolidColorBrush Teal => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 57,
            G = 204,
            B = 204,
        });

        public static SolidColorBrush Olive => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 61,
            G = 153,
            B = 112,
        });

        public static SolidColorBrush Green => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 46,
            G = 204,
            B = 64,
        });

        public static SolidColorBrush Lime => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 1,
            G = 255,
            B = 112,
        });

        public static SolidColorBrush Yellow => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 255,
            G = 220,
            B = 0,
        });

        public static SolidColorBrush Orange => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 255,
            G = 133,
            B = 27,
        });

        public static SolidColorBrush Red => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 255,
            G = 65,
            B = 54,
        });

        public static SolidColorBrush Navy => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 0,
            G = 31,
            B = 63,
        });

        public static SolidColorBrush Maroon => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 133,
            G = 20,
            B = 75,
        });

        public static SolidColorBrush Fuchsia => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 240,
            G = 18,
            B = 190,
        });

        public static SolidColorBrush Purple => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 177,
            G = 13,
            B = 201,
        });

        public static SolidColorBrush Silver => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 221,
            G = 221,
            B = 221,
        });

        public static SolidColorBrush Gray => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 170,
            G = 170,
            B = 170,
        });

        public static SolidColorBrush Black => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 17,
            G = 17,
            B = 17,
        });


        public static SolidColorBrush AppDefaultGreen => new SolidColorBrush(new Color()
        {
            A = 255,
            R = 20,
            G = 179,
            B = 155,
        });

        public static List<SolidColorBrush> RiBrushCollection => _colltor.Value;
        private static Lazy<List<SolidColorBrush>> _colltor = null;

        static NewBrush()
        {
            _colltor = new Lazy<List<SolidColorBrush>>(() =>
            {
                return new List<SolidColorBrush>
                {
                    Blue,
                    Aqua,
                    Teal,
                    Olive,
                    Green,
                    Lime,
                    Yellow,
                    Orange,
                    Red,
                    Navy,
                    Maroon,
                    Fuchsia,
                    Purple,
                    Silver,
                    Gray,
                    Black,
                    AppDefaultGreen,
                };
            });
        }
    }
}
