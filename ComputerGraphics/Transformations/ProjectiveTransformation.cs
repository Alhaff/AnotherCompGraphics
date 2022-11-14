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
    public class ProjectiveTransformation : TransformationBase
    {
        #region Variables
        private DragAblePoint _r0;
        private DragAblePoint _rx;
        private DragAblePoint _ry;
        #endregion

        #region Propreties
        public DragAblePoint R0Point
        {
            get => _r0;
            set
            {
                if (_r0 == null)
                {
                    _r0 = value;
                    _r0.PropertyChanged += (v,e) => OnPropertyChanged("R0"); ;
                }
                _r0 = value;
                OnPropertyChanged("R0Point");
            }
        }
        public DragAblePoint RxPoint
        {
            get => _rx;
            set
            {
                if (_rx == null)
                {
                    _rx = value;
                    _rx.PropertyChanged += (v,e) => OnPropertyChanged("Rx");
                }
                _rx = value;
                OnPropertyChanged("RxPoint");
            }
        }
        public DragAblePoint RyPoint
        {
            get => _ry;
            set
            {
                if (_ry == null)
                {
                    _ry = value;
                    _ry.PropertyChanged += (v, e) => OnPropertyChanged("Ry");
                }
                _ry = value;
                OnPropertyChanged("RyPoint");
            }
        }
        public Vector3 Rx
        {
            get
            {
                if (_rx != null)
                {
                    return _rx.Pos;
                }
                return Vector3.Zero;
            }
            set
            {
                _rx.Pos = value;
                OnPropertyChanged("Rx");
            }
        }
        public Vector3 Ry
        {
            get
            {
                if (_ry != null)
                {
                    return _ry.Pos;
                }
                return Vector3.Zero;
            }
            set
            {
                _ry.Pos = value;
                OnPropertyChanged("Ry");
            }
        }

        public Vector3 R0
        {
            get
            {
                if (_r0 != null)
                {
                    return _r0.Pos;
                }
                return Vector3.Zero;
            }
            set
            {
                _r0.Pos = value;
                OnPropertyChanged("R0");
            }
        }
        private TransformationMatrix ProjectiveMatrix
        {
            get => new TransformationMatrix
                (
                    Rx.X * Rx.Z, Rx.Y * Rx.Z, Rx.Z,
                    Ry.X * Rx.Z, Ry.Y * Ry.Z, Ry.Z,
                    R0.X * R0.Z, R0.Y * R0.Z, R0.Z
                );
        }
        #endregion
        public override Transformation Transform => v => v * ProjectiveMatrix;
    }
}
