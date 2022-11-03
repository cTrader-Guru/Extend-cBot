
using System;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cTrader.Guru.Extensions
{

    public static class ExtPosition
    {

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

    }

}
