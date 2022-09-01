using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{

    public class Strategy : License
    {

        #region Property

        public ExponentialMovingAverage FastEMA;
        public ExponentialMovingAverage SlowEMA;

        /* Override Example
        public virtual double EMAFilter { get; set; } // <-- If you want to use parameters, you have to overwrite them.
        */

        #endregion

        #region Events

        public bool TriggerBuy
        {



            get { return FastEMA.Result.Last(2) < SlowEMA.Result.Last(2) && FastEMA.Result.Last(1) > SlowEMA.Result.Last(1); }
        }

        public bool TriggerSell
        {



            get { return FastEMA.Result.Last(2) > SlowEMA.Result.Last(2) && FastEMA.Result.Last(1) < SlowEMA.Result.Last(1); }
        }

        public bool FilterBuy
        {



            get { return true; }
        }

        public bool FilterSell
        {



            get { return true; }
        }

        public bool Buy
        {



            get { return FilterBuy && TriggerBuy; }
        }

        public bool Sell
        {



            get { return FilterSell && TriggerSell; }
        }

        #endregion

    }

}
