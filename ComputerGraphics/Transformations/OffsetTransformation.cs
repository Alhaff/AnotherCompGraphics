using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Transformations
{
    public class OffsetTransformation : TransformationBase
    {
        #region Variables
        private float _dX = 0;
        private float _dY = 0;
        #endregion

        #region Propreties
        public float dX
        {
            get { return _dX; }
            set
            {
                _dX = value;
                OnPropertyChanged("dX");
            }
        }

        public float dY
        {
            get { return _dY; }
            set
            {
                _dY = value;
                OnPropertyChanged("dY");
            }
        }
        #endregion

        public override Transformation Transform => v => v + new System.Windows.Vector(dX, dY);
    }
}
