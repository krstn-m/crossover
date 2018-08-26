//using System;

namespace CrossExchange
{
    public class Trade
    {
        public int Id { get; set; }
        
        public string Symbol { get; set; }

        public int NoOfShares { get; set; }

        public decimal Price { get; set; }
        //public decimal Price { get => Math.Round(Price, 2); set => Price = value; }

        public int PortfolioId { get; set; }
        
        public string Action { get; set; }
    }
}