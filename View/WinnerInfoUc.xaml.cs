///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/10/2020 2:55:09 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Demo.Lottery
{
    /// <summary>
    /// Interaction logic for WinnerInfoUc.xaml
    /// </summary>
    public partial class WinnerInfoUc : UserControl
    {
        public AwardWinner Winner
        {
            get { return (AwardWinner)GetValue(WinnerProperty); }
            set { SetValue(WinnerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Winner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WinnerProperty =
            DependencyProperty.Register("Winner", typeof(AwardWinner), typeof(WinnerInfoUc), new PropertyMetadata(null));

        public WinnerInfoUc()
        {
            InitializeComponent();
        }
    }
}
