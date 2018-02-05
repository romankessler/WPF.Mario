namespace Mario.Eval
{
    using System;

    public class Position
    {
        private int _xMin;

        private int _xMax;

        private int _yMax;

        private int _yMin;

        public Position(int xMin, int xMax, int yMin, int yMax, bool isBlocked)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;

            Width = xMax - xMin;
            Height = yMax - yMin;

            IsBlocked = isBlocked;
        }

        public Position(int xMin, int yMin)
        {
            _xMin = xMin;
            _yMin = yMin;
        }

        public int Height { get; set; }

        public bool IsBlocked { get; set; }

        public int Width { get; set; }

        public int XMax
        {
            get
            {
                return _xMax;
            }
            set
            {
                _xMax = value;
                OnXPositionChanged();
            } 
        }

        public int XMin
        {
            get
            {
                return _xMin;
            } 
            set
            {
                _xMin = value;
            }
        }

        public event EventHandler YPositionChanged;

        protected virtual void OnYPositionChanged()
        {
            var handler = YPositionChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler XPositionChanged;

        protected virtual void OnXPositionChanged()
        {
            var handler = XPositionChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public int YMax
        {
            get
            {
                return _yMax;
            }
            set
            {
                _yMax = value;
                OnYPositionChanged();
            } 
        }

        public int YMin
        {
            get
            {
                return _yMin;
            }
            set
            {
                _yMin = value;
            } 
        }

        public CollisionSideEnum CheckCollision(Position target)
        {
            // X-Achse Kollision Rechts
            if (target.XMin >= XMin
                && target.XMin <= XMax)
            {
                return CollisionSideEnum.Right;
            }

            // X-Achse Kollision Links
            if (target.XMax >= XMin
                && target.XMax <= XMax)
            {
                return CollisionSideEnum.Left;
            }

            // Y-Achse Kollision Oben
            if (target.YMax >= YMin
                && target.YMax <= YMax)
            {
                return CollisionSideEnum.Top;
            }

            // Y-Achse Kollision Unten
            if (target.YMin >= YMax
                && target.YMin <= YMin)
            {
                return CollisionSideEnum.Bottom;
            }

            return CollisionSideEnum.None;
        }

        public Position GetCopy()
        {
            return new Position(XMin, XMax, YMin, YMax, IsBlocked);
        }

        public bool HasCollision(Position target)
        {
            return HasTouch(target) && (IsBlocked && target.IsBlocked);
        }

        public bool HasTouch(Position target)
        {
            var isInXRange = (IsPointInXRange(target.XMin) || IsPointInXRange(target.XMax));
            var isInYRange = (IsPointInYRange(target.YMin) || IsPointInYRange(target.YMax));

            if (isInXRange && isInYRange)
            {
                
            }

            return (isInXRange && isInYRange);
        }

        public void MoveXPosition(int value)
        {
            XMin = XMin + value;
            XMax = XMax + value;
        }

        public void MoveYPosition(int value)
        {
            YMin = YMin + value;
            YMax = YMax + value;
        }

        public void PlaceOutOfMap()
        {
            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;
        }

        public void SetXPosition(int value)
        {
            XMin = value;
            XMax = XMin + Width;
        }

        public void SetYPosition(int value)
        {
            YMin = value;
            YMax = YMin + Height;
        }

        public override string ToString()
        {
            return string.Format("XMin: {0}, XMax: {1}, YMin: {2}, YMax: {3}", XMin, XMax, YMin, YMax);
        }

        private bool IsPointInXRange(int point)
        {
            return point >= XMin && point <= XMax;
        }

        private bool IsPointInYRange(int point)
        {
            return point >= YMin && point <= YMax;
        }
    }

    public enum CollisionSideEnum
    {
        None,

        Top,

        Bottom,

        Left,

        Right
    }
}