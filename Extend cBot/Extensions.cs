using System;
using cAlgo.API;
using cAlgo.API.Internals;
using System.Globalization;



namespace cAlgo
{

    public static class Extensions
    {

        #region Identity

        public const string VERSION = "1.073";

        #endregion

        #region Enum

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

        public enum CapitalTo
        {

            Balance,
            Equity

        }

        public enum OpenTradeType
        {

            All,
            Buy,
            Sell

        }

        #endregion

        #region Helper

        public static Color ColorFromEnum(ColorNameEnum colorName)
        {

            return Color.FromName(colorName.ToString("G"));

        }

        #endregion

        #region Bars

        public static double LastGAP(this Bars thisBars, int thisDigits = 5)
        {

            return Math.Round(Math.Abs(thisBars.ClosePrices.Last(1) - thisBars.OpenPrices.Last(0)), thisDigits);

        }

        #endregion

        #region Bar

        public static double Body(this Bar thisBar, int thisDigits = 5)
        {

            return Math.Round(Math.Abs(thisBar.Close - thisBar.Open), thisDigits);

        }
 
        public static bool IsBullish(this Bar thisBar)
        {

            return thisBar.Close > thisBar.Open;

        }
      
        public static bool IsBearish(this Bar thisBar)
        {

            return thisBar.Close < thisBar.Open;

        }
      
        public static bool IsDoji(this Bar thisBar)
        {

            return thisBar.Close == thisBar.Open;

        }

        #endregion

        #region Symbol

        public static double DigitsToPips(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 2);

        }

        public static double PipsToDigits(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);

        }

        public static double RealSpread(this Symbol thisSymbol)
        {

            return Math.Round(thisSymbol.Spread / thisSymbol.PipSize, 2);

        }

        #endregion

        #region TimeFrame

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

        public static double ToDouble(this DateTime thisDateTime, string Culture = "en-EN")
        {

            string nowHour = (thisDateTime.Hour < 10) ? string.Format("0{0}", thisDateTime.Hour) : string.Format("{0}", thisDateTime.Hour);
            string nowMinute = (thisDateTime.Minute < 10) ? string.Format("0{0}", thisDateTime.Minute) : string.Format("{0}", thisDateTime.Minute);

            return string.Format("{0}.{1}", nowHour, nowMinute).ToDouble(Culture);

        }

        #endregion

        #region Position

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

            private readonly IAccount _account = null;
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