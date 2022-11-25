
using System;
using cAlgo.API;
using cTrader.Guru.Helper;

namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class RiskCalculator : Indicator
    {

        #region Enums

        public enum _MyPosition
        {

            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft

        }

        public enum _MyBalance
        {

            Balance,
            Equity

        }

        #endregion

        #region Identity

        public const string NAME = "Risk Calculator";

        public const string VERSION = "1.0.4";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/risk-calculator/")]
        public string ProductInfo { get; set; }

        [Parameter("Capital", Group = "Params", DefaultValue = _MyBalance.Equity)]
        public _MyBalance CapitalRisk { get; set; }

        [Parameter("Pips Risk", Group = "Params", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        public double PipsRisk { get; set; }

        [Parameter("Color", Group = "Styles", DefaultValue = ColorFromEnum.ColorNameEnum.DarkGray)]
        public ColorFromEnum.ColorNameEnum Boxcolor { get; set; }

        [Parameter("Vertical Position", Group = "Styles", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment Valign { get; set; }

        [Parameter("Horizontal Position", Group = "Styles", DefaultValue = HorizontalAlignment.Right)]
        public HorizontalAlignment Halign { get; set; }

        #endregion


        #region Indicator Events

        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

        }

        public override void Calculate(int index)
        {

            if (IsLastBar)
            {

                string info = String.Format("{0} ({1} / {2} pips)", SymbolName, CapitalRisk.ToString(), PipsRisk);

                double myCapitalRisk = _getMyCapital(CapitalRisk);

                info += string.Format(" | 1% ({0:0.00})", _getLotSize(myCapitalRisk, PipsRisk, 1, 0.01, 100));
                info += string.Format(" | 3% ({0:0.00})", _getLotSize(myCapitalRisk, PipsRisk, 3, 0.01, 100));
                info += string.Format(" | 5% ({0:0.00})", _getLotSize(myCapitalRisk, PipsRisk, 5, 0.01, 100));
                info += string.Format(" | 10% ({0:0.00})", _getLotSize(myCapitalRisk, PipsRisk, 10, 0.01, 100));

                Chart.DrawStaticText("RiskCalculator", info, Valign, Halign, ColorFromEnum.GetColor(Boxcolor));

            }

        }

        #endregion

        #region Private Methods

        private double _getLotSize(double capital, double stoploss, double percentage, double Minim, double Maxi)
        {

            double moneyrisk = ((capital / 100) * percentage);

            double sl_double = (stoploss * Symbol.PipSize);

            // --> In formato 0.01 = microlotto double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

            // --> In formato volume 1K = 1000 Math.Round((moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

            double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

            if (lots < Minim)
                return Minim;

            if (lots > Maxi)
                return Maxi;

            return lots;

        }

        private double _getMyCapital(_MyBalance x)
        {

            switch (x)
            {

                case _MyBalance.Equity:

                    return Account.Equity;
                default:


                    return Account.Balance;

            }

        }

        #endregion

    }

}







