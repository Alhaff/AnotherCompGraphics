using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SceneVector = System.Windows.Vector;

namespace ComputerGraphics.Transformations
{
    public class AffineTransformation : TransformationBase
    {
        #region Variables
        private SceneVector _r0 = new SceneVector(0, 0);
        private SceneVector _rx = new SceneVector(1, 0);
        private SceneVector _ry = new SceneVector(0, 1);
        #endregion

        #region Propreties
        public SceneVector R0
        {
            get { return _r0; }
            set
            {
                _r0 = value;
                OnPropertyChanged("R0");
            }
        }

        public SceneVector Rx
        {
            get { return _rx; }
            set
            {
                _rx = value;
                OnPropertyChanged("Rx");
            }
        }

        public SceneVector Ry
        {
            get { return _ry; }
            set
            {
                _ry = value;
                OnPropertyChanged("Ry");
            }
        }

        #endregion

        public override Transformation Transform => v => R0 + (Rx * (float)v.X) + (Ry * (float)v.Y);
           
    }
}
