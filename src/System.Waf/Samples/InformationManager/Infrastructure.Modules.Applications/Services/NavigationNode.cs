using System;
using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services
{
    public class NavigationNode : Model, INavigationNode
    {
        private readonly Action showAction;
        private readonly Action closeAction;
        private int? itemCount;
        private bool isSelected;
        private bool isFirstItemOfNewGroup;

        public NavigationNode(string name, Action showAction, Action closeAction, double group, double order)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name must not be null or empty.", nameof(name)); }
            if (showAction == null) { throw new ArgumentNullException(nameof(showAction)); }
            if (closeAction == null) { throw new ArgumentNullException(nameof(closeAction)); }
            if (group < 0) { throw new ArgumentException("group must be equal or greater than 0.", nameof(group)); }
            if (order < 0) { throw new ArgumentException("order must be equal or greater than 0.", nameof(order)); }
            Name = name;
            this.showAction = showAction;
            this.closeAction = closeAction;
            Group = group;
            Order = order;
        }
        
        public string Name { get; }

        public double Group { get; }

        public double Order { get; }

        public int? ItemCount
        {
            get { return itemCount; }
            set { SetProperty(ref itemCount, value); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    if (isSelected)
                    {
                        closeAction();
                    }
                    isSelected = value;
                    RaisePropertyChanged();
                    if (isSelected)
                    {
                        showAction();
                    }
                }
            }
        }

        public bool IsFirstItemOfNewGroup
        {
            get { return isFirstItemOfNewGroup; }
            set { SetProperty(ref isFirstItemOfNewGroup, value); }
        }
    }
}
