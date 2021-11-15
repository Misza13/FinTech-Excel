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
            var (dPlus, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return + S * Phi(dPlus)
                   - K * Math.Exp(-r * T) * Phi(dMinus);
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
            var (dPlus, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return - S * Phi(-dPlus)
                   + K * Math.Exp(-r * T) * Phi(-dMinus);
        }

        [ExcelFunction(
            Name = "Options.DeltaEuroCall",
            Description = "Delta of an european CALL option using B-S formula")]
        public static double DeltaEuroCall(
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
            var (dPlus, _) = DPlusMinus(S, K, T, sigma, r);
            return Phi(dPlus);
        }
        
        [ExcelFunction(
            Name = "Options.DeltaEuroPut",
            Description = "Delta of an european PUT option using B-S formula")]
        public static double DeltaEuroPut(
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
            var (dPlus, _) = DPlusMinus(S, K, T, sigma, r);
            return -Phi(-dPlus);
        }
        
        [ExcelFunction(
            Name = "Options.GammaEuro",
            Description = "Gamma of an european option using B-S formula")]
        public static double GammaEuro(
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
            var (dPlus, _) = DPlusMinus(S, K, T, sigma, r);
            return Norm(dPlus) / (S * sigma * Math.Sqrt(T));
        }
        
        [ExcelFunction(
            Name = "Options.ThetaEuroCall",
            Description = "Theta of an european CALL option using B-S formula")]
        public static double ThetaEuroCall(
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
            var (dPlus, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return (-S * Norm(dPlus) * sigma / (2 * Math.Sqrt(T))
                    - r * K * Math.Exp(-r * T) * Phi(dMinus)) / 365;
        }
        
        [ExcelFunction(
            Name = "Options.ThetaEuroPut",
            Description = "Theta of an european PUT option using B-S formula")]
        public static double ThetaEuroPut(
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
            var (dPlus, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return (- S * Norm(dPlus) * sigma / 2 / Math.Sqrt(T)
                    + r * K * Math.Exp(-r * T) * Phi(-dMinus)) / 365;
        }
        
        [ExcelFunction(
            Name = "Options.VegaEuro",
            Description = "Vega of an european option using B-S formula")]
        public static double VegaEuro(
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
            var (dPlus, _) = DPlusMinus(S, K, T, sigma, r);
            return S * Norm(dPlus) * Math.Sqrt(T) / 100;
        }
        
        [ExcelFunction(
            Name = "Options.RhoEuroCall",
            Description = "Rho of an european CALL option using B-S formula")]
        public static double RhoEuroCall(
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
            var (_, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return K * T * Math.Exp(-r * T) * Phi(dMinus) / 100;
        }
        
        [ExcelFunction(
            Name = "Options.RhoEuroPut",
            Description = "Rho of an european PUT option using B-S formula")]
        public static double RhoEuroPut(
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
            var (_, dMinus) = DPlusMinus(S, K, T, sigma, r);
            return -K * T * Math.Exp(-r * T) * Phi(-dMinus) / 100;
        }

        private static (double, double) DPlusMinus(double S, double K, double T, double sigma, double r)
        {
            var dPlus = (Math.Log(S / K) + (r + sigma * sigma / 2) * T) / (sigma * Math.Sqrt(T));
            var dMinus = dPlus - sigma * Math.Sqrt(T);
            return (dPlus, dMinus);
        }

        private static double Norm(double x) => Normal01.Density(x);

        private static double Phi(double x) => Normal01.CumulativeDistribution(x);
    }
}