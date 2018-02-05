namespace Mario.Eval.UserControls
{
    using System.Linq;
    using System.Windows;

    using Mario.Eval.CustomEventArgs;

    public class HudUserControl : ControlBase
    {
        public HudUserControl()
        {
            Focusable = false;
        }

        public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
            "AdditionalContent",
            typeof(object),
            typeof(HudUserControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PlayerLebenProperty = DependencyProperty.Register(
            "PlayerLeben",
            typeof(string),
            typeof(HudUserControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PlayerMuenzenProperty = DependencyProperty.Register(
            "PlayerMuenzen",
            typeof(string),
            typeof(HudUserControl),
            new PropertyMetadata(default(string)));

        public object AdditionalContent
        {
            get
            {
                return (object)GetValue(AdditionalContentProperty);
            }
            set
            {
                SetValue(AdditionalContentProperty, value);
            }
        }

        public string PlayerLeben
        {
            get
            {
                return (string)GetValue(PlayerLebenProperty);
            }
            set
            {
                SetValue(PlayerLebenProperty, value);
            }
        }

        public string PlayerMuenzen
        {
            get
            {
                return (string)GetValue(PlayerMuenzenProperty);
            }
            set
            {
                SetValue(PlayerMuenzenProperty, value);
            }
        }

        protected override void EvaluateControls()
        {
        }

        protected override void OnTemplateApplied()
        {
            var map = (MapUserControl)AdditionalContent;
            PlayerLeben = map.PlayersOnMap.First().MapItemStati.Life.ToString();
            PlayerMuenzen = map.PlayersOnMap.First().MapItemStati.Coins.ToString();
        }

        protected override void WireUpControlEvents()
        {
            var map = (MapUserControl)AdditionalContent;
            map.PlayersOnMap.ForEach(x => x.MapItemStati.LifeChanged += PlayerLifeChanged);
            map.PlayersOnMap.ForEach(x => x.MapItemStati.CoinsChanged += PlayerCoinsChanged);
        }

        private void PlayerCoinsChanged(object sender, OldNewValueEventArgs args)
        {
            PlayerMuenzen = args.NewValue.ToString();
        }

        private void PlayerLifeChanged(object sender, OldNewValueEventArgs args)
        {
            PlayerLeben = args.NewValue.ToString();
        }
    }
}