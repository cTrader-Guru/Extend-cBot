


/* --> CTRADER GURU
 
    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/cTraderGuru
    GitHub      : https://github.com/ctrader-guru

*/



using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using System.Globalization;



namespace cAlgo
{

    /// <summary>
    /// <para><b>cTrader Guru | Extensios</b></para>
    /// <para>A group of generic extensions that make the developer's life easier</para>
    /// </summary>
    public static class Extensions
    {

        #region Enum

        /// <summary>
        /// List of available colours to be displayed in the options
        /// </summary>
        public enum ColorNameEnum
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        /// <summary>
        /// List of capital for Money Management
        /// </summary>
        public enum CapitalTo
        {

            Balance,
            Equity

        }

        #endregion

        #region Helper

        /// <param name="colorName">The name of the colour to be converted, example : ColorNameEnum.Black</param>
        /// <returns>Color : The colour converted to the correct format</returns>
        public static Color ColorFromEnum(ColorNameEnum colorName)
        {

            return Color.FromName(colorName.ToString("G"));

        }

        #endregion

        #region Bars

        /// <summary>
        /// It searches within the bars loaded in memory for the precise opening.
        /// </summary>
        /// <param name="thisTime">The precise date and time of the candle to be selected</param>
        /// <returns>int : The index of the corresponding candle or if not found -1</returns>
        public static int GetIndexByDate(this Bars thisBars, DateTime thisTime)
        {

            for (int i = thisBars.OpenTimes.Count - 1; i >= 0; i--)
            {

                if (thisTime == thisBars.OpenTimes[i])
                    return i;

            }

            return -1;

        }

        /// <param name="thisDigits">Symbol Digits</param>
        /// <returns>double : GAP from last candle and current, example : 0,00350</returns>
        public static double LastGAP(this Bars thisBars, int thisDigits = 5)
        {

            return Math.Round(Math.Abs(thisBars.ClosePrices.Last(1) - thisBars.OpenPrices.Last(0)), thisDigits);

        }

        #endregion

        #region Bar

        /// <param name="thisDigits">Symbol Digits</param>
        /// <returns>double : The body of selected candle, example : 0,00350</returns>
        public static double Body(this Bar thisBar, int thisDigits = 5)
        {

            return Math.Round(Math.Abs(thisBar.Close - thisBar.Open), thisDigits);

        }

        /// <returns>bool : True if the candle is bullish</returns>        
        public static bool IsBullish(this Bar thisBar)
        {

            return thisBar.Close > thisBar.Open;

        }

        /// <returns>bool : True if the candle is bearish</returns>         
        public static bool IsBearish(this Bar thisBar)
        {

            return thisBar.Close < thisBar.Open;

        }

        /// <returns>bool : True if the candle is a doji</returns>         
        public static bool IsDoji(this Bar thisBar)
        {

            return thisBar.Close == thisBar.Open;

        }

        #endregion

        #region Symbol

        /// <param name="Pips">The number of pips in the format Digits</param>
        /// <returns>double : The representation of Digits in pips, example 0,00035 = 3,5 pips</returns>
        public static double DigitsToPips(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 2);

        }

        /// <param name="Pips">The number of pips in Double (2) format</param>
        /// <returns>double : The representation of Pips in Digits, example 3,5 = 0,00035 pips</returns>
        public static double PipsToDigits(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);

        }

        /// <returns>double : A correct rappresantation of Spread</returns>
        public static double RealSpread(this Symbol thisSymbol)
        {

            return Math.Round(thisSymbol.Spread / thisSymbol.PipSize, 2);

        }

        #endregion

        #region TimeFrame

        /// <returns>int : Returns the current timeframe in minutes</returns>
        public static int ToMinutes(this TimeFrame thisTimeFrame)
        {

            if (thisTimeFrame == TimeFrame.Daily)
                return 60 * 24;
            if (thisTimeFrame == TimeFrame.Day2)
                return 60 * 24 * 2;
            if (thisTimeFrame == TimeFrame.Day3)
                return 60 * 24 * 3;
            if (thisTimeFrame == TimeFrame.Hour)
                return 60;
            if (thisTimeFrame == TimeFrame.Hour12)
                return 60 * 12;
            if (thisTimeFrame == TimeFrame.Hour2)
                return 60 * 2;
            if (thisTimeFrame == TimeFrame.Hour3)
                return 60 * 3;
            if (thisTimeFrame == TimeFrame.Hour4)
                return 60 * 4;
            if (thisTimeFrame == TimeFrame.Hour6)
                return 60 * 6;
            if (thisTimeFrame == TimeFrame.Hour8)
                return 60 * 8;
            if (thisTimeFrame == TimeFrame.Minute)
                return 1;
            if (thisTimeFrame == TimeFrame.Minute10)
                return 10;
            if (thisTimeFrame == TimeFrame.Minute15)
                return 15;
            if (thisTimeFrame == TimeFrame.Minute2)
                return 2;
            if (thisTimeFrame == TimeFrame.Minute20)
                return 20;
            if (thisTimeFrame == TimeFrame.Minute3)
                return 3;
            if (thisTimeFrame == TimeFrame.Minute30)
                return 30;
            if (thisTimeFrame == TimeFrame.Minute4)
                return 4;
            if (thisTimeFrame == TimeFrame.Minute45)
                return 45;
            if (thisTimeFrame == TimeFrame.Minute5)
                return 5;
            if (thisTimeFrame == TimeFrame.Minute6)
                return 6;
            if (thisTimeFrame == TimeFrame.Minute7)
                return 7;
            if (thisTimeFrame == TimeFrame.Minute8)
                return 8;
            if (thisTimeFrame == TimeFrame.Minute9)
                return 9;
            if (thisTimeFrame == TimeFrame.Monthly)
                return 60 * 24 * 30;
            if (thisTimeFrame == TimeFrame.Weekly)
                return 60 * 24 * 7;

            return 0;

        }

        #endregion

        #region DateTime

        /// <param name="Culture">Localization of double value</param>
        /// <returns>double : Time representation in double format (example : 10:34:07 = 10,34)</returns>
        public static double ToDouble(this DateTime thisDateTime, string Culture = "en-EN")
        {

            string nowHour = (thisDateTime.Hour < 10) ? string.Format("0{0}", thisDateTime.Hour) : string.Format("{0}", thisDateTime.Hour);
            string nowMinute = (thisDateTime.Minute < 10) ? string.Format("0{0}", thisDateTime.Minute) : string.Format("{0}", thisDateTime.Minute);

            return string.Format("{0}.{1}", nowHour, nowMinute).ToDouble(Culture);

        }

        #endregion

        #region Position

        /// <param name="Symbol">The symbol for which the changes are being made</param>
        /// <param name="Activation">The number of pips to activate the control</param>
        /// <param name="Distance">The number of pips for the distance of the stoploss displacement</param>
        /// <returns>TradeResult : if no changes are made returns <b>null</b></returns>
        public static TradeResult BreakEven(this Position thisPosition, Symbol Symbol, double Activation, double Distance)
        {

            if (Activation == 0)
                return null;

            double breakeven;
            double distance = Symbol.PipsToDigits(Distance);
            double activation = Symbol.PipsToDigits(Activation);

            TradeResult result = null;

            switch (thisPosition.TradeType)
            {

                case TradeType.Buy:

                    breakeven = Math.Round(thisPosition.EntryPrice + distance, Symbol.Digits);

                    if (thisPosition.StopLoss == breakeven || thisPosition.TakeProfit == breakeven)
                        break;

                    if (Symbol.Bid > breakeven && Symbol.Bid >= thisPosition.EntryPrice + activation && (thisPosition.StopLoss == null || thisPosition.StopLoss < breakeven))
                    {

                        result = thisPosition.ModifyStopLossPrice(breakeven);

                    }

                    break;

                case TradeType.Sell:

                    breakeven = Math.Round(thisPosition.EntryPrice - distance, Symbol.Digits);

                    if (thisPosition.StopLoss == breakeven || thisPosition.TakeProfit == breakeven)
                        break;

                    if (Symbol.Bid < breakeven && Symbol.Ask <= thisPosition.EntryPrice - activation && (thisPosition.StopLoss == null || thisPosition.StopLoss > breakeven))
                    {

                        result = thisPosition.ModifyStopLossPrice(breakeven);

                    }

                    break;

            }

            return result;

        }

        /// <param name="Symbol">The symbol for which the changes are being made</param>
        /// <param name="Activation">The number of pips to activate the control</param>
        /// <param name="Distance">The number of pips for the distance of the stoploss displacement</param>
        /// <returns>TradeResult : if no changes are made returns <b>null</b></returns>
        public static TradeResult TrailingStop(this Position thisPosition, Symbol Symbol, double Activation, double Distance)
        {

            if (Activation == 0)
                return null;

            double trailing;
            double distance = Symbol.PipsToDigits(Distance);
            double activation = Symbol.PipsToDigits(Activation);

            TradeResult result = null;

            switch (thisPosition.TradeType)
            {

                case TradeType.Buy:

                    trailing = Math.Round(Symbol.Bid - distance, Symbol.Digits);

                    if (thisPosition.StopLoss == trailing || thisPosition.TakeProfit == trailing)
                        break;

                    if ((Symbol.Bid >= (thisPosition.EntryPrice + activation)) && (thisPosition.StopLoss == null || thisPosition.StopLoss < trailing))
                    {

                        result = thisPosition.ModifyStopLossPrice(trailing);

                    }

                    break;

                case TradeType.Sell:

                    trailing = Math.Round(Symbol.Ask + distance, Symbol.Digits);

                    if (thisPosition.StopLoss == trailing || thisPosition.TakeProfit == trailing)
                        break;

                    if ((Symbol.Ask <= (thisPosition.EntryPrice - activation)) && (thisPosition.StopLoss == null || thisPosition.StopLoss > trailing))
                    {

                        result = thisPosition.ModifyStopLossPrice(trailing);

                    }

                    break;

            }

            return result;

        }

        #endregion

        #region String

        /// <param name="Culture">Localization of double value</param>
        /// <returns>double : Time representation in double format (example : "10:34:07" = 10,34)</returns>
        public static double ToDouble(this string thisString, string Culture = "en-EN")
        {


            var culture = CultureInfo.GetCultureInfo(Culture);
            return double.Parse(thisString.Replace(',', '.').ToString(CultureInfo.InvariantCulture), culture);

        }

        #endregion

        #region Class

        public class MonenyManagement
        {

            private readonly double _minSize = 0.01;
            private double _percentage = 0;
            private double _fixedSize = 0;
            private double _pipToCalc = 30;

            private IAccount _account = null;
            public readonly Symbol Symbol;

            public CapitalTo CapitalType = CapitalTo.Balance;

            public double Percentage
            {

                get { return _percentage; }


                set { _percentage = (value > 0 && value <= 100) ? value : 0; }
            }

            public double FixedSize
            {

                get { return _fixedSize; }



                set { _fixedSize = (value >= _minSize) ? value : 0; }
            }

            public double PipToCalc
            {

                get { return _pipToCalc; }

                set { _pipToCalc = (value > 0) ? value : 100; }
            
            }

            public double Capital
            {

                get
                {

                    switch (CapitalType)
                    {

                        case CapitalTo.Equity:

                            return _account.Equity;
                        default:


                            return _account.Balance;

                    }

                }
            }

            public MonenyManagement(IAccount NewAccount, CapitalTo NewCapitalTo, double NewPercentage, double NewFixedSize, double NewPipToCalc, Symbol NewSymbol)
            {

                _account = NewAccount;

                Symbol = NewSymbol;

                CapitalType = NewCapitalTo;
                Percentage = NewPercentage;
                FixedSize = NewFixedSize;
                PipToCalc = NewPipToCalc;

            }

            public double GetLotSize()
            {

                if (FixedSize > 0)
                    return FixedSize;

                double moneyrisk = Capital / 100 * Percentage;

                double sl_double = PipToCalc * Symbol.PipSize;

                // --> 0.01 = microlotto double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
                // --> volume 1K = 1000 Math.Round((moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
                double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

                if (lots < _minSize)
                    return _minSize;

                return lots;

            }

        }

        #endregion

    }

}



namespace cAlgo.Robots
{

    public class Strategy : Robot
    {

        public ExponentialMovingAverage FastEMA;
        public ExponentialMovingAverage SlowEMA;

        public bool TriggerBuy
        {
            get
            {

                return FastEMA.Result.Last(2) <= SlowEMA.Result.Last(2) && FastEMA.Result.Last(1) > SlowEMA.Result.Last(1);

            }

        }

        public bool TriggerSell
        {
            get
            {

                return FastEMA.Result.Last(2) >= SlowEMA.Result.Last(2) && FastEMA.Result.Last(1) < SlowEMA.Result.Last(1);

            }

        }

        public bool FilterBuy
        {

            get
            {

                return true;

            }

        }

        public bool FilterSell
        {

            get
            {

                return true;

            }

        }

        public bool Buy
        {

            get
            {

                return FilterBuy && TriggerBuy;

            }

        }

        public bool Sell
        {

            get
            {

                return FilterSell && TriggerSell;

            }

        }

    }

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ExtendcBot : Strategy
    {

        #region Identity

        public const string NAME = "Extend cBot";

        public const string VERSION = "1.0.2";

        #endregion

        #region Params

        #region Identity

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/")]
        public string ProductInfo { get; set; }

        [Parameter("Label ( Magic Name )", Group = "Identity", DefaultValue = NAME)]
        public string MyLabel { get; set; }

        [Parameter("Preset information", Group = "Identity", DefaultValue = "Standard preset without any strategy")]
        public string PresetInfo { get; set; }

        #endregion

        #region Pausa

        [Parameter("From", Group = "Pause", DefaultValue = 18.00, MinValue = 0, MaxValue = 23.59, Step = 0.01)]
        public double PauseFrom { get; set; }

        [Parameter("To", Group = "Pause", DefaultValue = 9.00, MinValue = 0, MaxValue = 23.59, Step = 0.01)]
        public double PauseTo { get; set; }

        public bool IAmInPause
        {

            get
            {

                if (PauseFrom == 0 && PauseTo == 0) return false;

                double now = Server.Time.ToDouble();

                bool intraday = (PauseFrom < PauseTo && now >= PauseFrom && now <= PauseTo);
                bool overnight = (PauseFrom > PauseTo && ((now >= PauseFrom && now <= 23.59) || now <= PauseTo));

                return intraday || overnight;

            }

        }

        #endregion

        #region Strategy

        [Parameter("Stop Loss", Group = "Strategy", DefaultValue = 10, MinValue = 0, Step = 0.1)]
        public double StopLoss { get; set; }

        [Parameter("Take Profit R:R 1:?", Group = "Strategy", DefaultValue = 2, MinValue = 0, Step = 0.1)]
        public double TakeProfitRR { get; set; }

        public double TakeProfit
        {
            get
            {

                return Math.Round(StopLoss * TakeProfitRR, 1);

            }

        }

        [Parameter("Close On Trigger?", Group = "Strategy", DefaultValue = true)]
        public bool CloseOnTrigger { get; set; }

        [Parameter("Money Target (%, zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double MoneyTargetPercentage { get; set; }
        public double MoneyTarget
        {

            get { 
            
                return Math.Round( (Account.Balance / 100 ) * MoneyTargetPercentage, 2);

            }

        }
        
        [Parameter("Money Target Minimum Trades", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MoneyTargetTrades { get; set; }

        [Parameter("Max Spread allowed", Group = "Filters", DefaultValue = 1.5, MinValue = 0.1, Step = 0.1)]
        public double SpreadToTrigger { get; set; }

        [Parameter("Max GAP Allowed (pips)", Group = "Filters", DefaultValue = 1, MinValue = 0, Step = 0.01)]
        public double GAP { get; set; }

        [Parameter("Max Number of Trades", Group = "Filters", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MaxTrades { get; set; }

        [Parameter("Fast", Group = "EMA", DefaultValue = 5, MinValue = 1)]
        public int PeriodFastEMA { get; set; }

        [Parameter("Slow", Group = "EMA", DefaultValue = 9, MinValue = 2)]
        public int PeriodSlowEMA { get; set; }

        #endregion

        #region Money Management

        [Parameter("Fixed Lots (bypass all Capital)", Group = "Money Management", DefaultValue = 0, MinValue = 0, Step = 0.01)]
        public double FixedLots { get; set; }

        [Parameter("Capital", Group = "Money Management", DefaultValue = Extensions.CapitalTo.Balance)]
        public Extensions.CapitalTo MyCapital { get; set; }

        [Parameter("% Risk", Group = "Money Management", DefaultValue = 1, MinValue = 0.1, Step = 0.1)]
        public double MyRisk { get; set; }

        [Parameter("Pips To Calculate ( if no stoploss, empty = '100' )", Group = "Money Management", DefaultValue = 100, MinValue = 0, Step = 0.1)]
        public double FakeSL { get; set; }

        #endregion

        #region BreakEven

        [Parameter("Activation (zero = disabled)", Group = "Break Even", DefaultValue = 5, MinValue = 0, Step = 0.1)]
        public double BreakEvenActivation { get; set; }

        [Parameter("Distance", Group = "Break Even", DefaultValue = 1.1, MinValue = 0, Step = 0.1)]
        public double BreakEvenDistance { get; set; }

        #endregion

        #region Trailing

        [Parameter("Activation (zero = disabled)", Group = "Trailing", DefaultValue = 10, MinValue = 0, Step = 0.1)]
        public double TrailingActivation { get; set; }

        [Parameter("Distance", Group = "Trailing", DefaultValue = 10, MinValue = 1, Step = 0.1)]
        public double TrailingDistance { get; set; }

        #endregion

        #endregion

        #region Property

        public bool OpenedInThisBar = false;

        public double StrategyNetProfit = 0;

        public Position[] StrategyPositions = {};

        Extensions.MonenyManagement MonenyManagement1;

        #endregion

        #region cBot Events        

        public void StrategyInitialize()
        {

            FastEMA = Indicators.ExponentialMovingAverage(Bars.ClosePrices, PeriodFastEMA);
            SlowEMA = Indicators.ExponentialMovingAverage(Bars.ClosePrices, PeriodSlowEMA);

        }

        public void StrategyRun()
        {

            bool SharedConditions = !OpenedInThisBar && StrategyPositions.Length < MaxTrades && Bars.LastGAP(Symbol.Digits) <= Symbol.PipsToDigits(GAP) && Symbol.RealSpread() <= SpreadToTrigger;

            if (Buy && Sell)
            {

                Print("Trigger Buy and Sell, strategy error.");
                return;

            }

            MonenyManagement1 = new Extensions.MonenyManagement(Account, MyCapital, MyRisk, FixedLots, StopLoss > 0 ? StopLoss : FakeSL, Symbol);
            double lotSize = MonenyManagement1.GetLotSize();

            double volumeInUnits = Symbol.QuantityToVolumeInUnits(lotSize);

            if (Buy)
            {

                if (SharedConditions)
                {

                    ExecuteMarketRangeOrder(TradeType.Buy, SymbolName, volumeInUnits, 2, Ask, MyLabel, StopLoss, TakeProfit);
                    OpenedInThisBar = true;

                }

            }
            else if (Sell)
            {

                if (SharedConditions)
                {

                    ExecuteMarketRangeOrder(TradeType.Sell, SymbolName, volumeInUnits, 2, Bid, MyLabel, StopLoss, TakeProfit);
                    OpenedInThisBar = true;

                }

            }

        }

        protected override void OnStart()
        {

            Print(NAME, " ", VERSION);

            StrategyInitialize();


        }

        protected override void OnTick()
        {

            bool OnMoneyTargetClose = MoneyTargetPercentage > 0 && StrategyPositions.Length >= MoneyTargetTrades && StrategyNetProfit >= MoneyTarget;

            StrategyNetProfit = 0;

            StrategyPositions = Positions.FindAll(MyLabel, SymbolName);

            foreach (Position position in StrategyPositions)
            {

                bool OnTriggerClose = CloseOnTrigger && ((Buy && position.TradeType == TradeType.Sell) || (Sell && position.TradeType == TradeType.Buy));
                if (OnTriggerClose || OnMoneyTargetClose)
                {

                    position.Close();
                    continue;

                }

                TradeResult result = position.BreakEven(Symbol, BreakEvenActivation, BreakEvenDistance);

                if (result != null)
                {

                    if (result.IsSuccessful)
                    {

                        // --> Break Even successfully modified!!!

                    }
                    else
                    {

                        // --> Print("Error: {0}", result.Error);

                    }

                }

                result = position.TrailingStop(Symbol, TrailingActivation, TrailingDistance);

                if (result != null)
                {

                    if (result.IsSuccessful)
                    {

                        // --> TrailingStop successfully modified!!!

                    }
                    else
                    {

                        // --> Print("Error: {0}", result.Error);

                    }

                }

                StrategyNetProfit += position.NetProfit;

            }

            StrategyRun();

        }

        protected override void OnBar()
        {

            OpenedInThisBar = false;

        }

        protected override void OnStop()
        {



        }

        #endregion

    }

}
