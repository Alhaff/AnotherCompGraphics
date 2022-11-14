using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComputerGraphics.Scene;
using ComputerGraphics.DrawnObjects;
using ComputerGraphics.Converters;
using ComputerGraphics.Transformations;
using System.Diagnostics;
using System.Drawing;

namespace ComputerGraphics
{
    public delegate Vector Transformation(Vector point);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        private Dictionary<Key, Action> _keysAndActions = new Dictionary<Key, Action>();
        private int cameraMoveStepInPixels = 25;
        private double cameraScaleCoef = 0.5;

        #endregion

        #region Propreties
        private Scene.SceneDrawer scene{ get; set; }
        private WriteableBitmap SceneBitmap { get; set; }
        private Axes Axes { get; set; }

        private OffsetTransformation Offset { get; set; }

        private RotateTransformation Rotate { get; set; }

        private AffineTransformation Affine { get; set; }

        private ProjectiveTransformation Projective { get; set; }
        private TransformationConnector LinearTransformation { get; set; }
        private TransformationConnector SceneTransformation { get; set; }
        public Stopwatch Timer { get; set; } = new Stopwatch();
        private TimeSpan PreviousTick { get; set; }
        public float ElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - PreviousTick).TotalMilliseconds;
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            Offset = (OffsetTransformation)this.Resources["Offset"];
            Rotate = (RotateTransformation)this.Resources["Rotate"];
            Affine = (AffineTransformation)this.Resources["Affine"];
            Projective = (ProjectiveTransformation)this.Resources["Projective"];
            LinearTransformation = new TransformationConnector(Rotate, Offset);
            SceneTransformation = new TransformationConnector(Affine, Projective);
            _keysAndActions.Add(System.Windows.Input.Key.A, KeyA);
            _keysAndActions.Add(System.Windows.Input.Key.D, KeyD);
            _keysAndActions.Add(System.Windows.Input.Key.W, KeyW);
            _keysAndActions.Add(System.Windows.Input.Key.S, KeyS);
        }
        #region CameraControl
        void KeyA()
        {
            scene.Dx += cameraMoveStepInPixels;
        }
        void KeyD()
        {
            scene.Dx -= cameraMoveStepInPixels;
        }
        void KeyW()
        {
            scene.Dy += cameraMoveStepInPixels;
        }
        void KeyS()
        {
            scene.Dy -= cameraMoveStepInPixels;
        }

        private void Scene_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var key in _keysAndActions.Keys)
            {
                if (e.Key.Equals(key))
                {
                    _keysAndActions[key].Invoke();
                }
            }
        }

        private void Scene_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                scene.ScaleCoef += cameraScaleCoef;
            }
            else
            {
                scene.ScaleCoef -= cameraScaleCoef;
            }
        }

        private void Scene_MouseEnter(object sender, MouseEventArgs e)
        {
            Keyboard.Focus(Scene);
        }

        #endregion

        #region DragObjectSetup
        private void SceneManipulateLayer_DragOver(object sender, DragEventArgs e)
        {
            var dropPosition = e.GetPosition(SceneManipulateLayer);

            if (e.Data.GetDataPresent(typeof(Ellipse)))
            {
                var point = (Ellipse)e.Data.GetData(typeof(Ellipse));
                {
                    Canvas.SetLeft(point, dropPosition.X - point.Width / 2);
                    Canvas.SetTop(point, dropPosition.Y - point.Height / 2);
                }
            }
        }
        #endregion
        private void CreateSimpleBinding(object source, PropertyPath path, FrameworkElement target,
                              DependencyProperty prop, IValueConverter converter = null)
        {
            Binding bind = new Binding();
            bind.Source = source;
            bind.Path = path;
            if (converter != null) bind.Converter = converter;
            target.SetBinding(prop, bind);
        }
        private void CreateBindingWithTrigger(object source, PropertyPath path, FrameworkElement target,
                             DependencyProperty prop, UpdateSourceTrigger trigger, IValueConverter converter = null)
        {
            Binding bind = new Binding();
            bind.Source = source;
            bind.Path = path;
            bind.UpdateSourceTrigger = trigger;
            if (converter != null) bind.Converter = converter;
            target.SetBinding(prop, bind);
        }
        private void SetupStartScene()
        {
            CreateSimpleBinding(SceneContainer, new PropertyPath("ActualWidth"), SceneManipulateLayer, Canvas.WidthProperty);
            CreateSimpleBinding(SceneContainer, new PropertyPath("ActualHeight"), SceneManipulateLayer, Canvas.HeightProperty);
            scene = SceneDrawer.getInstance((int)this.SceneContainer.ActualWidth, (int)this.SceneContainer.ActualHeight, 20);
            SceneBitmap = BitmapFactory.New((int)scene.Width, (int)scene.Height);
            Scene.Source = SceneBitmap;
            scene.TransformScene += Affine;
            CreateBindingWithTrigger(scene, new PropertyPath("StepInPixels"), StepInPixel,TextBox.TextProperty,UpdateSourceTrigger.Explicit);
        }

        private void SetupAxes()
        {
            if (scene != null && Axes == null)
            {
                Axes = new Axes(scene);
                Axes.DrawPriority = 0;
                Axes.MyColor = Colors.Black;
                scene.DrawnObjects.Add("axes", Axes);
             }
        }

        private void Scene_Loaded(object sender, RoutedEventArgs e)
        {
            SetupStartScene();
            SetupLab1();
            Timer.Start();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void Draw()
        {
            var wr = SceneBitmap;
            var w = wr.PixelWidth;
            var h = wr.PixelHeight;
            var stride = wr.BackBufferStride;
            var pixelPtr = wr.BackBuffer;
            var bm2 = new Bitmap(w, h, stride, SceneDrawer.ConvertPixelFormat(wr.Format), pixelPtr);
            wr.Lock();

            using (var g = Graphics.FromImage(bm2))
            {
                scene.Draw(g);
            }
            wr.AddDirtyRect(new Int32Rect(0, 0, scene.Width, scene.Height));
            wr.Unlock();
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {

            if (ElapsedMillisecondsSinceLastTick >= 20)
            {
                Draw();
                PreviousTick = Timer.Elapsed;
            }
        }

       
    }
}
