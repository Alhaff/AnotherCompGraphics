using ComputerGraphics.DrawnObjects;
using ComputerGraphics.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics.Scene
{
    public class SceneDrawer : MyNotifyPropertyChanged
    {
        #region RealizationOfSingletonPathern
        private static SceneDrawer _scene;

        private SceneDrawer(int width, int height, int stepInPixels)
        {
            Width = width;
            Height = height;
            StepInPixels = stepInPixels;
        }
        public static SceneDrawer getInstance(int width, int height, int stepInPixels)
        {
            if (_scene == null)
                _scene = new SceneDrawer(width,height,stepInPixels);
            return _scene;
        }
        #endregion

        #region Variables
        private int _stepInPixels = 20;
        private double _skaleCoef = 1.2;
        #endregion

        #region Propreties

        public Dictionary<string, DrawnObjectBase> DrawnObjects { get; private set; } = new Dictionary<string, DrawnObjectBase>();
        public int Width { get; set; }
        public int Height { get; set; }
        private double OffsetX { get => Width / 200; }
        private double OffsetY { get => Height - Height/200; }
        public int StepInPixels
        {

            get => _stepInPixels;

            set
            {
                if (value >= 10 && value <= 80)
                {
                    _stepInPixels = value;
                    OnPropertyChanged("StepInPixels");
                }
            }

        }
        public float Dx { get; set; } = 10;
        public float Dy { get; set; } = -25;

        public double ScaleCoef
        {
            get => _skaleCoef;
            set
            {
                if (value > 0) _skaleCoef = value;
            }
        }

        #endregion

        #region Events
        
        public event Transformation TransformScene = p => p;
        #endregion

        #region Methods

        internal static System.Drawing.Imaging.PixelFormat ConvertPixelFormat(System.Windows.Media.PixelFormat sourceFormat)
        {
            if (PixelFormats.Bgr24 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            if (PixelFormats.Bgra32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (PixelFormats.Bgr32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
            if (PixelFormats.Pbgra32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return new System.Drawing.Imaging.PixelFormat();
        }
        public Vector ToBitmapCoord(Vector planeCoord)
        {
            Vector transformed = TransformScene.Invoke(planeCoord);
            var x = (transformed.X * StepInPixels) / ScaleCoef + OffsetX+ Dx;
            var y = (-(transformed.Y * StepInPixels) / ScaleCoef + OffsetY) + Dy;
            return new Vector(x, y);
        }

        public Vector ToPlaneCoord(Vector bitmapCoord)
        {
            var x = ((bitmapCoord.X - Dx - OffsetX) * ScaleCoef) / StepInPixels;
            var y = -((bitmapCoord.Y - Dy - OffsetY) * ScaleCoef) / StepInPixels;
            return new Vector(x, y);
        }

        public Vector ToBitmapCoordWithoutTransform(Vector planeCoord)
        {
            var x = (planeCoord.X * StepInPixels) / ScaleCoef + OffsetX + Dx;
            var y = -(planeCoord.Y * StepInPixels) / ScaleCoef + OffsetY + Dy;
            return new Vector(x, y);
        }

       
        public void Draw(Graphics scene)
        {
            scene.Clear(System.Drawing.Color.White);
            scene.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            foreach (var obj in DrawnObjects.Values.OrderBy(drawObject => drawObject.DrawPriority))
            {
                if (obj != null)
                {
                    if (obj is IDrawingSelf)
                    {
                        ((IDrawingSelf)obj).Draw(_scene, scene);
                    }
                    else
                    {
                        DrawObj(obj, scene);
                    }
                }
            }

        }
        internal void DrawObj(DrawnObjectBase obj, Graphics scene)
        {
            var lineBuilder = new LinkedList<Vector>();
            foreach (var contour in obj.GetContourPoints())
            {
                Transformation ToBitmap = obj.IsPlaneTransformMe ? ToBitmapCoord : ToBitmapCoordWithoutTransform;
                foreach (var points in contour.Select(point => ToBitmap(point))
                                               .LineCreator())
                {
                    DrawLine(scene, points, obj.MyColor);
                }

            }
        }
        public static bool IsNormalValue(Vector point)
        {
            return double.IsFinite(point.X) && double.IsFinite(point.Y);

        }
        internal void DrawLine(Graphics scene, Tuple<Vector, Vector> points, System.Windows.Media.Color Color)
        {
            System.Drawing.Color color =
                        System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
            System.Drawing.Pen pen = new System.Drawing.Pen(new SolidBrush(color));
            var vectStart = points.Item1;
            var vectEnd = points.Item2;
            if (IsNormalValue(vectStart) && IsNormalValue(vectEnd))
            {
                PointF start = new PointF((float)vectStart.X, (float)vectStart.Y);
                PointF end = new PointF((float)vectEnd.X, (float)vectEnd.Y);
                scene.DrawLine(pen, start, end);
            }
        }
        #endregion
    }
}
