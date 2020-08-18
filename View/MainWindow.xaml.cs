using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;

namespace Demo.Lottery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            var vm = DataContext as MainVM;
            vm.CloseAction += Close;
            LoadLogeElements(vm);
        }

        /*
        void Debug()
        {
            //var mod = GenerateElement(1, LoadImages()[2], 0);
            //var vis = new ModelVisual3D() { Content = mod };
            //mainViewPort.Children.Add(vis);
        }

        public void MoveUser()
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                for (int i = 0; i != 2; i++)
                {
                    var betal = 1;
                    while (true)
                    {
                        var offset = 0d;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            betal = _tranlate3D.OffsetZ == 60 ? -1 : betal;
                            _tranlate3D.OffsetZ += betal;
                            offset = _tranlate3D.OffsetZ;
                            Thread.Sleep(10);
                        }));
                        if (offset == -100)
                        {
                            break;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// XY-plane
        /// </summary>
        GeometryModel3D GenerateElement(double radius, string image, double z = -10)
        {
            var mod = new GeometryModel3D();

            var geo = new MeshGeometry3D();
            var pointCount = 50;
            var step = 2 * Math.PI / pointCount;
            for (int i = 0; i != pointCount; i++)
            {
                var x = radius * Math.Cos(step * i);
                var y = radius * Math.Sin(step * i);
                geo.TextureCoordinates.Add(new Point(x, y));
                geo.Positions.Add(new Point3D(x, y, z));
            }
            for (int i = 0; i != pointCount; i++)
            {
                var a = 0;
                var b = i + 1;
                var c = (i < (pointCount - 1)) ? i + 2 : 1;

                geo.TriangleIndices.Add(a);
                geo.TriangleIndices.Add(b);
                geo.TriangleIndices.Add(c);
            }
            mod.Geometry = geo;

            var material = new DiffuseMaterial();
            var bi = new BitmapImage();
            using (var fs = new FileStream(image, FileMode.Open))
            {
                bi.BeginInit();
                bi.StreamSource = fs;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
            }
            bi.Freeze();
            material.Brush = new ImageBrush(bi);
            mod.Material = material;

            // Create transforms
            var trn = new Transform3DGroup();
            trn.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 180)));
            trn.Children.Add(_tranlate3D);
            mod.Transform = trn;
            return mod;
        }

        string[] LoadImages()
        {
            var file = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var imgFolder = Path.Combine(Path.GetDirectoryName(file), "Images");
            return Directory.GetFiles(imgFolder);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveUser();
        }
        */

        void LoadLogeElements(MainVM vm)
        {
            vm.LoadUser();
            foreach (var item in vm.Elements)
            {
                mainViewPort.Children.Add(item.Element);
            }
        }
    }
}
