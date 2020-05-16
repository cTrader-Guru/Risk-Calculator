/*  CTRADER GURU --> Indicator Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;

namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class RiskCalculator : Indicator
    {

        #region Enums

        /// <summary>
        /// Enumeratore per scegliere nelle opzioni la posizione del box info
        /// </summary>
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
        
        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Risk Calculator";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.2";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/risk-calculator/")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// Il capital nel quale effettuare i calcoli
        /// </summary>
        [Parameter("Capital", Group = "Params", DefaultValue = _MyBalance.Balance)]
        public _MyBalance CapitalRisk { get; set; }

        /// <summary>
        /// Il numero di pips con il quale calcoliamo il rischio
        /// </summary>
        [Parameter("Pips Risk", Group = "Params", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        public double PipsRisk { get; set; }

        /// <summary>
        /// Il colore del font
        /// </summary>
        [Parameter("Color", Group = "Styles", DefaultValue = "DodgerBlue")]
        public string Boxcolor { get; set; }

        /// <summary>
        /// Opzione per la posizione del box info in verticale
        /// </summary>
        [Parameter("Vertical Position", Group = "Styles", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment Valign { get; set; }

        /// <summary>
        /// Opzione per la posizione del box info in orizontale
        /// </summary>
        [Parameter("Horizontal Position", Group = "Styles", DefaultValue = HorizontalAlignment.Right)]
        public HorizontalAlignment Halign { get; set; }

        #endregion

        #region Property

        // --> Qui inseriremo variabili e costanti del progetto

        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            // --> L'utente potrebbe aver inserito un colore errato
            if (Color.FromName(Boxcolor).ToArgb() == 0)
                Boxcolor = "DodgerBlue";

        }

        public override void Calculate(int index)
        {

            if (IsLastBar)
            {

                string info = String.Format("RISK CALCULATOR {0} | {1} pips\r\n", VERSION, PipsRisk);

                double myCapitalRisk = _getMyCapital(CapitalRisk);

                string risk1 = string.Format("1%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 1, 0.01, 100));
                string risk3 = string.Format("3%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 3, 0.01, 100));
                string risk5 = string.Format("5%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 5, 0.01, 100));
                string risk10 = string.Format("10%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 10, 0.01, 100));
                string risk15 = string.Format("15%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 15, 0.01, 100));
                string risk20 = string.Format("20%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 20, 0.01, 100));
                string risk25 = string.Format("25%  =  {0:0.00}", _getLotSize(myCapitalRisk, PipsRisk, 25, 0.01, 100));

                info = string.Format("{0}\r\n{1}", info, risk1);
                info = string.Format("{0}\r\n{1}", info, risk3);
                info = string.Format("{0}\r\n{1}", info, risk5);
                info = string.Format("{0}\r\n{1}", info, risk10);
                info = string.Format("{0}\r\n{1}", info, risk15);
                info = string.Format("{0}\r\n{1}", info, risk20);
                info = string.Format("{0}\r\n{1}", info, risk25);

                Chart.DrawStaticText("RiskCalculator", info, Valign, Halign, Boxcolor);

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







