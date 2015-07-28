using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buddhabrot.Models
{
    /// <summary>
    /// 実部と複部が浮動小数点数型の複素数を表します。
    /// </summary>
    public struct DoubleComplex
    {
        private double _Real;
        public double Real { get { return _Real; } set { _Real = value; } }
        private double _Imag;
        public double Imag { get { return _Imag; } set { _Imag = value; } }

        public DoubleComplex(double real, double imag)
        {
            _Real = real; _Imag = imag;
        }

        public static DoubleComplex operator +(DoubleComplex c1, DoubleComplex c2)
        {
            return new DoubleComplex(c1.Real + c2.Real, c1.Imag + c2.Imag);
        }
        public static DoubleComplex operator *(DoubleComplex c1, DoubleComplex c2)
        {
            return new DoubleComplex(c1.Real * c2.Real - c1.Imag * c2.Imag, c1.Imag * c2.Real + c1.Real * c2.Imag);
        }

        public static DoubleComplex operator -(DoubleComplex c1, DoubleComplex c2)
        {
            return new DoubleComplex(c1.Real - c2.Real, c1.Imag - c2.Imag);
        }

        public static DoubleComplex operator /(DoubleComplex c1, DoubleComplex c2)
        {
            double re = (c1.Real * c2.Real + c1.Imag * c2.Imag);
            double im = (c1.Real * c2.Imag + c1.Imag * c2.Real);
            double m = c2.Real * c2.Real + c2.Imag * c2.Imag;
            return new DoubleComplex(re / m, im / m);
        }

        public double Norm()
        {
            return Real * Real + Imag * Imag;
        }

    }
}
