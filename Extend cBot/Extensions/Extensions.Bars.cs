
using System;
using cAlgo.API;

namespace cTrader.Guru.Extensions
{
    public static class ExtBars
    {

        public static double LastGAP(this Bars thisBars, int thisDigits = 5)
        {

            return Math.Round(Math.Abs(thisBars.ClosePrices.Last(1) - thisBars.OpenPrices.Last(0)), thisDigits);

        }

    }

}
