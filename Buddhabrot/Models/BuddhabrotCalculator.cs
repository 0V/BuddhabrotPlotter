using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buddhabrot.Models
{
    public struct CalculationResult
    {
        public CalculationResult(int divergence,DoubleComplex[] result)
        {
            _Divergence = divergence; 
            _Reasult = result;
        }


        private int _Divergence;
        /// <summary>
        /// 発散までに計算した回数。-1 ならば発散していない。
        /// </summary>
        public int Divergence { get { return _Divergence; } set { _Divergence = value; } }

        public DoubleComplex[] _Reasult;
        /// <summary>
        /// 発散した場合に値が入る。
        /// </summary>
        public DoubleComplex[] Reasult { get { return _Reasult; } set { _Reasult = value; } }
    }

    public class BuddhabrotCalculator
    {
        public BuddhabrotCalculator()
        {
        }

        public BuddhabrotCalculator(int iteration)
        {
            Iteration = iteration;
        }

        public BuddhabrotCalculator(int iteration, DoubleComplex initialValue)
        {
            Iteration = iteration;
            InitialValue = initialValue;
        }

        public int _Iteration = 30;
        /// <summary>
        /// 反復回数 デフォルト30
        /// </summary>
        public int Iteration { get { return _Iteration; } set { _Iteration = value; } }

        private double _Threshold = 2.0;
        /// <summary>
        /// 収束判定に用いる閾値
        /// </summary>
        public double Threshold
        {
            get { return _Threshold; }
            set { _Threshold = value; }
        }

        /// <summary>
        /// z_0
        /// </summary>
        private DoubleComplex _InitialValue = new DoubleComplex(0, 0);
        public DoubleComplex InitialValue
        {
            get { return _InitialValue; }
            set { _InitialValue = value; }
        }

        /// <summary>
        /// z_(n+1) = (z_n)^2 - c
        /// </summary>
        public CalculationResult Calculate(double real, double imag)
        {
            return Calculate(new DoubleComplex(real, imag));
        }

        public CalculationResult Calculate(DoubleComplex c)
        {
            DoubleComplex z = InitialValue;
            var result = new CalculationResult();

            result.Reasult = new DoubleComplex[Iteration];
            double squareThreshold = Threshold * Threshold;
            for (int i = 0; i < Iteration; i++)
            {
                z = z * z + c;

                if (z.Norm() >= squareThreshold)
                {

                    result.Divergence = i;
                    return result;
                }
                result.Reasult[i] = z;
            }

            return new CalculationResult(-1,null);
        }

    }
}
