namespace Mario.Eval.UserControls.Buffs
{
    using Mario.Eval.UserControls.People;

    public class MapItemDamageUserControl : BuffMapItemUserControl
    {
        public MapItemDamageUserControl()
            : base("ImgMine")
        {
            ImageType = ItemImageTypeEnum.Image;
            IsBlocked = false;
        }

        protected override void BuffEnd(MapItemUserControl buffMapItemUserControl)
        {
            var mapItemUserControl = (PlayerUserControl)buffMapItemUserControl;

            mapItemUserControl.MapItemStati.Life--;
        }

        protected override void BuffStart(MapItemUserControl mapItemUserControl)
        {
            var parent = (MapUserControl)Parent;

            var newItem = new AnimationControl("ImgGroundExplosion", 5, 5, MapItemPosition);

            parent.Children.Add(newItem);
        }

        protected override int SetBuffTimerTime()
        {
            return 0;
        }
    }
}