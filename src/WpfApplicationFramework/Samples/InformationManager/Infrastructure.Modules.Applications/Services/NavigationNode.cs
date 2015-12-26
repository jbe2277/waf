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
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name must not be null or empty."); }
            if (showAction == null) { throw new ArgumentNullException("showAction"); }
            if (closeAction == null) { throw new ArgumentNullException("closeAction"); }
            if (group < 0) { throw new ArgumentException("group must be equal or greater than 0."); }
            if (order < 0) { throw new ArgumentException("order must be equal or greater than 0."); }
            
            this.Name = name;
            this.showAction = showAction;
            this.closeAction = closeAction;
            this.Group = group;
            this.Order = order;
        }
        
      
        public string Name { get; private set; }

        public double Group { get; private set; }

        public double Order { get; private set; }

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
