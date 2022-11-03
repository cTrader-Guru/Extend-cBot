
using System;
using cAlgo.API.Internals;

namespace cTrader.Guru.Helper
{

    public class MonenyManagement
    {

        #region Enum

        public enum CapitalTo
        {

            Balance,
            Equity

        }

        #endregion

        #region Property

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

        #endregion

        #region Methods
        
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

        #endregion

    }

}
