using ComputerGraphics.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics
{
    public partial class MainWindow : Window
    {
        Circle circle { get; set; }

        private void SetupLab1()
        {
            SetupAxes();
            if (circle == null)
            {
                circle = new Circle();
                circle.DrawPriority = 1;
                circle.R = 5;
                circle.Center = new System.Windows.Vector(10, 10);
                circle.TransformMe += LinearTransformation;
            }
            if (scene != null)
            {
                if(Rotate.CenterPoint == null) Rotate.CenterPoint = new DragAblePoint(scene, (float)circle.Center.X, (float)circle.Center.Y);
                Rotate.CenterPoint.IsPlaneTransformMe = true;
                Rotate.CenterPoint.Pos = new Vector3((float)circle.Center.X, (float)circle.Center.Y, 1);
                Rotate.CenterPoint.AddPointOnCanvas(SceneManipulateLayer);

                if (!scene.DrawnObjects.ContainsKey("axes")) scene.DrawnObjects.Add("axes", Axes);
                if (!scene.DrawnObjects.ContainsKey("circle")) scene.DrawnObjects.Add("circle", circle);
                if (!scene.DrawnObjects.ContainsKey("RotatePoint")) scene.DrawnObjects.Add("RotatePoint", Rotate.CenterPoint);
            }
        }
        private void Lab1_Selected(object sender, RoutedEventArgs e)
        {
            SetupLab1();
        }

        private void Lab1_Unselected(object sender, RoutedEventArgs e)
        {
            if (Axes != null)
            {
                scene.DrawnObjects.Remove("axes");
                scene.DrawnObjects.Remove("circle");
                Rotate.CenterPoint.RemovePointFromCanvas(SceneManipulateLayer);
            }
        }
    }
}
