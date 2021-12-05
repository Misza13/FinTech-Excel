using NUnit.Framework;

namespace FinTech.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;

    [TestFixture]
    public class PseudoNewtonMethodTests
    {
        [TestCaseSource(nameof(FindRootTestCaseGenerator))]
        public void FindRoot_ShouldFindRoot(
            Func<double, double> func,
            double x0,
            double expected)
        {
            // Act
            var result = func.FindRootByPseudoNewtonMethod(
                x0,
                0.01,
                0.001);

            // Assert
            result.Should().BeApproximately(expected, 0.001);
        }

        public static IEnumerable<TestCaseData> FindRootTestCaseGenerator()
        {
            Func<double, double> x2less4 = x => x * x - 4;
            
            yield return new TestCaseData(x2less4, 0, 2);
            yield return new TestCaseData(x2less4, 1.9, 2);
            yield return new TestCaseData(x2less4, 8, 2);
            yield return new TestCaseData(x2less4, 13, 2);
            
            Func<double, double> log_x = x => Math.Log(x, Math.E);
            
            yield return new TestCaseData(log_x, 0.1, 1);
            yield return new TestCaseData(log_x, 0.3, 1);
            yield return new TestCaseData(log_x, 1.4, 1);
            yield return new TestCaseData(log_x, 2, 1);
            
            Func<double, double> logistic = x => 1.0 / (1.0 + Math.Exp(-x + 0.7)) - 0.5;
            
            yield return new TestCaseData(logistic, 0.1, 0.7);
            yield return new TestCaseData(logistic, 0.6, 0.7);
            yield return new TestCaseData(logistic, 1.4, 0.7);
            yield return new TestCaseData(logistic, 2, 0.7);
        }
    }
}