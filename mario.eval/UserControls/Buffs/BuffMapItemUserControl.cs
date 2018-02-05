namespace Mario.Eval.UserControls.Buffs
{
    using System;
    using System.Windows.Threading;

    public abstract class BuffMapItemUserControl : MapItemUserControl
    {

        public int BuffTimeInSeconds { get; set; }

        protected abstract void BuffEnd(MapItemUserControl buffMapItemUserControl);


        protected abstract int SetBuffTimerTime();

        protected abstract void BuffStart(MapItemUserControl mapItemUserControl);

        private bool _buffStarted;

        protected BuffMapItemUserControl(string imageResourceName)
            : base(imageResourceName)
        {
            BuffTimeInSeconds = SetBuffTimerTime();
        }

        protected override void UseItem(MapItemUserControl mapItemUserControl)
        {
            if (!_buffStarted)
            {
                _buffStarted = true;
                MapItemUser = mapItemUserControl;
                var buffTimer = new DispatcherTimer();
                buffTimer.Interval = new TimeSpan(0, 0, 0, BuffTimeInSeconds);

                buffTimer.Tick += BuffTimerFinished;

                BuffStart(mapItemUserControl);
                HideUsedBuffItem();
                buffTimer.Start();
            }
        }

        private void HideUsedBuffItem()
        {
            Opacity = 0;
            MapItemPosition.PlaceOutOfMap();
        }

        private void BuffTimerFinished(object sender, EventArgs e)
        {
            var timer = (DispatcherTimer)sender;
            timer.IsEnabled = false;
            BuffEnd(MapItemUser);
            MapItemIsUsed = true;
        }

        private MapItemUserControl MapItemUser { get; set; }
    }
}