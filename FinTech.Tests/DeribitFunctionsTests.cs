namespace FinTech.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DeribitFunctionsTests
    {
        [TestCase(47000, 34000, 1.0/12, 0.85, 0.0136,
            0.0115, 1.0,
            42176, 10)]
        [TestCase(60000, 25000, 0.5, 0.65, 0.0150,
            0.0035, 1.0,
            44803, 10)]
        [TestCase(75000, 65000, 1, 0.95, 0.0150,
            0.2800, 1.0,
            72620, 20)]
        public void ShortPutLiquidationPrice_ShouldCalculateValue(
            double S,
            double K,
            double T,
            double sigma,
            double r,
            double sellPrice,
            double investmentRatio,
            
            double expected,
            double tolerance)
        {
            // Act
            var result = DeribitFunctions.ShortPutLiquidationPrice(
                S, K, T, sigma, r, sellPrice, investmentRatio);

            // Assert
            result.Should().BeApproximately(expected, tolerance);
        }
    }
}