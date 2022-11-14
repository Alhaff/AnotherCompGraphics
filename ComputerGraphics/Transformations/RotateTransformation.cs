using ComputerGraphics.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SceneVector = System.Windows.Vector;
namespace ComputerGraphics.Transformations
{
    public class RotateTransformation : TransformationBase
    {
        #region Variables
        private double _angle = 0;
        private double _rotateAngle =0;
        private DragAblePoint _centerPoint;
        #endregion

        #region Propreties
        public DragAblePoint CenterPoint
        {
            get => _centerPoint;
            set
            {

                if (_centerPoint == null)
                {
                    _centerPoint = value;
                    CenterPoint.PropertyChanged += (v,e) => OnPropertyChanged("Center");
                }
                _centerPoint = value;
                OnPropertyChanged("CenterPoint");
            }
        }
        public SceneVector Center
        {
            get
            {
                if (CenterPoint != null)
                {
                    return new SceneVector(CenterPoint.Pos.X, CenterPoint.Pos.Y);

                }
                return new SceneVector(0,0);
            }
            set
            {
                CenterPoint.Pos = new Vector3((float)value.X,(float)value.Y,1);
                OnPropertyChanged("Center");

            }
        }
        public double Angle
        {
            get { return (Math.Round(_angle / (Math.PI / 180))); }
            set
            {
                _angle = value * (Math.PI / 180);
                RotateAngle += _angle;
                OnPropertyChanged("Angle");
            }
        }
        private double RotateAngle
        {
            get => _rotateAngle;
            set
            {
                _rotateAngle = value;
                while (_rotateAngle >2*Math.PI)
                {
                   _rotateAngle -= 2*Math.PI;
                }
            }
        }
        private float CosAngle { get => (float)Math.Cos(RotateAngle); }
        private float SinAngle { get => (float)Math.Sin(RotateAngle); }

        private TransformationMatrix RotationMatrix
        {
            get =>
                new TransformationMatrix
                (
                     CosAngle, SinAngle, 0,
                    -1 * SinAngle, CosAngle, 0,
                    (float)(-1 * Center.X * (CosAngle - 1) + Center.Y * SinAngle),
                    (float)(-1 * Center.X * SinAngle - Center.Y * (CosAngle - 1)), 1
                );
        }
        #endregion
        public RotateTransformation()
        {

        }

        public override Transformation Transform => v =>  v * RotationMatrix;

    }
}
