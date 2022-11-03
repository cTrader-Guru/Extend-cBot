
using System;
using cAlgo.API.Internals;

namespace cTrader.Guru.Extensions
{

    public static class ExtSymbol
    {

        public static double DigitsToPips(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 1);

        }

        public static double PipsToDigits(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);

        }

        public static double RealSpread(this Symbol thisSymbol)
        {

            return Math.Round(thisSymbol.Spread / thisSymbol.PipSize, 2);

        }

    }

}
