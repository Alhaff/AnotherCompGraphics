using ComputerGraphics.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ComputerGraphics.DrawnObjects
{
    public abstract class DrawnObjectBase : MyNotifyPropertyChanged
    {
        public int DrawPriority { get; set; } = 0;
        public Color MyColor { get; set; } = Colors.Black;
        public bool IsPlaneTransformMe { get; set; } = true;

        public event Transformation TransformMe = p => p;

        protected Vector ApplyTransformMe(Vector vect) => TransformMe(vect);
        protected abstract IEnumerable<IEnumerable<Vector>> ObjectContourPoints();

        public IEnumerable<IEnumerable<Vector>> GetContourPoints()
        {
            foreach (var countour in ObjectContourPoints())
                yield return countour.Select(point => TransformMe(point));
        }
    }
}
