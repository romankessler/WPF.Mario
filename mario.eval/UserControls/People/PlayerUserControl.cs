namespace Mario.Eval.UserControls.People
{
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

        private bool _isKeyDownDown;

        private bool _isKeyLeftDown;

        private bool _isKeyRightDown;

        private bool _isKeyUpDown;

        public Key KeyMoveDown { get; set; }

        public Key KeyMoveLeft { get; set; }

        public Key KeyMoveRight { get; set; }

        public Key KeyMoveUp { get; set; }

        private void PlayerHurt()
        {
            var parent = (MapUserControl)Parent;
            var newItem = new AnimationControl("ImgBloodFontain", 5, 7, MapItemPosition); // TODO (ROK): Einbauen, dass animation dem Spieler folgt
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
            // TODO (ROK): Einbauen, dass die Leben über einen Service gehandelt, initialisiert werden.
            MapItemStati.Life = 5;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            /* TODO (ROK): Steuerung des Spielers muss in ein separater Service ausgelagert werden. 
             * Dieser Service soll als control erstellt werden und fokusiert werden.
             Dadurch können alle Tasten abgerufen werden und auch Spieler 2 kann sich bewegen. 

             Problem zur Zeit: Spieler 2 kann sich nicht bewegen, da der Fokus auf Spieler 1 liegt und somit nur 
             die Tasteneingabe von Spieler 1 berücksichtigt werden.*/
            Focusable = true;
            Focus();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            /* TODO (ROK): Dieser Teil soll über ein Service gesetzt werden können 
             * IsMoving = true
             * MapItemDirection = MapItemDirectionEnum. Left Right Up Down
             */

            if (keyEventArgs.Key == KeyMoveLeft)
            {
                IsMoving = true;
                _isKeyLeftDown = true; // TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
                MapItemDirection = MapItemDirectionEnum.Left;
            }

            if (keyEventArgs.Key == KeyMoveRight)
            {
                IsMoving = true;
                _isKeyRightDown = true;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
                MapItemDirection = MapItemDirectionEnum.Right;
            }

            if (keyEventArgs.Key == KeyMoveUp)
            {
                IsMoving = true;
                _isKeyUpDown = true;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
                MapItemDirection = MapItemDirectionEnum.Up;
            }

            if (keyEventArgs.Key == KeyMoveDown)
            {
                IsMoving = true;
                _isKeyDownDown = true;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
                MapItemDirection = MapItemDirectionEnum.Down;
            }
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            /* TODO (ROK): Dieser Teil soll über ein Service gesetzt werden können 
             * IsMoving = false*/

            if (keyEventArgs.Key == KeyMoveLeft)
            {
                _isKeyLeftDown = false;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
            }

            if (keyEventArgs.Key == KeyMoveRight)
            {
                _isKeyRightDown = false;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
            }

            if (keyEventArgs.Key == KeyMoveUp)
            {
                _isKeyUpDown = false;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
            }

            if (keyEventArgs.Key == KeyMoveDown)
            {
                _isKeyDownDown = false;// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
            }

            if (!_isKeyUpDown// TODO (ROK): kann über ein dictionary im Steuerungsservice gelöst werden
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