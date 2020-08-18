///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/10/2020 2:12:29 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Demo.Lottery
{
    public class LogoFile
    {
        #region field
        string _logoImg = string.Empty;
        #endregion

        #region property
        public string LogoImg
        {
            get => _logoImg;
            private set
            {
                _logoImg = value;
                var fileName = Path.GetFileNameWithoutExtension(_logoImg);
                FileNameArr = fileName.Split('-');
            }
        }
        public string[] FileNameArr { get; set; }
        #endregion

        #region constructor
        public LogoFile(string logo)
        {
            LogoImg = logo;
        }
        #endregion

        #region method

        #endregion
    }
}
