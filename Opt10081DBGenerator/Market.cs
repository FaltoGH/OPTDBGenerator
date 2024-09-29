namespace Opt10081DBGenerator
{
    public class Market
    {
        public int Id;
        private Market(int id)
        {
            Id = id;
        }
        public static Market KOSPI = new Market(0);
        public static Market KOSDAQ = new Market(10);
        public static Market ELW = new Market(3);
        public static Market ETF = new Market(8);
        public static Market KONEX = new Market(50);
        public static Market MutualFund = new Market(4);
        public static Market NewBuyRight = new Market(5);
        public static Market Ritz = new Market(6);
        public static Market HighYieldFund = new Market(9);
        public static Market K_OTC = new Market(30);
        public static Market ETN = new Market(60);
        public static Market ThirdMarket = new Market(33);
    }
}
