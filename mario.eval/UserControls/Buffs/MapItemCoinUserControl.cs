namespace Mario.Eval.UserControls.Buffs
{
    public class MapItemCoinUserControl : BuffMapItemUserControl
    {
        public MapItemCoinUserControl()
            : base("ImgCoin")
        {
            ImageType = ItemImageTypeEnum.Image;
            IsBlocked = false;
        }

        protected override void BuffEnd(MapItemUserControl buffMapItemUserControl)
        {
        }

        protected override void BuffStart(MapItemUserControl mapItemUserControl)
        {
            mapItemUserControl.MapItemStati.Coins++;
        }

        protected override int SetBuffTimerTime()
        {
            return 0;
        }
    }
}