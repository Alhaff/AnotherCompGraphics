using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Transformations
{
    public class TransformationConnector : TransformationBase
    {
        public readonly Transformation[] Transformations;
        public TransformationConnector(params Transformation[] transformations)
        {
            Transformations = transformations;
        }
        public override Transformation Transform => v=>
        {
            foreach (var transformation in Transformations)
            {
                v = transformation(v);
            }
            return v;
        };
    }
}
