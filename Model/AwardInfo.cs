///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/11/2020 3:48:17 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Demo.Lottery
{
    [Serializable]
    public class AwardInfo
    {
        #region field

        #endregion

        #region property
        [XmlAttribute(nameof(Number))]
        public int Number { get; set; }

        string _award = string.Empty;
        /// <summary>
        /// 奖项名称
        /// </summary>
        [XmlAttribute(nameof(AwardName))]
        public string AwardName
        {
            get => _award;
            set
            {
                _award = value;
            }
        }

        string _strNumOfWinning = string.Empty;
        [XmlIgnore]
        public string StrNumberOfWinning
        {
            get => _strNumOfWinning;
            set
            {
                _strNumOfWinning = value;
                NumberOfWinning = int.TryParse(value, out int number) ? number : 0;
            }
        }

        int _numOfWinning;
        /// <summary>
        /// 中奖人数
        /// </summary>
        [XmlAttribute(nameof(NumberOfWinning))]
        public int NumberOfWinning
        {
            get => _numOfWinning; set
            {
                _numOfWinning = value;
                if (0 == _numOfWinning && !string.IsNullOrEmpty(StrNumberOfWinning))
                {
                    StrNumberOfWinning = string.Empty;
                }
                if (0 != _numOfWinning && _numOfWinning.ToString() != StrNumberOfWinning)
                {
                    StrNumberOfWinning = _numOfWinning.ToString();
                }
            }
        }

        /// <summary>
        /// 已中奖人数
        /// </summary>
        [XmlAttribute(nameof(NumberOfHasWon))]
        public int NumberOfHasWon { get; set; }

        string _description = null;
        /// <summary>
        /// 奖品描述
        /// </summary>
        [XmlAttribute(nameof(Description))]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
            }
        }

        public string AwardSign
        {
            get => GetAwardSign();
        }

        //bool _canSelectAll = false;
        ///// <summary>
        ///// 是否单次全部抽完
        ///// </summary>
        //[XmlAttribute(nameof(CanSelectAll))]
        //public bool CanSelectAll
        //{
        //    get => _canSelectAll;
        //    set
        //    {
        //        _canSelectAll = value;
        //        StrSelectAll = _canSelectAll ? "是" : "否";
        //    }
        //}

        //[XmlIgnore]
        //public string StrSelectAll { get; set; }

        bool _deleted = false;
        [XmlIgnore]
        public bool Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                DeleteHandle?.Invoke();
            }
        }

        [XmlIgnore]
        public Action DeleteHandle { get; set; }
        #endregion

        #region constructor
        public AwardInfo()
        {

        }
        #endregion

        #region method
        string GetAwardSign()
        {
            var strBuilder = new StringBuilder($"{this.AwardName}\t");
            if (!string.IsNullOrEmpty(this.Description))
            {
                strBuilder.Append($"{this.Description} * {(this.NumberOfWinning - this.NumberOfHasWon)}");
            }
            else
            {
                strBuilder.Append($"{(this.NumberOfWinning - this.NumberOfHasWon)}人");
            }
            return strBuilder.ToString();
        }
        #endregion
    }
}
