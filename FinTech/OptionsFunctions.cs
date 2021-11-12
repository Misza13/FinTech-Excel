namespace FinTech
{
    using System;
    using ExcelDna.Integration;
    using MathNet.Numerics.Distributions;

    public static class OptionsFunctions
    {
        private static readonly Normal Normal01 = new Normal(0, 1);
        
        [ExcelFunction(
            Name = "Options.BlackScholesEuroCall",
            Description = "Price of an european CALL option using B-S formula")]
        public static double BlackScholesEuroCall(
            [ExcelArgument(
                Name = "S",
                Description = "Price of underlying instrument")]
            double S,
            
            [ExcelArgument(
                Name = "K",
                Description = "Option strike price")]
            double K,
            
            [ExcelArgument(
                Name = "T",
                Description = "Time to option expiration (in years)")]
            double T,
            
            [ExcelArgument(
                Name = "sigma",
                Description = "Annual volatility of underlying instrument")]
            double sigma,
            
            [ExcelArgument(
                Name = "r",
                Description = "Risk-free interest rate")]
            double r)
        {
            var dPlus = (Math.Log(S / K) + (r + sigma * sigma / 2) * T) / (sigma * Math.Sqrt(T));
            var dMinus = dPlus - sigma * Math.Sqrt(T);
            return S * Phi(dPlus) -
                   K * Phi(dMinus);
        }
        
        [ExcelFunction(
            Name = "Options.BlackScholesEuroPut",
            Description = "Price of an european PUT option using B-S formula")]
        public static double BlackScholesEuroPut(
            [ExcelArgument(
                Name = "S",
                Description = "Price of underlying instrument")]
            double S,
            
            [ExcelArgument(
                Name = "K",
                Description = "Option strike price")]
            double K,
            
            [ExcelArgument(
                Name = "T",
                Description = "Time to option expiration (in years)")]
            double T,
            
            [ExcelArgument(
                Name = "sigma",
                Description = "Annual volatility of underlying instrument")]
            double sigma,
            
            [ExcelArgument(
                Name = "r",
                Description = "Risk-free interest rate")]
            double r)
        {
            var dPlus = (Math.Log(S / K) + (r + sigma * sigma / 2) * T) / (sigma * Math.Sqrt(T));
            var dMinus = dPlus - sigma * Math.Sqrt(T);
            return -S * Phi(-dPlus) +
                   K * Math.Exp(-r * T) * Phi(-dMinus);
        }

        private static double Phi(double x)
        {
            return Normal01.CumulativeDistribution(x);
        }
    }
}