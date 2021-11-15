namespace FinTech.Tests
{
    using System;
    using FluentAssertions;
    using FluentAssertions.Numeric;

    public static class AssertionExtensions
    {
        public static AndConstraint<NumericAssertions<double>> BeApproximatelyS(
            this NumericAssertions<double> parent,
            double expectedValue,
            int significantDigits)
        {
            var size = (int) Math.Floor(Math.Log10(expectedValue));
            return parent.BeApproximately(expectedValue, Math.Pow(10, size - 4));
        }
    }
}