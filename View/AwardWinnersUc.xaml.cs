///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/14/2020 10:51:28 AM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.Lottery
{
    /// <summary>
    /// Interaction logic for AwardInfoUc.xaml
    /// </summary>
    public partial class AwardWinnersUc : UserControl
    {
        public List<AwardWinner> Winners
        {
            get { return (List<AwardWinner>)GetValue(WinnersProperty); }
            set { SetValue(WinnersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Winners.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WinnersProperty =
            DependencyProperty.Register("Winners", typeof(List<AwardWinner>), typeof(AwardWinnersUc), new PropertyMetadata(null));

        public AwardWinnersUc()
        {
            InitializeComponent();
        }
    }
}
