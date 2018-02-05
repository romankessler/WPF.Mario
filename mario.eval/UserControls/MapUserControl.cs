namespace Mario.Eval.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Mario.Eval.UserControls.Buffs;
    using Mario.Eval.UserControls.People;

    public class MapUserControl : Canvas
    {
        public MapUserControl()
        {
            RandomNumber = new Random();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            ApplyTemplate();
        }

        public enum CollisionTypeEnum
        {
            Blocked,

            Touched,

            None
        }

        public static readonly DependencyProperty MapTerrainImageProperty = DependencyProperty.Register(
            "MapTerrainImage",
            typeof(ImageSource),
            typeof(MapUserControl),
            new PropertyMetadata(default(ImageSource)));

        private List<MapItemUserControl> _itemPool;

        public ImageSource MapTerrainImage
        {
            get
            {
                return (ImageSource)GetValue(MapTerrainImageProperty);
            }
            set
            {
                SetValue(MapTerrainImageProperty, value);
            }
        }

        public List<PlayerUserControl> PlayersOnMap
        {
            get
            {
                return Children.OfType<PlayerUserControl>().ToList();
            }
        }

        public Random RandomNumber { get; set; }

        public override void EndInit()
        {
            base.EndInit();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            InitializeSize();
            WireUpEvents();
            InitSpawnMachine();
        }

        public CollisionTypeEnum GetCollisionType(MapItemUserControl mapItem, Position newPosition)
        {
            if (IsMapItemBlocked(mapItem, newPosition)
                || IsOutOfMap(newPosition))
            {
                return CollisionTypeEnum.Blocked;
            }

            if (IsMapItemTouched(mapItem, newPosition))
            {
                return CollisionTypeEnum.Touched;
            }

            return CollisionTypeEnum.None;
        }

        public CollisionTypeEnum GetCollisionType(MapItemUserControl mapItem)
        {
            return GetCollisionType(mapItem, mapItem.MapItemPosition);
        }

        private MapItemUserControl GetRandomItem()
        {
            var i = RandomNumber.Next(0, _itemPool.Count);

            var item = _itemPool.GetRange(i, 1).First();

            var type = item.GetType();

            return (MapItemUserControl)Activator.CreateInstance(type);
        }

        private void InitItemPool()
        {
            _itemPool = new List<MapItemUserControl>()
                            {
                                new MapItemDamageUserControl(),
                                new MapItemSpeedUpUserControl(),
                                new MapItemCoinUserControl(),
                                new MapItemLifeUpUserControl()
                            };
        }

        private void InitSpawnMachine()
        {
            InitItemPool();
            var spawnTimer = new DispatcherTimer();
            spawnTimer.Tick += SpawnTimerOnTick;

            spawnTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            spawnTimer.Start();
        }

        private void InitializeSize()
        {
            Height = MapTerrainImage.Height;
            Width = MapTerrainImage.Width;
        }

        private bool IsMapItemBlocked(MapItemUserControl mapItem, Position newPosition)
        {
            var uiElementCollection = Children.OfType<MapItemUserControl>();

            foreach (var element in uiElementCollection)
            {
                if (element == mapItem)
                {
                    continue;
                }
                var hasCollision = element.MapItemPosition.HasCollision(newPosition);

                if (hasCollision)
                {
                    Console.WriteLine("Position blocked");
                    return true;
                }
            }

            return false;
        }

        private bool IsMapItemTouched(MapItemUserControl mapItem, Position newPosition)
        {
            var uiElementCollection = Children.OfType<MapItemUserControl>();

            foreach (var element in uiElementCollection)
            {
                if (element == mapItem)
                {
                    continue;
                }
                var hasTouch = element.MapItemPosition.HasTouch(newPosition);

                if (hasTouch)
                {
                    Console.WriteLine("Item TouchItem");
                    return true;
                }
            }

            return false;
        }

        private bool IsOutOfMap(Position mapItemPosition)
        {
            if (mapItemPosition.XMin <= 0)
            {
                return true;
            }
            if (mapItemPosition.XMax >= Width)
            {
                return true;
            }
            if (mapItemPosition.YMin <= 0)
            {
                return true;
            }
            if (mapItemPosition.YMax >= Height)
            {
                return true;
            }

            return false;
        }

        private void MonitorPosition(object sender, EventArgs e)
        {
            try
            {
                var updatedItem = (MapItemUserControl)sender;

                foreach (var element in Children.OfType<MapItemUserControl>())
                {
                    if (element == updatedItem)
                    {
                        continue;
                    }

                    if (element.MapItemIsUsed)
                    {
                        continue;
                    }

                    var collisionType = GetCollisionType(element);

                    switch (collisionType)
                    {
                        case CollisionTypeEnum.Blocked:
                            break;
                        case CollisionTypeEnum.Touched:
                            updatedItem.TouchItem(element);
                            Console.WriteLine("Item TouchItem");

                            break;
                        case CollisionTypeEnum.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void OnMapItemUsed(object sender, EventArgs eventArgs)
        {
            var mapItemUserControl = (MapItemUserControl)sender;
            Children.Remove(mapItemUserControl);
        }

        private void SetRandomPosition(MapItemUserControl newItem)
        {
            newItem.XPosition = RandomNumber.Next(1, (int)(Width - newItem.Width));
            newItem.YPosition = RandomNumber.Next(1, (int)(Height - newItem.Height));
        }

        private void SpawnTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (Children.OfType<MapItemUserControl>().ToList().Count <= 20)
            {
                var newItem = GetRandomItem();
                newItem.MapItemUsed += OnMapItemUsed;

                do
                {
                    SetRandomPosition(newItem);
                }
                while (IsMapItemTouched(newItem, newItem.MapItemPosition));

                Children.Add(newItem);
            }
        }

        private void WireUpEvents()
        {
            WireUpPositionChangedEvents();
        }

        private void WireUpPositionChangedEvents()
        {
            var dpdBottomProperty = DependencyPropertyDescriptor.FromProperty(BottomProperty, typeof(Canvas));
            var dpdLeftProperty = DependencyPropertyDescriptor.FromProperty(LeftProperty, typeof(Canvas));

            foreach (UIElement element in Children)
            {
                dpdBottomProperty.AddValueChanged(element, MonitorPosition);
                dpdLeftProperty.AddValueChanged(element, MonitorPosition);
            }
        }
    }
}