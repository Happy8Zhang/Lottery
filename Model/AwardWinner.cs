///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/14/2020 3:15:56 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace Demo.Lottery
{
    public class AwardWinner : Employee
    {
        #region field

        #endregion

        #region property
        /// <summary>
        /// 奖项
        /// </summary>
        [Description("奖项")]
        public string AwardName { get; set; }

        /// <summary>
        /// 奖品描述
        /// </summary>
        [Description("奖品")]
        public string AwardDescription { get; set; }

        /// <summary>
        /// 获奖者头像
        /// </summary>
        public BitmapImage Logo { get; set; }
        #endregion

        #region constructor

        #endregion

        #region method

        #endregion
    }
}
