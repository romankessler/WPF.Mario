namespace Mario.Eval.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal class AnimationControl : ControlBase
    {
        public AnimationControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
        }

        public AnimationControl(
            string imageRessourceName,
            int maxXTiles,
            int maxYTiles,
            Position mapItemPosition,
            AnimationType type = AnimationType.Once,
            int framesPerSecond = 40)
        {
            MaxXTiles = maxXTiles;
            MaxYTiles = maxYTiles;

            ImageSource = Application.Current.Resources[imageRessourceName] as ImageSource;
            FramesPerSecond = framesPerSecond;

            ItemAnimationType = type;

            Canvas.SetLeft(this, mapItemPosition.XMin - (mapItemPosition.XMax - mapItemPosition.XMin));
            Canvas.SetBottom(this, mapItemPosition.YMin);
        }

        public enum AnimationType
        {
            Once,

            Infinite
        }

        public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.Register(
            "AnimationType",
            typeof(AnimationType),
            typeof(AnimationControl),
            new PropertyMetadata(default(AnimationType)));

        public static readonly DependencyProperty FramesPerSecondProperty = DependencyProperty.Register(
            "FramesPerSecond",
            typeof(int),
            typeof(AnimationControl),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource",
            typeof(ImageSource),
            typeof(AnimationControl),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty MaxXTilesProperty = DependencyProperty.Register(
            "MaxXTiles",
            typeof(int),
            typeof(AnimationControl),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MaxYTilesProperty = DependencyProperty.Register(
            "MaxYTiles",
            typeof(int),
            typeof(AnimationControl),
            new PropertyMetadata(default(int)));

        private ImageBrush _imageBrush;

        private TranslateTransform _offset;

        public int CurrentColumn { get; set; }

        public int CurrentRow { get; set; }

        public DateTime FrameLastSkip { get; set; }

        public int FramesPerSecond
        {
            get
            {
                return (int)GetValue(FramesPerSecondProperty);
            }
            set
            {
                SetValue(FramesPerSecondProperty, value);
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        public AnimationType ItemAnimationType
        {
            get
            {
                return (AnimationType)GetValue(AnimationTypeProperty);
            }
            set
            {
                SetValue(AnimationTypeProperty, value);
            }
        }

        public int MaxXTiles
        {
            get
            {
                return (int)GetValue(MaxXTilesProperty);
            }
            set
            {
                SetValue(MaxXTilesProperty, value);
            }
        }

        public int MaxYTiles
        {
            get
            {
                return (int)GetValue(MaxYTilesProperty);
            }
            set
            {
                SetValue(MaxYTilesProperty, value);
            }
        }

        protected override void EvaluateControls()
        {
            _offset = (TranslateTransform)Template.FindName("PART_SpriteSheetOffset", this);
            _imageBrush = (ImageBrush)Template.FindName("PART_ImageBrush", this);
        }

        protected override void OnTemplateApplied()
        {
            Initialize();
        }

        protected override void WireUpControlEvents()
        {
        }

        private void Initialize()
        {
            Focusable = false;

            Height = (ImageSource.Height / MaxYTiles);
            Width = (ImageSource.Width / MaxXTiles);

            _imageBrush.ImageSource = ImageSource;
            _imageBrush.Viewport = new Rect(0, 0, _imageBrush.ImageSource.Width, _imageBrush.ImageSource.Height);

            CompositionTarget.Rendering += OnRender;

            CurrentRow = 0;
            CurrentColumn = 0;
        }

        private void OnFrame()
        {
            if (CurrentColumn >= MaxXTiles)
            {
                CurrentColumn = 0;

                _offset.Y = -CurrentRow * (ImageSource.Height / MaxYTiles);
                Console.WriteLine("2: X: {0} Y: {1}", _offset.X, _offset.Y);
                CurrentRow++;

                if (CurrentRow > MaxYTiles)
                {
                    switch (ItemAnimationType)
                    {
                        case AnimationType.Once:
                            CompositionTarget.Rendering -= OnRender;
                            break;
                        case AnimationType.Infinite:
                            CurrentRow = 0;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            Console.WriteLine("Row:{0} Column:{1}", CurrentRow, CurrentColumn);

            _offset.X = -CurrentColumn * (ImageSource.Width / MaxXTiles);
            Console.WriteLine("1: X: {0} Y: {1}", _offset.X, _offset.Y);
            CurrentColumn++;
        }

        private void OnRender(object sender, EventArgs e)
        {
            if (FrameLastSkip + TimeSpan.FromSeconds(1.0 / FramesPerSecond) <= DateTime.Now)
            {
                FrameLastSkip = DateTime.Now;
                OnFrame();
            }
        }
    }
}