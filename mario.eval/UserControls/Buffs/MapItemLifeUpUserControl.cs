namespace Mario.Eval.UserControls.Buffs
{
    public class MapItemLifeUpUserControl : BuffMapItemUserControl
    {
        public MapItemLifeUpUserControl()
            : base("ImgHeart")
        {
            ImageType = ItemImageTypeEnum.Image;
            IsBlocked = false;
        }

        protected override void BuffEnd(MapItemUserControl buffMapItemUserControl)
        {
        }

        protected override void BuffStart(MapItemUserControl mapItemUserControl)
        {
            mapItemUserControl.MapItemStati.Life++;
        }

        protected override int SetBuffTimerTime()
        {
            return 0;
        }
    }
}