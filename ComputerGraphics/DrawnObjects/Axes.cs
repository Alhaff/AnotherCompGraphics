using ComputerGraphics.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerGraphics.Helpers;
using System.Windows;
using System.Windows.Media;

namespace ComputerGraphics.DrawnObjects
{
    internal class Axes : DrawnObjectBase, IDrawingSelf
    {
        #region Variables
        private int _cellAmountOnAbscissaAxe;
        private int _cellAmountOnOrdinateAxe;
        #endregion

        #region Propreties
        public int CellAmountOnAbscissaAxe
        {
            get { return _cellAmountOnAbscissaAxe; }
            set
            {
                _cellAmountOnAbscissaAxe = value;
                OnPropertyChanged("CellAmountOnAbscissaAxe");
            }
        }

        public int CellAmountOnOrdinateAxe
        {
            get { return _cellAmountOnOrdinateAxe; }
            set
            {
                _cellAmountOnOrdinateAxe = value;
                OnPropertyChanged("CellAmountOnOrdinateAxe");
            }
        }

        public int StartXAxis { get => 0; }
        public int StartYAxis { get => 0; }

        public int EndXAxis { get => CellAmountOnAbscissaAxe + StartXAxis; }
        public int EndYAxis { get => CellAmountOnOrdinateAxe + StartYAxis; }
        #endregion

        #region Constructors

        public Axes(SceneDrawer scene)
        {
            CellAmountOnAbscissaAxe = (int)(scene.Width) / scene.StepInPixels;
            CellAmountOnOrdinateAxe = (int)(scene.Height) / scene.StepInPixels;
            MyColor = System.Windows.Media.Color.FromRgb(128, 128, 128);
        }
        /// <summary>
        ///  Створює об'єкт кооординатних вісей
        /// </summary>
        /// <param name="cellAmountOnAbscissaAxe"> Загальна кількість клітинок на осі абсцис</param>
        /// <param name="cellAmountOnOrdinateAxe">Загальна кількість клітинок на осі ординат</param>
        public Axes(int cellAmountOnAbscissaAxe, int cellAmountOnOrdinateAxe)
        {
            CellAmountOnAbscissaAxe = cellAmountOnAbscissaAxe;
            CellAmountOnOrdinateAxe = cellAmountOnOrdinateAxe;
            MyColor = System.Windows.Media.Color.FromRgb(128, 128, 128);
        }
        /// <summary>
        ///  Створює об'єкт кооординатних вісей
        /// </summary>
        /// <param name="cellAmountOnAbscissaAxe"> Загальна кількість клітинок на осі абсцис</param>
        /// <param name="cellAmountOnOrdinateAxe">Загальна кількість клітинок на осі ординат</param>
        /// <param name="color">Колір вісей</param>
        public Axes(int cellAmountOnAbscissaAxe, int cellAmountOnOrdinateAxe, System.Windows.Media.Color color)
        {
            CellAmountOnAbscissaAxe = cellAmountOnAbscissaAxe;
            CellAmountOnOrdinateAxe = cellAmountOnOrdinateAxe;
            MyColor = color;
        }
        #endregion

        #region Methods
        private Vector Point(double x, double y) => ApplyTransformMe(new Vector(x, y));
        private IEnumerable<Vector> GetAbscissaLine(int y)
        {
            //for (int x = -CellAmountOnAbscissaAxe / 2; x <= CellAmountOnAbscissaAxe / 2; x++)
            //{
            //    yield return Point(x, y);
            //}
            //Uncomment bottom lines and comment uper to reduce points amount, which optimize drawing process
            yield return Point(StartXAxis, y);
            yield return Point(EndXAxis, y);
        }
        private IEnumerable<Vector> GetOrdinateLine(int x)
        {
            //for (int y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            //{
            //    yield return Point(x, y);
            //}
            //Uncomment bottom lines and comment uper to reduce points amount, which optimize drawing process
            yield return Point(x, StartYAxis);
            yield return Point(x, EndYAxis);
        }

        private void DrawOrdinateAndAbscissaGrid(SceneDrawer scene, Graphics g)
        {
            Transformation ToBitmap = IsPlaneTransformMe ? scene.ToBitmapCoord : scene.ToBitmapCoordWithoutTransform;
            for (var x = StartXAxis; x <= EndXAxis; x++)
            {
                foreach (var points in GetOrdinateLine(x).Select(point => ToBitmap(point))
                                                         .LineCreator())
                {
                    System.Windows.Media.Color color = x != 0 ? MyColor : Colors.Blue;
                    scene.DrawLine(g, points, color);
                }
            }

            for (var y = StartYAxis; y <= EndYAxis; y++)
            {
                foreach (var points in GetAbscissaLine(y).Select(point => ToBitmap(point))
                                                         .LineCreator())
                {
                    System.Windows.Media.Color color = y != 0 ? MyColor : Colors.Blue;
                    scene.DrawLine(g, points, color);
                }
            }
            DrawElements(scene, g);
        }
        private void DrawElements(SceneDrawer scene, Graphics g)
        {
            Transformation ToBitmap = IsPlaneTransformMe ? scene.ToBitmapCoord : scene.ToBitmapCoordWithoutTransform;
            var abscissaEnd = ToBitmap(Point(EndXAxis, 0));
            var ordinateEnd = ToBitmap(Point(0, EndYAxis));
            var k = 0.6;
            var k1 = 0.4;
            var abscissaArrowUp = ToBitmap(Point(EndXAxis - k, k1));
            var abscissaArrowDown = ToBitmap(Point(EndXAxis - k, -k1));
            var ordinateArrowRight = ToBitmap(Point(k1, EndYAxis - k));
            var ordinateArrowLeft = ToBitmap(Point(-k1, EndYAxis - k));
            var temp1 = ToBitmap(Point(1, 0.3));
            var temp2 = ToBitmap(Point(1, -0.3));
            var temp3 = ToBitmap(Point(0.3, 1));
            var temp4 = ToBitmap(Point(-0.3, 1));
            var color1 = Colors.Blue;
            var color2 = Colors.Red;
            scene.DrawLine(g, Tuple.Create(abscissaEnd, abscissaArrowUp), color1);
            scene.DrawLine(g, Tuple.Create(abscissaEnd, abscissaArrowDown), color1);
            scene.DrawLine(g, Tuple.Create(ordinateEnd, ordinateArrowRight), color1);
            scene.DrawLine(g, Tuple.Create(ordinateEnd, ordinateArrowLeft), color1);
            scene.DrawLine(g, Tuple.Create(temp1, temp2), color2);
            scene.DrawLine(g, Tuple.Create(temp3, temp4), color2);
        }

        private void DrawText(SceneDrawer scene, Graphics g)
        {
            Transformation ToBitmap = IsPlaneTransformMe ? scene.ToBitmapCoord : scene.ToBitmapCoordWithoutTransform;
            var xPoint = ToBitmap(Point(EndXAxis - 1.5, 0 - 0.5));
            var yPoint = ToBitmap(Point(-2 + 0.5, EndYAxis - 0.5));
            var zeroPoint = ToBitmap(Point(0 + 0.1, 0 + 0.1));
            var onePointX = ToBitmap(Point(1 + 0.1, 0 + 0.1));
            var onePointY = ToBitmap(Point(-1 + 0.1, 1 + 0.1));
            g.DrawString("X", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, (float)xPoint.X, (float)xPoint.Y);
            g.DrawString("Y", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, (float)yPoint.X, (float)yPoint.Y);
            g.DrawString("0", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, (float)zeroPoint.X, (float)zeroPoint.Y);
            g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, (float)onePointX.X, (float)onePointX.Y);
            g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, (float)onePointY.X, (float)onePointY.Y);
        }
        public void Draw(SceneDrawer scene, Graphics g)
        {
            DrawOrdinateAndAbscissaGrid(scene, g);
            DrawText(scene, g);
        }

        protected override IEnumerable<IEnumerable<Vector>> ObjectContourPoints()
        {
            throw new NotImplementedException("Use IDrawingSelf, to draw this figure");
        }
        #endregion
    }
}
