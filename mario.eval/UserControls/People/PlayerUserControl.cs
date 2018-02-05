namespace Mario.Eval.UserControls.People
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Mario.Eval.CustomEventArgs;

    public class PlayerUserControl : MapItemUserControl
    {
        public PlayerUserControl()
        {
            ImageType = ItemImageTypeEnum.Charset;
            IsBlocked = true;
        }

        public static readonly DependencyProperty KeyMoveDownProperty = DependencyProperty.Register(
            "KeyMoveDown",
            typeof(Key),
            typeof(PlayerUserControl),
            new PropertyMetadata(default(Key)));

        public static readonly DependencyProperty KeyMoveLeftProperty = DependencyProperty.Register(
            "KeyMoveLeft",
            typeof(Key),
            typeof(PlayerUserControl),
            new PropertyMetadata(default(Key)));

        public static readonly DependencyProperty KeyMoveRightProperty = DependencyProperty.Register(
            "KeyMoveRight",
            typeof(Key),
            typeof(PlayerUserControl),
            new PropertyMetadata(default(Key)));

        public static readonly DependencyProperty KeyMoveUpProperty = DependencyProperty.Register(
            "KeyMoveUp",
            typeof(Key),
            typeof(PlayerUserControl),
            new PropertyMetadata(default(Key)));

        private bool _isKeyDownDown;

        private bool _isKeyLeftDown;

        private bool _isKeyRightDown;

        private bool _isKeyUpDown;

        public Key KeyMoveDown
        {
            get
            {
                return (Key)GetValue(KeyMoveDownProperty);
            }
            set
            {
                SetValue(KeyMoveDownProperty, value);
            }
        }

        public Key KeyMoveLeft
        {
            get
            {
                return (Key)GetValue(KeyMoveLeftProperty);
            }
            set
            {
                SetValue(KeyMoveLeftProperty, value);
            }
        }

        public Key KeyMoveRight
        {
            get
            {
                return (Key)GetValue(KeyMoveRightProperty);
            }
            set
            {
                SetValue(KeyMoveRightProperty, value);
            }
        }

        public Key KeyMoveUp
        {
            get
            {
                return (Key)GetValue(KeyMoveUpProperty);
            }
            set
            {
                SetValue(KeyMoveUpProperty, value);
            }
        }


        private void PlayerHurt()
        {
            var parent = (MapUserControl)Parent;
            var newItem = new AnimationControl("ImgBloodFontain", 5, 7, MapItemPosition);
            parent.Children.Add(newItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WireUpEvents();
            MapItemDirection = MapItemDirectionEnum.Right;
            InitializeLife();
        }



        private void InitializeLife()
        {
            MapItemStati.Life = 5;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Focusable = true;
            Focus();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            

            if (keyEventArgs.Key == KeyMoveLeft)
            {
                IsMoving = true;
                _isKeyLeftDown = true;
                MapItemDirection = MapItemDirectionEnum.Left;
            }

            if (keyEventArgs.Key == KeyMoveRight)
            {
                IsMoving = true;
                _isKeyRightDown = true;
                MapItemDirection = MapItemDirectionEnum.Right;
            }

            if (keyEventArgs.Key == KeyMoveUp)
            {
                IsMoving = true;
                _isKeyUpDown = true;
                MapItemDirection = MapItemDirectionEnum.Up;
            }

            if (keyEventArgs.Key == KeyMoveDown)
            {
                IsMoving = true;
                _isKeyDownDown = true;
                MapItemDirection = MapItemDirectionEnum.Down;
            }
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == KeyMoveLeft)
            {
                _isKeyLeftDown = false;
            }

            if (keyEventArgs.Key == KeyMoveRight)
            {
                _isKeyRightDown = false;
            }

            if (keyEventArgs.Key == KeyMoveUp)
            {
                _isKeyUpDown = false;
            }

            if (keyEventArgs.Key == KeyMoveDown)
            {
                _isKeyDownDown = false;
            }

            if (!_isKeyUpDown
                && !_isKeyDownDown
                && !_isKeyLeftDown
                && !_isKeyRightDown)
            {
                IsMoving = false;
            }
        }

        private void PlayerDie()
        {
            MapItemImage = new ImageBrush();
            IsMoving = false;

            var parent = (Panel)Parent;

            var newItem = new AnimationControl("ImgExplosion", 4, 4, MapItemPosition);

            parent.Children.Add(newItem);
            parent.Children.Remove(this);
        }

        private new void WireUpEvents()
        {
            Loaded += OnLoaded;
            PreviewKeyDown += OnPreviewKeyDown;
            PreviewKeyUp += OnPreviewKeyUp;
            MapItemStati.LifeChanged += MapItemStatiOnLifeChanged;
        }

        private void MapItemStatiOnLifeChanged(object sender, OldNewValueEventArgs args)
        {
            if (args.OldValue>args.NewValue)
            {
                PlayerHurt();
            }


            if (args.NewValue <= 0)
            {
                PlayerDie();
                PreviewKeyDown -= OnPreviewKeyDown;
                MapItemImage = null;
            }
        }
    }
}