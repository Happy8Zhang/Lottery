///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/5/2020 3:13:47 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Data;
using System.ComponentModel;

namespace Demo.Lottery
{
    public class MainVM : ViewModelBase
    {
        #region field
        readonly static string _executingAssemmblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        readonly string _defaultUserPath = Path.Combine(_executingAssemmblyFolder, "Images", "default.png");
        readonly static string[] _excelExtension = new string[] { ".xlsx", ".xls" };

        Random _random = new Random();
        readonly double _elementRadius = 10;
        readonly int _resolution = 50;
        readonly double _defaultZ = -500;
        int _maxX = 0;
        int _maxY = 0;
        Dictionary<bool, Action<LogoElement>> _dicElemActions = null;
        List<AwardInfo> _awards = null;
        AwardInfo _currentAward = null;
        LogoElement _tempLogoElem = null;
        AwardWinner _checkedWinner = null;
        List<AwardWinner> _winners = new List<AwardWinner>();

        /// <summary>
        /// true:开始
        /// false:停止
        /// </summary>
        bool _isEnabled = false;
        double _effect = 0d;

        RelayCommand _startCmd = null;
        RelayCommand _setCmd = null;
        RelayCommand _exportCmd = null;
        #endregion

        #region property
        /// <summary>
        /// 背景图片
        /// </summary>
        public BitmapImage BackgroundImg { get; set; }
        public CameraInfo Camera { get; set; } = CameraInfo.Instance;
        /// <summary>
        /// 所有参与用户信息
        /// </summary>
        public List<UserInfo> UserInfos { get; set; }
        /// <summary>
        /// 窗口中显示的用户
        /// </summary>
        public List<LogoElement> Elements { get; set; }
        /// <summary>
        /// 背景模糊
        /// </summary>
        public double Effect
        {
            get => _effect;
            set
            {
                SetProperty(ref _effect, value, nameof(Effect));
            }
        }
        /// <summary>
        /// 所有奖项
        /// </summary>
        public List<AwardInfo> Awards
        {
            get => _awards; set
            {
                _awards = value;
                if (null != _awards)
                {
                    _awards.Reverse();
                }
            }
        }
        /// <summary>
        /// 当前抽奖项
        /// </summary>
        public AwardInfo CurrentAward
        {
            get => _currentAward;
            set
            {
                SetProperty(ref _currentAward, value, nameof(CurrentAward));
            }
        }
        /// <summary>
        /// 每次抽奖的获奖者
        /// </summary>
        public AwardWinner Winner
        {
            get => _checkedWinner;
            set
            {
                SetProperty(ref _checkedWinner, value, nameof(Winner));
            }
        }
        /// <summary>
        /// 所有获奖者
        /// </summary>
        public List<AwardWinner> Winners
        {
            get => _winners;
            set
            {
                SetProperty(ref _winners, value, nameof(Winners));
            }
        }

        public RelayCommand StartCmd
        {
            get
            {
                if (null == _startCmd)
                {
                    _startCmd = new RelayCommand(obj =>
                    {
                        LotteryFlow();
                    });
                }
                return _startCmd;
            }
        }

        public RelayCommand SetCmd
        {
            get
            {
                if (null == _setCmd)
                {
                    _setCmd = new RelayCommand(obj =>
                    {
                        CurrentAward = null;
                        new SettingWnd().ShowDialog();
                        LoadAwards();
                    });
                }
                return _setCmd;
            }
        }

        public RelayCommand ExportCmd
        {
            get
            {
                if (null == _exportCmd)
                {
                    _exportCmd = new RelayCommand(obj =>
                    {
                        Export();
                    });
                }
                return _exportCmd;
            }
        }
        #endregion

        #region constructor
        public MainVM()
        {
            Init();
            LoadBackground();
            LoadAwards();
        }
        #endregion

        #region method
        void Init()
        {
            //视角变小，保证每个元素都可以见到
            var rad = Math.PI / 180 * ((CameraInfo.Instance.FieldOfView / 4) / 2);
            var r = Math.Abs(_defaultZ) * Math.Tan(rad);
            _maxX = (int)(CameraInfo.Instance.Position.X + r);
            _maxY = (int)(CameraInfo.Instance.Position.Y + r);

            _dicElemActions = new Dictionary<bool, Action<LogoElement>>
            {
                { true, StartAnimation },
                { false, StopAnimation},
            };
        }

        /// <summary>
        /// 背景图片
        /// </summary>
        void LoadBackground()
        {
            var backFolder = Path.Combine(ImagesFolder(), "Background");
            if (!Directory.Exists(backFolder))
            {
                return;
            }
            var images = Directory.GetFiles(backFolder);
            if (0 == images.Length)
            {
                return;
            }
            BackgroundImg = ImageUtil.ConvertToBitmapImg(images[0]);
        }

        /// <summary>
        /// 所有奖项
        /// </summary>
        void LoadAwards()
        {
            Awards = EntityXmlSerializer<List<AwardInfo>>.ReadFromFile(SettingVM.AWARDINFO_PATH);
            NextAward();
        }

        /// <summary>
        /// 加载用户
        /// </summary>
        internal void LoadUser()
        {
            Elements = new List<LogoElement>();
            UserInfos = new List<UserInfo>();

            var employeeFolder = Path.Combine(ImagesFolder(), "Employee");
            if (!Directory.Exists(employeeFolder))
            {
                MessageBox.Show($"用户信息没有保存于{employeeFolder}");
                return;
            }
            var files = Directory.GetFiles(employeeFolder).ToList();
            var employeeLogos = files.FirstOrDefault(item => _excelExtension.Any(ext => item.EndsWith(ext)));
            var employeeInfo = ExcelHelper.ReadExcel(employeeLogos, true);
            if (null == employeeInfo || 0 == employeeInfo.Tables.Count)
            {
                MessageBox.Show("没有读取到用户");
                return;
            }
            files.Remove(employeeLogos);
            var logos = files.Select(item => new LogoFile(item)).ToList();
            for (int i = 0; i != employeeInfo.Tables[0].Rows.Count; i++)
            {
                var row = employeeInfo.Tables[0].Rows[i];
                var userInfo = new UserInfo
                {
                    Name = row["姓名"].ToString(),
                    Number = null == row["工号"] ? "" : row["工号"].ToString(),
                    Department = null == row["部门"] ? "" : row["部门"].ToString(),
                    Phone = null == row["电话"] ? "" : row["电话"].ToString(),
                    Company = null == row["公司"] ? "" : row["公司"].ToString(),
                };

                if (0 == logos.Count)
                {
                    userInfo.LogoPath = _defaultUserPath;
                }
                else
                {
                    MatchUserLogo(userInfo, ref logos);
                }
                UserInfos.Add(userInfo);
                Elements.Add(CreateElement(userInfo));
            }
        }

        /// <summary>
        /// 用户信息与用户头像进行匹配
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logos"></param>
        void MatchUserLogo(UserInfo user, ref List<LogoFile> logos)
        {
            var matchedLogo = logos.Select(logo =>
            {
                var weight = logo.FileNameArr.Aggregate(0, (sum, next) => sum += user.ContainBasicProperty(next) ? 1 : 0);
                return new { Weight = weight, Logo = logo };
            })?.OrderByDescending(item => item.Weight).First().Logo;
            if (null == matchedLogo)
            {
                user.LogoPath = _defaultUserPath;
            }
            else
            {
                user.LogoPath = matchedLogo.LogoImg;
                logos.Remove(matchedLogo);
            }
        }

        string ImagesFolder()
        {
            return Path.Combine(_executingAssemmblyFolder, "Images");
        }

        /// <summary>
        /// 创建用户头像，用于显示
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        LogoElement CreateElement(UserInfo user)
        {
            var center = new Vector3D(GenerateRandomPoint(_maxX), GenerateRandomPoint(_maxY), _defaultZ);
            var geometryModel = ElementUtil.Generate(_elementRadius, user.Logo, center, _resolution);
            var ttf = (geometryModel.Transform as Transform3DGroup)?.Children.FirstOrDefault(item => item is TranslateTransform3D) as TranslateTransform3D;
            var visual = new ModelVisual3D { Content = geometryModel };
            var logoElement = new LogoElement
            {
                Element = visual,
                User = user,
                TranslateTransform = ttf,
            };
            return logoElement;
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        void LotteryFlow()
        {
            _isEnabled = !_isEnabled;
            if (null == Elements)
            {
                return;
            }
            if (_isEnabled)
            {
                InitBeforeStartingLottery();
            }
            TranslateWithAnimation();
            if (!_isEnabled)
            {
                EndOfEachLottery();
            }
        }

        /// <summary>
        /// 开始抽奖前
        /// </summary>
        void InitBeforeStartingLottery()
        {
            Winner = null;
            _tempLogoElem = null;
            Effect = 0;
        }

        /// <summary>
        /// 开始/停止
        /// </summary>
        void TranslateWithAnimation()
        {
            var animation = _dicElemActions[_isEnabled];
            foreach (var element in Elements)
            {
                if (element.User.IsChecked)
                {
                    element.TranslateTransform.OffsetZ = Camera.Position.Z;
                    element.TranslateTransform.BeginAnimation(TranslateTransform3D.OffsetZProperty, null);
                    continue;
                }
                animation(element);
            }
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="element"></param>
        void StartAnimation(LogoElement element)
        {
            element.TranslateTransform.OffsetX = GenerateRandomPoint(_maxX);
            element.TranslateTransform.OffsetY = GenerateRandomPoint(_maxY);

            var beginTime = TimeSpan.FromMilliseconds(_random.Next(0, 2000));
            var duration = new System.Windows.Duration(TimeSpan.FromSeconds(1));
            element.OffsetZAnimation = new DoubleAnimation
            {
                Duration = duration,
                From = -500,
                To = 0,
                BeginTime = beginTime,
                RepeatBehavior = RepeatBehavior.Forever,
            };
            element.TranslateTransform.BeginAnimation(TranslateTransform3D.OffsetZProperty, null);
            element.TranslateTransform.BeginAnimation(TranslateTransform3D.OffsetZProperty, element.OffsetZAnimation);
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="element"></param>
        void StopAnimation(LogoElement element)
        {
            element.OffsetZAnimation.BeginTime = null;
            element.TranslateTransform.BeginAnimation(TranslateTransform3D.OffsetZProperty, element.OffsetZAnimation);

            if (null == _tempLogoElem)
            {
                _tempLogoElem = element;
            }
            else
            {
                var current = element.TranslateTransform.OffsetZ;
                var temp = _tempLogoElem.TranslateTransform.OffsetZ;
                if (current > temp)
                {
                    _tempLogoElem = element;
                }
            }
        }

        /// <summary>
        /// 每次抽奖停止后
        /// </summary>
        void EndOfEachLottery()
        {
            //停止
            Effect = 20;
            //停止，显示抽中的用户
            var winner = new AwardWinner
            {
                Logo = ImageUtil.ConvertToBitmapImg(_tempLogoElem.User?.LogoPath)
            };
            _tempLogoElem.User.CopyPropertiesValueWithPropName(winner);
            if (null != CurrentAward)
            {
                CurrentAward.NumberOfHasWon += 1;
                winner.AwardName = CurrentAward.AwardName;
                winner.AwardDescription = CurrentAward.Description;
            }
            _winners.Add(winner);
            Winners = new List<AwardWinner>(_winners);
            Winner = winner;
            NextAward();
        }

        double GenerateRandomPoint(int max)
        {
            return _random.Next(max * -10, max * 10) / 10.0;
        }

        /// <summary>
        /// 下一个奖项
        /// </summary>
        void NextAward()
        {
            CurrentAward = Awards?.FirstOrDefault(item => item.NumberOfHasWon != item.NumberOfWinning);
            RaiseProperties(nameof(CurrentAward), nameof(CurrentAward.AwardSign));
        }

        /// <summary>
        /// 导出
        /// </summary>
        void Export()
        {
            if (null == Winners || 0 == Winners.Count)
            {
                MessageBox.Show("没有中奖用户，请抽奖后再进行导导出");
                return;
            }
            var dt = new DataTable();
            var props = typeof(AwardWinner).GetTypeInfo().GetProperties();
            var columns = props.Select(prop => new { Header = prop.GetCustomAttribute<DescriptionAttribute>()?.Description, Type = prop.PropertyType, PropInfo = prop }).
                                Where(item => !string.IsNullOrEmpty(item.Header));
            foreach (var column in columns)
            {
                var col = new DataColumn(column.Header, column.Type);
                dt.Columns.Add(col);
            }
            foreach (var winner in Winners)
            {
                var row = dt.NewRow();
                foreach (var column in columns)
                {
                    row[column.Header] = column.PropInfo.GetValue(winner);
                }
                dt.Rows.Add(row);
            }
            var fileName = "获奖名单.xlsx";
            ExcelHelper.ExportExcel(dt, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName));
            MessageBox.Show($"获奖名单已保存于桌面，其文件名:{fileName}");
        }
        #endregion
    }
}
