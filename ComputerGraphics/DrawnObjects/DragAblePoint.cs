using ComputerGraphics.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SceneVector = System.Windows.Vector;
namespace ComputerGraphics.DrawnObjects
{
    public class DragAblePoint : DrawnObjectBase, IDrawingSelf
    {
        #region Variables
        private Vector3 _center;
        public readonly SceneDrawer _scene;
        #endregion

        #region Propreties
        public Vector3 Pos
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Pos");
            }
        }
        public System.Windows.Shapes.Ellipse Ellipse { get; set; }

        private bool DragIsOver { get; set; } = true; 
        #endregion

        #region Constructors
        public DragAblePoint(SceneDrawer scene, float x, float y, float z = 1f)
        {
            _scene = scene;
            MyColor = System.Windows.Media.Color.FromRgb(220, 0, 0);
            _center = new Vector3(x, y, z);
            Ellipse = new System.Windows.Shapes.Ellipse();
            Ellipse.Height = 0.3 * _scene.StepInPixels;
            Ellipse.Width = 0.3 * _scene.StepInPixels;
            Ellipse.Fill = new SolidColorBrush(MyColor);
            Ellipse.MouseMove += _pointCenter_MouseMove;
            IsPlaneTransformMe = true;
        }

        public virtual void _pointCenter_MouseMove(object sender, MouseEventArgs e)
        {
            Func<int, byte> opositeColor = (par) => (byte)Math.Abs(255 - par);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragIsOver = false;
                var chosenPoint = sender as System.Windows.Shapes.Ellipse;
                var savedColor = chosenPoint.Fill;
                chosenPoint.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(MyColor.A,
                                                                      opositeColor(MyColor.R),
                                                                      opositeColor(MyColor.G),
                                                                      opositeColor(MyColor.B)
                                                                      ));
                DragDrop.DoDragDrop(Ellipse, Ellipse, DragDropEffects.Move);
                chosenPoint.Fill = savedColor;
                var plCoord = _scene.ToPlaneCoord(new System.Windows.Vector(
                                                        (float)(Canvas.GetLeft(chosenPoint) + chosenPoint.Width / 2),
                                                        (float)(Canvas.GetTop(chosenPoint) + chosenPoint.Height / 2)));
                Pos = new Vector3((float)plCoord.X,(float)plCoord.Y, Pos.Z);
                DragIsOver = true;
            }
        }

        public DragAblePoint(SceneDrawer scene, Vector3 vector) : this(scene, vector.X, vector.Y, vector.Z)
        {

        }
        #endregion

        #region Methods
        protected override IEnumerable<IEnumerable<SceneVector>> ObjectContourPoints()
        {

            throw new NotImplementedException();
        }

        public void AddPointOnCanvas(Canvas canvas)
        {
            if (!canvas.Children.Contains(Ellipse))
            {
                canvas.Children.Add(Ellipse);
                var coord = IsPlaneTransformMe ? _scene.ToBitmapCoord(new SceneVector(Pos.X,Pos.Y))
                               : _scene.ToBitmapCoordWithoutTransform(new SceneVector(Pos.X, Pos.Y));
                Ellipse.Fill = new SolidColorBrush(MyColor);
                Canvas.SetLeft(Ellipse, coord.X - Ellipse.Width / 2);
                Canvas.SetTop(Ellipse, coord.Y - Ellipse.Height / 2);
            }
        }
        public void RemovePointFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(Ellipse);
        }

        public virtual void Draw(SceneDrawer pl, Graphics g)
        {
            if (DragIsOver)
            {
                var coord = IsPlaneTransformMe ? _scene.ToBitmapCoord(ApplyTransformMe(new SceneVector(Pos.X,Pos.Y)))
                    : _scene.ToBitmapCoordWithoutTransform(ApplyTransformMe(new SceneVector(Pos.X,Pos.Y)));
                Canvas.SetLeft(Ellipse, coord.X - Ellipse.Width / 2);
                Canvas.SetTop(Ellipse, coord.Y - Ellipse.Height / 2);
            }
        }
        #endregion
    }
}
