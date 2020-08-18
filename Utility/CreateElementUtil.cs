///	Copyright (c) 2020 Launch. All Rights Reserved
/// Author:			Happy
/// Time:			8/6/2020 3:01:39 PM
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Demo.Lottery
{
    internal class ElementUtil
    {
        #region field

        #endregion

        #region property

        #endregion

        #region constructor

        #endregion

        #region method
        /// <summary>
        /// Plane = XY 
        /// Normal=(0,0,1)
        /// Generates a model of a Circle given specified parameters
        /// <param name="radius">Radius of circle</param>
        /// <param name="logo">image to circle's material</param>
        /// <param name="center">Center position of the circle</param>
        /// <param name="resolution">Number of slices to iterate the circumference of the circle</param>
        /// <returns>A GeometryModel3D representation of the circle</returns>
        /// https://stackoverflow.com/questions/14195196/how-to-create-a-circle-in-3d-with-known-center-point-radius-and-it-is-on-a-plan
        /// </summary>
        internal static GeometryModel3D Generate(double radius, BitmapImage logo, Vector3D center, int resolution)
        {
            var mod = new GeometryModel3D();
            var geo = new MeshGeometry3D();
            var step = 2 * Math.PI / resolution;
            for (int i = 0; i != resolution; i++)
            {
                var x = radius * Math.Cos(step * i);
                var y = radius * Math.Sin(step * i);
                geo.TextureCoordinates.Add(new Point(x, y));
                geo.Positions.Add(new Point3D(x, y, 0));
            }
            for (int i = 0; i != resolution; i++)
            {
                var a = 0;
                var b = i + 1;
                var c = (i < (resolution - 1)) ? i + 2 : 1;

                geo.TriangleIndices.Add(a);
                geo.TriangleIndices.Add(b);
                geo.TriangleIndices.Add(c);
            }
            mod.Geometry = geo;

            // Create material
            var material = new DiffuseMaterial
            {
                Brush = new ImageBrush(logo)
            };
            mod.Material = material;

            // Create transforms
            var trn = new Transform3DGroup();
            trn.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 180)));
            // translate from (0,0,0) to center
            trn.Children.Add(new TranslateTransform3D(center));
            mod.Transform = trn;
            return mod;
        }
        #endregion
    }
}
