namespace Mario.Eval.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class MapItemUserControl : UserControl
    {
        public event EventHandler MapItemUsed;

        public MapItemUserControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            InitializeStati();
        }

        public MapItemUserControl(string imageResourceName)
        {
            Charset = Application.Current.Resources[imageResourceName] as ImageSource;
            InitializeItemSize(Charset, ImageType);
            InitializeMapItemPosition();
            InitializeStati();
        }

        public enum ItemImageTypeEnum
        {
            Charset,

            Image
        }

        public static readonly DependencyProperty CharsetProperty = DependencyProperty.Register(
            "Charset",
            typeof(ImageSource),
            typeof(MapItemUserControl),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty ImageTypeProperty = DependencyProperty.Register(
            "ImageType",
            typeof(ItemImageTypeEnum),
            typeof(MapItemUserControl),
            new PropertyMetadata(default(ItemImageTypeEnum)));

        public static readonly DependencyProperty IsBlockedProperty = DependencyProperty.Register(
            "IsBlocked",
            typeof(bool),
            typeof(MapItemUserControl),
            new PropertyMetadata(default(bool)));

        private TranslateTransform MapItemImageOffset;

        private MapUserControl MapUserControl;

        private bool _mapItemIsUsed;

        private Position _mapItemPosition;

        public ImageSource Charset
        {
            get
            {
                return (ImageSource)GetValue(CharsetProperty);
            }
            set
            {
                SetValue(CharsetProperty, value);
            }
        }

        public ItemImageTypeEnum ImageType
        {
            get
            {
                return (ItemImageTypeEnum)GetValue(ImageTypeProperty);
            }
            set
            {
                SetValue(ImageTypeProperty, value);
            }
        }

        public bool IsBlocked
        {
            get
            {
                return (bool)GetValue(IsBlockedProperty);
            }
            set
            {
                SetValue(IsBlockedProperty, value);
            }
        }

        public bool IsMoving { get; set; }

        public ImageBrush MapItemImage { get; set; }

        public bool MapItemIsUsed
        {
            get
            {
                return _mapItemIsUsed;
            }
            set
            {
                _mapItemIsUsed = value;
                if (value)
                {
                    OnMapItemUsed();
                }
            }
        }

        public Position MapItemPosition
        {
            get
            {
                return _mapItemPosition;
            }
            set
            {
                if (MapItemPosition != value)
                {
                    SetYPositionOnMap(value);
                    SetXPositionOnMap(value);

                    if (MapItemPosition != null)
                    {
                        MapItemPosition.XPositionChanged -= MapItemPositionOnXPositionChanged;
                        MapItemPosition.YPositionChanged -= MapItemPositionOnYPositionChanged;
                    }

                    _mapItemPosition = value;
                    MapItemPosition.XPositionChanged += MapItemPositionOnXPositionChanged;
                    MapItemPosition.YPositionChanged += MapItemPositionOnYPositionChanged;
                }
            }
        }

        public MapItemStati MapItemStati { get; set; }

        public int PlayerImageFrame { get; set; }

        public DateTime PlayerImageFrameLastSkip { get; set; }

        public DateTime PlayerImageLastFrame { get; set; }

        public int XPosition
        {
            get
            {
                if (MapItemPosition != null)
                {
                    return (int)MapItemPosition.XMin;
                }
                return 0;
            }
            set
            {
                if (MapItemPosition == null)
                {
                    MapItemPosition = new Position(value, 0);
                }
                MapItemPosition.SetXPosition(value);
            }
        }

        public int YPosition
        {
            get
            {
                if (MapItemPosition != null)
                {
                    return (int)MapItemPosition.YMin;
                }
                return 0;
            }
            set
            {
                if (MapItemPosition == null)
                {
                    MapItemPosition = new Position(0, value);
                }
                MapItemPosition.SetYPosition(value);
            }
        }

        protected MapItemDirectionEnum MapItemDirection { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EvaluateControls();

            InitializeImageSource();
            InitializeItemSize(Charset, ImageType);

            InitializeImageSize(MapItemImage, ImageType);

            InitializeMapItemPosition();
            WireUpEvents();
        }

        public void TouchItem(MapItemUserControl element)
        {
            element.UseItem(this);
        }

        protected void EvaluateControls()
        {
            MapItemImage = (ImageBrush)Template.FindName("PART_MapItemImage", this);
            MapItemImageOffset = (TranslateTransform)Template.FindName("PART_SpriteSheetOffset", this);

            MapUserControl = (MapUserControl)Parent;
        }

        protected void InitializeImageSize(ImageBrush image, ItemImageTypeEnum imageType)
        {
            switch (imageType)
            {
                case ItemImageTypeEnum.Charset:
                    image.Viewport = new Rect(0, 0, image.ImageSource.Width, image.ImageSource.Height);
                    image.Stretch = Stretch.None;
                    break;
                case ItemImageTypeEnum.Image:
                    image.Viewport = new Rect(0, 0, Width, Height);
                    image.Stretch = Stretch.Fill;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageType");
            }

            PlayerImageFrame = 1;
        }

        protected virtual void InitializeImageSource()
        {
            if (MapItemImage == null)
            {
                MapItemImage = new ImageBrush(Charset);
            }

            MapItemImage.ImageSource = Charset;
        }

        protected void InitializeItemSize(ImageSource imageSource, ItemImageTypeEnum imageType)
        {
            switch (imageType)
            {
                case ItemImageTypeEnum.Charset:
                    Height = imageSource.Height / 4;
                    Width = imageSource.Width / 3;
                    break;
                case ItemImageTypeEnum.Image:
                    var bitmapImage = imageSource as BitmapSource;
                    Height = bitmapImage.PixelHeight;
                    Width = bitmapImage.PixelWidth;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void InitializeMapItemPosition()
        {
            int yMax;

            var xMax = (int)(XPosition + Width);

            if (IsBlocked)
            {
                yMax = (int)(YPosition + Height / 2);
            }
            else
            {
                yMax = (int)(YPosition + Height);
            }

            MapItemPosition = new Position(XPosition, xMax, YPosition, yMax, IsBlocked);
            SetDepth();
        }

        protected virtual void OnMapItemUsed()
        {
            var handler = MapItemUsed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected void SetDepth()
        {
            if (IsBlocked)
            {
                if (MapUserControl != null)
                {
                    Canvas.SetZIndex(this, (int)(MapUserControl.Height - Canvas.GetBottom(this)));
                }
            }
            else
            {
                Canvas.SetZIndex(this, 0);
            }
        }

        protected virtual void UseItem(MapItemUserControl mapItemUserControl)
        {
        }

        protected void WireUpEvents()
        {
            CompositionTarget.Rendering += OnRender;
        }

        private void CharsetImageAnimate()
        {
            if (PlayerImageFrameLastSkip + new TimeSpan(0, 0, 0, 0, 40) <= DateTime.Now)
            {
                if (ImageType == ItemImageTypeEnum.Image)
                {
                    PlayerImageFrame = 1;
                }

                PlayerImageFrameLastSkip = DateTime.Now;
                PlayerImageFrame++;

                if (PlayerImageFrame >= 5)
                {
                    PlayerImageFrame = 1;
                }
                switch (PlayerImageFrame)
                {
                    case 1:
                        MapItemImageOffset.X = 0;
                        break;
                    case 2:
                        MapItemImageOffset.X = -Width;
                        break;
                    case 3:
                        MapItemImageOffset.X = -2 * Width;
                        break;
                    case 4:
                        MapItemImageOffset.X = 0;
                        break;
                }
            }
        }

        private void ImageUseCharsetLine(TranslateTransform imageOffset, ItemImageTypeEnum imageType, int lineNumber)
        {
            if (imageType == ItemImageTypeEnum.Image)
            {
                lineNumber = 1;
            }
            switch (lineNumber)
            {
                case 1:
                    imageOffset.Y = 0;
                    break;
                case 2:
                    imageOffset.Y = -2 * Height;
                    break;
                case 3:
                    imageOffset.Y = -3 * Height;
                    break;
                case 4:
                    imageOffset.Y = -Height;
                    break;
            }
        }

        private void InitializeStati()
        {
            MapItemStati = new MapItemStati()
                               {
                                   Life = 5,
                                   Speed = 4,
                                   Coins = 0
                               };
        }

        private void MapItemPositionOnXPositionChanged(object sender, EventArgs eventArgs)
        {
            var position = (Position)sender;
            SetXPositionOnMap(position);
        }

        private void MapItemPositionOnYPositionChanged(object sender, EventArgs eventArgs)
        {
            var position = (Position)sender;
            SetYPositionOnMap(position);
        }

        private void MoveMapItem()
        {
            const int FRAMES_PER_SECOND = 100;
            const int FRAME_TIME_IN_MILI_SECONDS = (int)1000 / FRAMES_PER_SECOND;

            if (PlayerImageLastFrame + new TimeSpan(0, 0, 0, 0, FRAME_TIME_IN_MILI_SECONDS) <= DateTime.Now)
            {
                PlayerImageLastFrame = DateTime.Now;
                switch (MapItemDirection)
                {
                    case MapItemDirectionEnum.Up:
                        if (IsMoving)
                        {
                            CharsetImageAnimate();
                            ImageUseCharsetLine(MapItemImageOffset, ImageType, 1);
                            MoveStepYAxis(1);
                        }
                        else
                        {
                            SetPlayerImageAnimationToStartingPosition();
                        }
                        break;
                    case MapItemDirectionEnum.Down:
                        if (IsMoving)
                        {
                            CharsetImageAnimate();
                            ImageUseCharsetLine(MapItemImageOffset, ImageType, 2);
                            MoveStepYAxis(-1);
                        }
                        else
                        {
                            SetPlayerImageAnimationToStartingPosition();
                        }
                        break;
                    case MapItemDirectionEnum.Left:
                        if (IsMoving)
                        {
                            CharsetImageAnimate();
                            ImageUseCharsetLine(MapItemImageOffset, ImageType, 3);
                            MoveStepXAxis(-1);
                        }
                        else
                        {
                            SetPlayerImageAnimationToStartingPosition();
                        }
                        break;
                    case MapItemDirectionEnum.Right:
                        if (IsMoving)
                        {
                            CharsetImageAnimate();
                            ImageUseCharsetLine(MapItemImageOffset, ImageType, 4);
                            MoveStepXAxis(1);
                        }
                        else
                        {
                            SetPlayerImageAnimationToStartingPosition();
                        }
                        break;
                }
            }
        }

        private void MoveStepXAxis(int quantity)
        {
            var newPosition = MapItemPosition.GetCopy();
            newPosition.MoveXPosition(MapItemStati.Speed * quantity);

            var collisionType = MapUserControl.GetCollisionType(this, newPosition);

            switch (collisionType)
            {
                case MapUserControl.CollisionTypeEnum.Blocked:
                    break;
                case MapUserControl.CollisionTypeEnum.Touched:

                case MapUserControl.CollisionTypeEnum.None:
                    MapItemPosition = newPosition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveStepYAxis(int quantity)
        {
            var newPosition = MapItemPosition.GetCopy();
            newPosition.MoveYPosition(MapItemStati.Speed * quantity);

            var collisionType = MapUserControl.GetCollisionType(this, newPosition);

            switch (collisionType)
            {
                case MapUserControl.CollisionTypeEnum.Blocked:
                    break;
                case MapUserControl.CollisionTypeEnum.Touched:

                case MapUserControl.CollisionTypeEnum.None:
                    MapItemPosition = newPosition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRender(object sender, EventArgs e)
        {
            MoveMapItem();
        }

        private void SetPlayerImageAnimationToStartingPosition()
        {
            switch (ImageType)
            {
                case ItemImageTypeEnum.Charset:
                    PlayerImageFrame = 1;
                    MapItemImageOffset.X = -Width;
                    break;
                case ItemImageTypeEnum.Image:
                    PlayerImageFrame = 1;
                    MapItemImageOffset.X = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetXPositionOnMap(Position position)
        {
            Canvas.SetLeft(this, position.XMin);
            SetDepth();
        }

        private void SetYPositionOnMap(Position position)
        {
            Canvas.SetBottom(this, position.YMin);
            SetDepth();
        }
    }

    public enum MapItemDirectionEnum
    {
        Up,

        Down,

        Right,

        Left
    }
}