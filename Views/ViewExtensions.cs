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
    public static class ViewExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var dependencyObject = child.FindParentObject();
            if (dependencyObject == null) return default;
            T result;
            if ((result = dependencyObject as T) == null) result = dependencyObject.FindParent<T>();
            return result;
        }

        public static DependencyObject FindParentObject(this DependencyObject child)
        {
            if (child == null) return null;
            ContextMenu contextMenu;
            if ((contextMenu = child as ContextMenu) != null)
            {
                var placementTarget = contextMenu.PlacementTarget;
                if (placementTarget != null) return placementTarget;
            }

            Popup obj;
            if ((obj = child as Popup) != null)
            {
                var attachedTarget = (DependencyObject)obj.GetValue(null);
                if (attachedTarget != null) return attachedTarget;
            }

            ContentElement contentElement;
            if ((contentElement = child as ContentElement) == null)
            {
                FrameworkElement frameworkElement;
                if ((frameworkElement = child as FrameworkElement) != null)
                {
                    var parent = frameworkElement.Parent;
                    if (parent != null) return parent;
                }

                return VisualTreeHelper.GetParent(child);
            }

            var parent1 = ContentOperations.GetParent(contentElement);
            if (parent1 != null) return parent1;
            var frameworkContentElement = contentElement as FrameworkContentElement;
            if (frameworkContentElement == null) return null;
            return frameworkContentElement.Parent;
        }
    }
}
