///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/11/2020 3:32:39 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Demo.Lottery
{
    public class SettingVM : ViewModelBase
    {
        #region field
        AwardInfo _awardInfo = new AwardInfo();
        ObservableCollection<AwardInfo> _awards = new ObservableCollection<AwardInfo>();


        RelayCommand _addCmd = null;
        #endregion

        #region property
        public static readonly string AWARDINFO_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AwardInfo.xml");

        public AwardInfo Award
        {
            get => _awardInfo;
            set
            {
                SetProperty(ref _awardInfo, value, nameof(Award));
            }
        }

        public ObservableCollection<AwardInfo> Awards
        {
            get => _awards;
            set
            {
                SetProperty(ref _awards, value, nameof(Awards));
            }
        }

        public RelayCommand AddCmd
        {
            get
            {
                if (null == _addCmd)
                {
                    _addCmd = new RelayCommand(obj =>
                    {
                        AddAward();
                    });
                }
                return _addCmd;
            }
        }
        #endregion

        #region constructor
        public SettingVM()
        {
            LoadAwardInfo();
            Awards.CollectionChanged += CollectionChanged;
        }
        #endregion

        #region method
        void LoadAwardInfo()
        {
            if (!File.Exists(AWARDINFO_PATH))
            {
                return;
            }
            try
            {
                Awards = EntityXmlSerializer<ObservableCollection<AwardInfo>>.ReadFromFile(AWARDINFO_PATH);
                foreach (var item in Awards)
                {
                    item.DeleteHandle += DeleteAward;
                }
            }
            catch { }
        }

        void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateAwardInfoXml();
        }

        void AddAward()
        {
            if (_awardInfo.NumberOfWinning <= 0)
            {
                MessageBox.Show("请设置中奖人数");
                return;
            }
            _awardInfo.DeleteHandle += DeleteAward;
            _awards ??= new ObservableCollection<AwardInfo>();
            _awardInfo.Number = _awards.Count + 1;
            _awards.Add(_awardInfo);
            Awards = _awards;
            Award = new AwardInfo();
        }

        /// <summary>
        /// 删除奖项
        /// </summary>
        void DeleteAward()
        {
            var collection = Awards.Where(item => !item.Deleted);
            Awards = null == collection || !collection.Any() ?
                     new ObservableCollection<AwardInfo>() :
                     new ObservableCollection<AwardInfo>(collection);
            UpdateAwardInfoXml();
        }

        /// <summary>
        /// 保存奖项
        /// </summary>
        void UpdateAwardInfoXml()
        {
            if (0 == Awards.Count && File.Exists(AWARDINFO_PATH))
            {
                File.Delete(AWARDINFO_PATH);
            }
            for (int i = 0; i != Awards.Count; i++)
            {
                Awards[i].Number = i + 1;
            }
            EntityXmlSerializer<ObservableCollection<AwardInfo>>.XmlSerialize(AWARDINFO_PATH, Awards);
        }
        #endregion
    }
}
