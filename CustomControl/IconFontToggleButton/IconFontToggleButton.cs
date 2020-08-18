///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			7/31/2020 5:10:52 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Demo.Lottery
{
    public class IconFontToggleButton : ToggleButton
    {
        public string IconFontUnicode
        {
            get { return (string)GetValue(IconFontUnicodeProperty); }
            set { SetValue(IconFontUnicodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconFontUnicode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFontUnicodeProperty =
            DependencyProperty.Register("IconFontUnicode", typeof(string), typeof(IconFontToggleButton), new PropertyMetadata(""));

        public SolidColorBrush MouseOverColor
        {
            get { return (SolidColorBrush)GetValue(MouseOverColorProperty); }
            set { SetValue(MouseOverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverColorProperty =
            DependencyProperty.Register("MouseOverColor", typeof(SolidColorBrush), typeof(IconFontToggleButton), new PropertyMetadata(NewBrush.AppDefaultGreen));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(IconFontToggleButton), new PropertyMetadata(new CornerRadius(0)));

        static IconFontToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconFontToggleButton), new FrameworkPropertyMetadata(typeof(IconFontToggleButton)));
        }
    }
}
