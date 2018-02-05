namespace Mario.Eval.UserControls.Buffs
{
    public class MapItemSpeedUpUserControl : BuffMapItemUserControl
    {
        public MapItemSpeedUpUserControl()
            : base("ImgMushroomFlash")
        {
            ImageType = ItemImageTypeEnum.Image;
            IsBlocked = false;
        }

        private const int SPEED_UP_VALUE = 3;

        protected override void BuffEnd(MapItemUserControl mapItemUserControl)
        {
            mapItemUserControl.MapItemStati.Speed -= SPEED_UP_VALUE;
        }

        protected override void BuffStart(MapItemUserControl mapItemUserControl)
        {
            mapItemUserControl.MapItemStati.Speed += SPEED_UP_VALUE;
        }

        protected override int SetBuffTimerTime()
        {
            return 2;
        }
    }
}