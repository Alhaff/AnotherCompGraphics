using ComputerGraphics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ComputerGraphics.DrawnObjects
{
    public class Circle : DrawnObjectBase
    {
        #region Variables

        private float _r;
        private Vector _center;
        private Vector _startBreakPoint;
        private Vector _endBreakPoint;

        #endregion

        #region Propreties

        public Vector StartBreakPoint
        {
            get { return _startBreakPoint; }
            set
            {
                if (value.Length <= R + 1E-5)
                {
                    _startBreakPoint = value;
                    OnPropertyChanged("StartBreakPoint");
                }
            }
        }

        public Vector EndBreakPoint
        {
            get { return _endBreakPoint; }
            set
            {

                if (value.Length <= R + 1E-5)
                {
                    _endBreakPoint = value;
                    OnPropertyChanged("EndBreakPoint");
                }
            }
        }

        public Vector Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Pos");
            }
        }

        public float R
        {
            get { return _r; }
            set
            {
                _r = value;
                MoveBreakPoint(Center, value);
                OnPropertyChanged("R");
            }
        }

        #endregion

        #region Constructors
        public Circle()
        {
            Center = new Vector(0, 0);
            R = 0;
            MyColor = Colors.Red;
        }

        public Circle(Vector startCoord, float r, Color color)
        {
            Center = startCoord;
            R = r;
            MyColor = color;
            StartBreakPoint = (Center + new Vector(R, 0));
            EndBreakPoint = (Center + new Vector(R, 0));
        }

        public Circle(Vector startCoord, float r, Vector startBreakPoint, Vector endBreakPoint, Color color)
        {
            Center = startCoord;
            R = r;
            StartBreakPoint = startBreakPoint;
            EndBreakPoint = endBreakPoint;
            MyColor = color;
        }
        #endregion

        #region Methods
        private void MoveBreakPoint(Vector newCenter, float newR)
        {
            var startAngle = StartBreakPoint.Angle();
            var endAngle = EndBreakPoint.Angle();
            StartBreakPoint = Line.GetLineEndPoint(newCenter, newR, startAngle);
            EndBreakPoint = Line.GetLineEndPoint(newCenter, newR, endAngle);
        }
        internal IEnumerable<Vector> GetCirclePoints()
        {
            var startAngle = StartBreakPoint.Angle() < 0 ? 2 * Math.PI - Math.Abs(StartBreakPoint.Angle()) : StartBreakPoint.Angle();
            var endAngle = EndBreakPoint.Angle() <= 0 ? 2 * Math.PI - Math.Abs(EndBreakPoint.Angle()) : EndBreakPoint.Angle();
            var start = -2 * Math.PI + Math.Max(endAngle, startAngle);
            var end = Math.Min(endAngle, startAngle);
            end = end == 0 ? 2 * Math.PI : end;

            for (double t = start; t <= end; t += Math.PI / 180)
            {
                yield return Line.GetLineEndPoint(Center, R, t);
            }

        }
        protected override IEnumerable<IEnumerable<Vector>> ObjectContourPoints()
        {
            yield return GetCirclePoints();
        }
        #endregion
    }
}
