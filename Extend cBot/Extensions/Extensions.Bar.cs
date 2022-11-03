
using System;
using cAlgo.API;

namespace cTrader.Guru.Extensions
{
    public static class ExtBar
    {

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

    }

}
