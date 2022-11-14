using ComputerGraphics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Transformations
{
    public abstract class TransformationBase : MyNotifyPropertyChanged
    {
        public abstract Transformation Transform { get; }

        public static implicit operator Transformation(TransformationBase tr) => tr.Transform;
    }
}
