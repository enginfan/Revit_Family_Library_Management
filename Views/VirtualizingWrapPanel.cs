using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TripleKill.Views
{
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        public static readonly DependencyProperty DynamicThresholdProperty = DependencyProperty.Register("DynamicThreshold", typeof(int), typeof(VirtualizingWrapPanel), new PropertyMetadata(3,
            delegate (DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                VirtualizingWrapPanel virtualizingWrapPanel;
                object newValue;
                if ((virtualizingWrapPanel = o as VirtualizingWrapPanel) != null && (newValue = e.NewValue) is int)
                {
                    var num = (int)newValue;
                    if (num < 3) virtualizingWrapPanel.DynamicThreshold = 3;
                }
            }));

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel),
            new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel),
            new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.RegisterAttached("ScrollOffset", typeof(int), typeof(VirtualizingWrapPanel), new PropertyMetadata(10));

        private readonly TranslateTransform _transform = new TranslateTransform();

        private Size _extent = new Size(0.0, 0.0);

        private Point _offset;

        private ScrollViewer _scrollOwner;

        private Size _viewPort = new Size(0.0, 0.0);

        public VirtualizingWrapPanel()
        {
            RenderTransform = _transform;
        }

        public int DynamicThreshold
        {
            get => (int)GetValue(DynamicThresholdProperty);
            set => SetValue(DynamicThresholdProperty, value);
        }

        public double ItemHeight
        {
            get => Convert.ToDouble(GetValue(ItemHeightProperty));
            set => SetValue(ItemHeightProperty, value);
        }

        public double ItemWidth
        {
            get => Convert.ToDouble(GetValue(ItemWidthProperty));
            set => SetValue(ItemWidthProperty, value);
        }

        public int ScrollOffset
        {
            get => Convert.ToInt32(GetValue(ScrollOffsetProperty));
            set => SetValue(ScrollOffsetProperty, value);
        }

        public bool CanVerticallyScroll { get; set; }

        public bool CanHorizontallyScroll { get; set; }

        public double ExtentWidth => _extent.Width;

        public double ExtentHeight => _extent.Height;

        public double ViewportWidth => _viewPort.Width;

        public double ViewportHeight => _viewPort.Height;

        public double HorizontalOffset => _offset.X;

        public double VerticalOffset => _offset.Y;

        public ScrollViewer ScrollOwner
        {
            get
            {
                ScrollViewer result;
                if ((result = _scrollOwner) == null) result = _scrollOwner = this.FindParent<ScrollViewer>();
                return result;
            }
            set => _scrollOwner = value;
        }

        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + ScrollOffset);
        }

        public void LineLeft()
        {
            throw new NotImplementedException();
        }

        public void LineRight()
        {
            throw new NotImplementedException();
        }

        public void LineUp()
        {
            SetVerticalOffset(VerticalOffset - ScrollOffset);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return default;
        }

        public void MouseWheelDown()
        {
            SetVerticalOffset(VerticalOffset + ScrollOffset);
        }

        public void MouseWheelLeft()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelRight()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelUp()
        {
            SetVerticalOffset(VerticalOffset - ScrollOffset);
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + _viewPort.Height);
        }

        public void PageLeft()
        {
            throw new NotImplementedException();
        }

        public void PageRight()
        {
            throw new NotImplementedException();
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - _viewPort.Height);
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0.0 || _viewPort.Height >= _extent.Height)
                offset = 0.0;
            else if (offset + _viewPort.Height >= _extent.Height) offset = _extent.Height - _viewPort.Height;
            _offset.Y = offset;
            var scrollOwner = ScrollOwner;
            if (scrollOwner != null) scrollOwner.InvalidateScrollInfo();
            _transform.Y = -offset;
            InvalidateMeasure();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var itemContainerGenerator = ItemContainerGenerator;
            UpdateScrollInfo(finalSize);
            var num = CalculateChildrenPerRow(finalSize);
            var availableItemWidth = finalSize.Width / num;
            var margin = num == 1 ? 0.0 : (finalSize.Width - ItemWidth * num) / (num - 1);
            for (var i = 0; i <= Children.Count - 1; i++)
            {
                var uielement = Children[i];
                var num2 = itemContainerGenerator.IndexFromGeneratorPosition(new GeneratorPosition(i, 0));
                var num3 = num2 / num;
                var column = num2 % num;
                var x = CalculateXCoordinate(column, num, availableItemWidth, margin);
                var finalRect = new Rect(x, num3 * ItemHeight, ItemWidth, ItemHeight);
                uielement.Arrange(finalRect);
            }

            return finalSize;
        }

        private double CalculateXCoordinate(int column, int rowCapacity, double availableItemWidth, double margin)
        {
            if (rowCapacity <= DynamicThreshold)
            {
                if (rowCapacity == 1) return column * availableItemWidth + (availableItemWidth - ItemWidth) / 2.0;
                return column * ItemWidth + column * margin;
            }

            if (rowCapacity >= Children.Count) return column * ItemWidth;
            return column * (ItemWidth + margin);
        }

        protected override void BringIndexIntoView(int index)
        {
            if (index < 0 || index >= Children.Count) throw new ArgumentOutOfRangeException();
            var num = index / CalculateChildrenPerRow(RenderSize);
            SetVerticalOffset(num * ItemHeight);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateScrollInfo(availableSize);
            var num = 0;
            var num2 = 0;
            GetVisibleRange(ref num, ref num2);
            var internalChildren = InternalChildren;
            var itemContainerGenerator = ItemContainerGenerator;
            var position = itemContainerGenerator.GeneratorPositionFromIndex(num);
            var num3 = position.Offset == 0 ? position.Index : position.Index + 1;
            using (itemContainerGenerator.StartAt(position, GeneratorDirection.Forward, true))
            {
                var i = num;
                while (i <= num2)
                {
                    bool flag;
                    UIElement uielement;
                    if ((uielement = itemContainerGenerator.GenerateNext(out flag) as UIElement) != null)
                    {
                        if (flag)
                        {
                            if (num3 >= internalChildren.Count)
                                AddInternalChild(uielement);
                            else
                                InsertInternalChild(num3, uielement);
                            itemContainerGenerator.PrepareItemContainer(uielement);
                        }
                        else if (!uielement.Equals(internalChildren[num3]))
                        {
                            RemoveInternalChildRange(num3, 1);
                        }

                        uielement.Measure(new Size(ItemWidth, ItemHeight));
                        i++;
                        num3++;
                    }
                }
            }

            CleanUpItems(num, num2);
            return new Size(double.IsInfinity(availableSize.Width) ? 0.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 0.0 : availableSize.Height);
        }

        protected override void OnClearChildren()
        {
            base.OnClearChildren();
            SetVerticalOffset(0.0);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetVerticalOffset(VerticalOffset);
        }

        private int CalculateChildrenPerRow(Size availableSize)
        {
            if (!double.IsPositiveInfinity(availableSize.Width)) return Math.Max(1, Convert.ToInt32(Math.Floor(availableSize.Width / ItemWidth)));
            return Children.Count;
        }

        private Size CalculateExtent(Size availableSize, int itemsCount)
        {
            var num = CalculateChildrenPerRow(availableSize);
            return new Size(num * ItemWidth, ItemHeight * Math.Ceiling(Convert.ToDouble(itemsCount) / num));
        }

        private void CleanUpItems(int startIndex, int endIndex)
        {
            var internalChildren = InternalChildren;
            var itemContainerGenerator = ItemContainerGenerator;
            for (var i = internalChildren.Count - 1; i >= 0; i--)
            {
                var position = new GeneratorPosition(i, 0);
                var num = itemContainerGenerator.IndexFromGeneratorPosition(position);
                if (num >= 0 && (num < startIndex || num > endIndex))
                {
                    itemContainerGenerator.Remove(position, 1);
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private int GetItemCount(DependencyObject element)
        {
            var itemsOwner = ItemsControl.GetItemsOwner(element);
            if (!itemsOwner.HasItems) return 0;
            return itemsOwner.Items.Count;
        }

        private void GetVisibleRange(ref int firstIndex, ref int lastIndex)
        {
            var num = CalculateChildrenPerRow(_extent);
            firstIndex = Convert.ToInt32(Math.Floor(_offset.Y / ItemHeight)) * num;
            lastIndex = Convert.ToInt32(Math.Ceiling((_offset.Y + _viewPort.Height) / ItemHeight)) * num - 1;
            var itemCount = GetItemCount(this);
            if (lastIndex >= itemCount) lastIndex = itemCount - 1;
        }

        private void UpdateScrollInfo(Size availableSize)
        {
            var size = CalculateExtent(availableSize, GetItemCount(this));
            if (size != _extent)
            {
                _extent = size;
                ScrollOwner.InvalidateScrollInfo();
            }

            if (availableSize != _viewPort)
            {
                _viewPort = availableSize;
                ScrollOwner.InvalidateScrollInfo();
            }
        }
    }
}
