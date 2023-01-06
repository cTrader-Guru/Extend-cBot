/* --> cTrader Guru
 
    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/cTraderGuru
    GitHub      : https://github.com/ctrader-guru


    Edit only   :
         
        . NAME and VERSION of this cbot according to cBot Name "public class ExtendcBot : Strategy{...}"
        . All methods of "Strategy.cs"
        . Then intialize "public void StrategyInitialize(){...}";
        . If necessary, add the configuration parameters of any indicators

*/



using System;

using cAlgo.API;
using cAlgo.API.Internals;

using cTrader.Guru.Helper;
using cTrader.Guru.Extensions;

namespace cAlgo.Robots
{

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class ExtendcBot : Strategy
    {

        #region Identity

        public const string NAME = "Extend cBot";

        public const string VERSION = "1.084";

        #endregion

        #region Enum

        public enum OpenTradeType
        {

            All,
            Buy,
            Sell

        }

        public enum LoopMode
        {

            OnTick,
            OnBar,
            OnTimer

        }

        #endregion

        #region Params

        #region Identity

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/")]
        public string ProductInfo { get; set; }

        [Parameter("Label ( Magic Name )", Group = "Identity", DefaultValue = NAME)]
        public string MyLabel { get; set; }

        [Parameter("Preset information", Group = "Identity", DefaultValue = "USDJPY 5m | 29.04.2021 to 29.04.2022 | €1000")]
        public string PresetInfo { get; set; }

        #endregion

        #region Strategy

        [Parameter("Loop Mode", Group = "Strategy", DefaultValue = LoopMode.OnTick)]
        public LoopMode MyLoopMode { get; set; }

        [Parameter("Open Trade Type", Group = "Strategy", DefaultValue = OpenTradeType.Buy)]
        public OpenTradeType MyOpenTradeType { get; set; }

        [Parameter("Stop Loss", Group = "Strategy", DefaultValue = 30, MinValue = 0, Step = 0.1)]
        public double StopLoss { get; set; }

        [Parameter("Take Profit R:R 1:?", Group = "Strategy", DefaultValue = 5, MinValue = 0, Step = 0.1)]
        public double TakeProfitRR { get; set; }

        public double TakeProfit
        {


            get { return Math.Round(StopLoss * TakeProfitRR, 1); }
        }

        [Parameter("Close On Trigger?", Group = "Strategy", DefaultValue = false)]
        public bool CloseOnTrigger { get; set; }

        [Parameter("Use BreakEven?", Group = "Strategy", DefaultValue = false)]
        public bool UseBreakEven { get; set; }

        [Parameter("Use Trailing?", Group = "Strategy", DefaultValue = false)]
        public bool UseTrailing { get; set; }

        [Parameter("Use Deviation Martingala? (bypass all)", Group = "Strategy", DefaultValue = true)]
        public bool UseDM { get; set; }

        #endregion

        #region Pausa

        [Parameter("From (18.0 = 18:00)", Group = "Pause", DefaultValue = 0, MinValue = 0, MaxValue = 23.59, Step = 0.01)]
        public double PauseFrom { get; set; }

        [Parameter("To (8.20 = 08:20)", Group = "Pause", DefaultValue = 0, MinValue = 0, MaxValue = 23.59, Step = 0.01)]
        public double PauseTo { get; set; }

        [Parameter("Close All From (zero = disabled)", Group = "Pause", DefaultValue = 0, MinValue = 0, MaxValue = 23.59, Step = 0.01)]
        public double OverClockFrom { get; set; }

        public bool IAmInPause
        {

            get
            {

                if (PauseFrom == 0 && PauseTo == 0)
                    return false;

                double now = Server.Time.ToDouble();

                bool intraday = (PauseFrom < PauseTo && now >= PauseFrom && now <= PauseTo);
                bool overnight = (PauseFrom > PauseTo && ((now >= PauseFrom && now <= 23.59) || now <= PauseTo));

                return intraday || overnight;

            }
        }

        public bool IAmInOverClock
        {

            get
            {

                if (OverClockFrom == 0)
                    return false;

                double now = Server.Time.ToDouble();

                return now >= OverClockFrom;

            }
        }

        #endregion

        #region Filters

        [Parameter("Max Spread allowed", Group = "Filters", DefaultValue = 1.5, MinValue = 0, Step = 0.1)]
        public double SpreadToTrigger { get; set; }

        [Parameter("Max GAP Allowed (pips)", Group = "Filters", DefaultValue = 2, MinValue = 0, Step = 0.1)]
        public double GAP { get; set; }

        [Parameter("Max Number of Trades", Group = "Filters", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MaxTrades { get; set; }

        [Parameter("Minimum Distance Between Trades", Group = "Filters", DefaultValue = 15, MinValue = 0, Step = 0.1)]
        public double MinDistanceTrades { get; set; }
        public double upperTrade = 0;
        public double lowerTrade = 0;
        public bool HaveAskDistance = false;
        public bool HaveBidDistance = false;

        #endregion

        #region Money Target

        [Parameter("Percentage (zero = disabled)", Group = "Money Target", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double MoneyTargetPercentage { get; set; }
        public double MoneyTarget
        {



            get { return Math.Round((Account.Balance / 100) * MoneyTargetPercentage, 2); }
        }


        [Parameter("Minimum Trades to Activation", Group = "Money Target", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MoneyTargetTrades { get; set; }

        #endregion

        #region Money Management

        [Parameter("Fixed Lots (bypass all Capital)", Group = "Money Management", DefaultValue = 0, MinValue = 0, Step = 0.01)]
        public double FixedLots { get; set; }

        [Parameter("Capital", Group = "Money Management", DefaultValue = MonenyManagement.CapitalTo.Balance)]
        public MonenyManagement.CapitalTo MyCapital { get; set; }

        [Parameter("% Risk", Group = "Money Management", DefaultValue = 1, MinValue = 0.1, Step = 0.1)]
        public double MyRisk { get; set; }

        [Parameter("Pips To Calculate ( empty = stoploss )", Group = "Money Management", DefaultValue = 30, MinValue = 0, Step = 0.1)]
        public double FakeSL { get; set; }

        [Parameter("% Max (zero = disabled)", Group = "Drawdown", DefaultValue = 0, MinValue = 0, MaxValue = 100, Step = 0.1)]
        public double DDPercentage { get; set; }

        #endregion

        #region Indicators Setup

        [Parameter("Fast", Group = "EMA", DefaultValue = 30, MinValue = 1)]
        public int PeriodFastEMA { get; set; }

        [Parameter("Slow", Group = "EMA", DefaultValue = 50, MinValue = 2)]
        public int PeriodSlowEMA { get; set; }

        /* Override Example
        [Parameter("Filter", Group = "EMA", DefaultValue = 20, MinValue = 2)]
        public override double EMAFilter { get; set; }
        */

        #endregion

        #region BreakEven

        [Parameter("Activation (zero = disabled)", Group = "Break Even", DefaultValue = 10, MinValue = 0, Step = 0.1)]
        public double BreakEvenActivation { get; set; }

        [Parameter("Distance", Group = "Break Even", DefaultValue = 1.1, MinValue = 0, Step = 0.1)]
        public double BreakEvenDistance { get; set; }

        #endregion

        #region Trailing

        [Parameter("Activation (zero = disabled)", Group = "Trailing", DefaultValue = 15, MinValue = 0, Step = 0.1)]
        public double TrailingActivation { get; set; }

        [Parameter("Distance", Group = "Trailing", DefaultValue = 10, MinValue = 1, Step = 0.1)]
        public double TrailingDistance { get; set; }

        #endregion

        #region Deviation Martingala

        [Parameter("Multiplier (zero = disabled)", Group = "Deviation Martingala", DefaultValue = 1.5, MinValue = 0, Step = 0.1)]
        public double DMMultiplier { get; set; }

        [Parameter("Max Consecutive Loss (zero = infinite)", Group = "Deviation Martingala", DefaultValue = 6, MinValue = 0, Step = 1)]
        public int DMMaxLoss { get; set; }

        #endregion

        #endregion

        #region Property

        public bool OpenedInThisBar = false;

        public double StrategyNetProfit = 0;

        public Position[] StrategyPositions = Array.Empty<Position>();

        MonenyManagement MonenyManagement1;

        public int ConsecutiveLoss = 0;

        public DateTime PreventGlitch;

        #endregion

        #region cBot Events

        public void StrategyInitialize()
        {

            FastEMA = Indicators.ExponentialMovingAverage(Bars.ClosePrices, PeriodFastEMA);
            SlowEMA = Indicators.ExponentialMovingAverage(Bars.ClosePrices, PeriodSlowEMA);

        }

        public void StrategyRun()
        {
            
            bool UsingRecovery = UseDM && DMMultiplier > 0 && ConsecutiveLoss > 0;
            bool SharedConditions = !IAmInPause && !UsingRecovery && !OpenedInThisBar && StrategyPositions.Length < MaxTrades && Bars.LastGAP(Symbol.Digits) <= Symbol.PipsToDigits(GAP) && Symbol.RealSpread() <= SpreadToTrigger;

            if (Buy && Sell)
            {

                Print("Trigger Buy and Sell, strategy error.");
                return;

            }

            MonenyManagement1 = new MonenyManagement(Account, MyCapital, MyRisk, FixedLots, FakeSL > 0 ? FakeSL : StopLoss, Symbol);
            double lotSize = MonenyManagement1.GetLotSize();

            double volumeInUnits = Symbol.QuantityToVolumeInUnits(lotSize);

            if (Buy)
            {

                if (SharedConditions && MyOpenTradeType != OpenTradeType.Sell && HaveAskDistance)
                {

                    ExecuteMarketRangeOrder(TradeType.Buy, SymbolName, volumeInUnits, 2, Ask, MyLabel, StopLoss, TakeProfit);
                    Print("Open on trigger, consecutive loss {0}", ConsecutiveLoss);

                }

            }
            else if (Sell)
            {

                if (SharedConditions && MyOpenTradeType != OpenTradeType.Buy && HaveBidDistance)
                {

                    ExecuteMarketRangeOrder(TradeType.Sell, SymbolName, volumeInUnits, 2, Bid, MyLabel, StopLoss, TakeProfit);
                    Print("Open on trigger, consecutive loss {0}", ConsecutiveLoss);

                }

            }

        }

        protected override void OnStart()
        {
            
            Print(NAME, " ", VERSION);

            Positions.Opened += OnOpenPositions;
            Positions.Closed += OnClosePositions;

            StrategyInitialize();

            if (MyLoopMode == LoopMode.OnTimer) Timer.Start(1);

        }

        protected override void OnTick()
        {

            // --> REQUIRE LICENSE GENERATOR 1.075 OR GREATER https://github.com/cTrader-Guru/License-Generator
            // --> REQUIRED : AccessRights.FullAccess
            // --> CheckLicense(NAME);
            
            bool OnMoneyTargetClose = MoneyTargetPercentage > 0 && StrategyPositions.Length >= MoneyTargetTrades && StrategyNetProfit >= MoneyTarget;

            double DDControl = Math.Round((Account.Balance / 100) * DDPercentage, 2) * -1;
            bool OnDrawDownClose = DDControl < 0 && StrategyNetProfit <= DDControl;

            bool UsingRecovery = UseDM && DMMultiplier > 0 && ConsecutiveLoss > 0;
            double DistanceMin = Symbol.PipsToDigits(MinDistanceTrades);

            lowerTrade = 0;
            upperTrade = 0;
            HaveAskDistance = true;
            HaveBidDistance = true;

            StrategyNetProfit = 0;

            StrategyPositions = Positions.FindAll(MyLabel, SymbolName);

            foreach (Position position in StrategyPositions)
            {

                if (!UsingRecovery)
                {

                    bool OnTriggerClose = CloseOnTrigger && ((Buy && position.TradeType == TradeType.Sell) || (Sell && position.TradeType == TradeType.Buy));
                    if (OnTriggerClose || OnMoneyTargetClose || OnDrawDownClose || IAmInOverClock)
                    {

                        position.Close();
                        continue;

                    }

                    if (lowerTrade == 0 || position.EntryPrice < lowerTrade)
                        lowerTrade = position.EntryPrice;
                    if (upperTrade == 0 || position.EntryPrice > upperTrade)
                        upperTrade = position.EntryPrice;

                    if (MinDistanceTrades > 0)
                    {

                        HaveAskDistance = HaveAskDistance && Math.Abs(Math.Round(Ask - position.EntryPrice, Symbol.Digits)) >= DistanceMin;

                        HaveBidDistance = HaveBidDistance && Math.Abs(Math.Round(Bid - position.EntryPrice, Symbol.Digits)) >= DistanceMin;

                    }

                    TradeResult result;

                    if (UseBreakEven)
                    {

                        result = position.BreakEven(Symbol, BreakEvenActivation, BreakEvenDistance);

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

                    }

                    if (UseTrailing)
                    {

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

                    }

                }

                StrategyNetProfit += position.NetProfit;

            }

            if(MyLoopMode == LoopMode.OnTick) StrategyRun();

        }

        protected override void OnBar()
        {

            OpenedInThisBar = false;

            if (MyLoopMode == LoopMode.OnBar) StrategyRun();

        }

        protected override void OnTimer()
        {

            base.OnTimer();

            StrategyRun();

        }

        protected override void OnStop()
        {

            Positions.Opened -= OnOpenPositions;
            Positions.Closed -= OnClosePositions;
        }

        #endregion

        #region Methods
        private void OnOpenPositions(PositionOpenedEventArgs eventArgs)
        {

            Position position = eventArgs.Position;
            if (position.SymbolName != SymbolName || position.Label != MyLabel)
                return;

            OpenedInThisBar = true;

        }
        private void OnClosePositions(PositionClosedEventArgs eventArgs)
        {

            Position position = eventArgs.Position;
            if (position.SymbolName != SymbolName || position.Label != MyLabel)
                return;

            if (PreventGlitch == Server.Time)
            {

                Print("Glitch when close position, it is probably a bug of the cTrader and not of this cBot because 2 positions (Martingala) were opened simultaneously.");
                return;

            }

            PreventGlitch = Server.Time;

            if (position.NetProfit < 0)
            {

                ConsecutiveLoss++;

                bool UseRecovery = UseDM && DMMultiplier > 0 && (DMMaxLoss == 0 || ConsecutiveLoss < DMMaxLoss);

                if (UseRecovery)
                {

                    TradeType reversed = (position.TradeType == TradeType.Sell) ? TradeType.Buy : TradeType.Sell;

                    double stops = Math.Abs(position.Pips);

                    ExecuteMarketOrder(reversed, SymbolName, Symbol.QuantityToVolumeInUnits(Math.Round(position.Quantity * DMMultiplier, 2)), MyLabel, stops, stops);
                    Print("Open Martingala Deviation, consecutive loss {0}, pips {1}", ConsecutiveLoss, stops);

                }
                else
                {

                    ConsecutiveLoss = 0;

                }

            }
            else
            {

                ConsecutiveLoss = 0;

            }

        }

        #endregion

    }

}
