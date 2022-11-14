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
    public class Line : DrawnObjectBase
    {
        #region Variables
        private Vector _start;
        private Vector _end;
        #endregion

        #region Properties

        public Vector Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }

        public Vector End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }
        #endregion

        #region Constructors
        public Line(Vector start, Vector end, Color color)
        {
            Start = start;
            End = end;
            MyColor = color;
        }

        public Line(Vector start, float length, double angle, Color color)
        {
            Start = start;
            End = GetLineEndPoint(start, length, angle);
            MyColor = color;
        }
        #endregion

        #region Methods
        internal IEnumerable<Vector> GetLinePoint()
        {
            yield return Start;
            yield return End;
        }

        protected override IEnumerable<IEnumerable<Vector>> ObjectContourPoints()
        {
            yield return GetLinePoint();
        }

        static public Vector GetLineEndPoint(Vector startPoint, float length, double angle = 0)
        {
            var tmp = startPoint + (new Vector(length, 0)).Rotate(angle);
            return new Vector(tmp.X, tmp.Y);
        }
        #endregion
    }
}
