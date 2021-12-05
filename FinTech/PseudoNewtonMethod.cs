namespace FinTech
{
    using System;

    public static class PseudoNewtonMethod
    {
        public static double FindRootByPseudoNewtonMethod(
            this Func<double, double> func,
            double x0,
            double epsilonWiggle = 0.01,
            double within = 0.0001,
            int maxIterations = 20)
        {
            for (var i = 0; i < maxIterations; i++)
            {
                var fx0 = func(x0);

                if (Math.Abs(fx0) < within) return x0;

                var dfx0 = (func(x0 + epsilonWiggle) - fx0) / epsilonWiggle;

                x0 -= fx0 / dfx0;
            }
            
            return x0;
        }
    }
}