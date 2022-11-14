using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Transformations
{
    public class TransformationMatrix
    {
        #region Variables
        public readonly float[,] matrix = new float[3, 3];
        #endregion

        #region Constructors
        public TransformationMatrix(float[,] m)
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    matrix[i, j] = m[i, j];
                }
            }
        }
        public TransformationMatrix(params float[] values)
        {
            if (values.Length >= 9)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        matrix[i, j] = values[3 * i + j];
                    }
                }
            }
        }
        #endregion

        #region Operators
        public float this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < matrix.GetLength(0))
                {
                    if (j >= 0 && j < matrix.GetLength(1))
                    {
                        return matrix[i, j];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
        public static Vector3 operator *(Vector3 vect, TransformationMatrix Matrix)
        {
            float x = vect.X * Matrix[0, 0] + vect.Y * Matrix[1, 0] + vect.Z * Matrix[2, 0];
            float y = vect.X * Matrix[0, 1] + vect.Y * Matrix[1, 1] + vect.Z * Matrix[2, 1];
            float z = vect.X * Matrix[0, 2] + vect.Y * Matrix[1, 2] + vect.Z * Matrix[2, 2];
            x /= z;
            y /= z;
            z /= z;
            return new Vector3(x, y, 1);
        }

        public static System.Windows.Vector operator *(System.Windows.Vector vect, TransformationMatrix Matrix)
        {
            var ExtendVector = new Vector3((float)vect.X, (float)vect.Y, 1);
            float x = ExtendVector.X * Matrix[0, 0] + ExtendVector.Y * Matrix[1, 0] + ExtendVector.Z * Matrix[2, 0];
            float y = ExtendVector.X * Matrix[0, 1] + ExtendVector.Y * Matrix[1, 1] + ExtendVector.Z * Matrix[2, 1];
            float z = ExtendVector.X * Matrix[0, 2] + ExtendVector.Y * Matrix[1, 2] + ExtendVector.Z * Matrix[2, 2];
            x /= z;
            y /= z;
            z /= z;
            return new System.Windows.Vector(x, y);
        }
        public static TransformationMatrix operator *(int num, TransformationMatrix Matrix)
        {
            return new TransformationMatrix
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2]

                );
        }
        public static TransformationMatrix operator *(TransformationMatrix Matrix, int num)
        {
            return new TransformationMatrix
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2]

                );
        }
        #endregion
    }
}
