namespace FinTech
{
    using ExcelDna.Integration;
    using QuantSharp;

    public static class OptionsFunctions
    {
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
            return BlackScholesOptionsFunctions.EuroCall(S, K, T, sigma, r);
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
            return BlackScholesOptionsFunctions.EuroPut(S, K, T, sigma, r);
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
            return BlackScholesOptionsFunctions.DeltaEuroCall(S, K, T, sigma, r);
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
            return BlackScholesOptionsFunctions.DeltaEuroPut(S, K, T, sigma, r);
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
            return BlackScholesOptionsFunctions.GammaEuro(S, K, T, sigma, r);
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
            return -BlackScholesOptionsFunctions.ThetaEuroCall(S, K, T, sigma, r) / 365;
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
            return -BlackScholesOptionsFunctions.ThetaEuroPut(S, K, T, sigma, r) / 365;
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
            return BlackScholesOptionsFunctions.VegaEuro(S, K, T, sigma, r) / 100;
        }
        
        [ExcelFunction(
            Name = "Options.VommaEuro",
            Description = "Vomma of an european option using B-S formula")]
        public static double VommaEuro(
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
            return BlackScholesOptionsFunctions.VommaEuro(S, K, T, sigma, r) / 100;
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
            return BlackScholesOptionsFunctions.RhoEuroCall(S, K, T, sigma, r) / 100;
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
            return BlackScholesOptionsFunctions.RhoEuroPut(S, K, T, sigma, r) / 100;
        }
        
        [ExcelFunction(
            Name = "Options.ImpVolEuroCall",
            Description = "Implied volatility of an european CALL option")]
        public static double ImpliedVolatilityEuroCall(
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
                Name = "r",
                Description = "Risk-free interest rate")]
            double r,
            
            [ExcelArgument(
                Name = "C",
                Description = "Price of the option")]
            double C)
        {
            return BlackScholesOptionsFunctions.ImpliedVolatilityEuroCall(S, K, T, r, C);
        }
        
        [ExcelFunction(
            Name = "Options.ImplVolEuroPut",
            Description = "Implied volatility of an european PUT option")]
        public static double ImpliedVolatilityEuroPut(
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
                Name = "r",
                Description = "Risk-free interest rate")]
            double r,
            
            [ExcelArgument(
                Name = "P",
                Description = "Price of the option")]
            double P)
        {
            return BlackScholesOptionsFunctions.ImpliedVolatilityEuroPut(S, K, T, r, P);
        }
    }
}