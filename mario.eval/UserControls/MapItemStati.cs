namespace Mario.Eval.UserControls
{
    using System;

    using Mario.Eval.CustomEventArgs;

    public class MapItemStati
    {
        public event EventHandler<OldNewValueEventArgs> LifeChanged;

        public event EventHandler<OldNewValueEventArgs> CoinsChanged;

        private int _coins;

        private int _life;

        private int _speed;

        public int Coins
        {
            get
            {
                return _coins;
            }

            set
            {
                if (value != _coins)
                {
                    OnCoinsChanged(_coins, value);
                    _coins = value;
                }
            }
        }

        public int Life
        {
            get
            {
                return _life;
            }

            set
            {
                if (value != _life)
                {
                    OnLifeChanged(_life, value);
                    _life = value;
                }
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }

            set
            {
                if (value != _speed)
                {
                    _speed = value;
                }
            }
        }

        protected virtual void OnCoinsChanged(int oldValue, int newValue)
        {
            var handler = CoinsChanged;
            if (handler != null)
            {
                handler(this, new OldNewValueEventArgs(oldValue, newValue));
            }
        }

        protected virtual void OnLifeChanged(int oldValue, int newValue)
        {
            var handler = LifeChanged;
            if (handler != null)
            {
                handler(this, new OldNewValueEventArgs(oldValue, newValue));
            }
        }
    }
}