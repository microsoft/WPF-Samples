using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CustomComboBox
{
    public class ExpandableToggleButton : Button
    {
        private ExpandableToggleButtonAutomationPeer peer;

        private ExpandCollapseState state = ExpandCollapseState.Collapsed;

        public static readonly RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExpandableToggleButton));

        public static readonly RoutedEvent CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExpandableToggleButton));

        public event RoutedEventHandler Expanded
        {
            add { AddHandler(ExpandedEvent, value); }
            remove { RemoveHandler(ExpandedEvent, value); }
        }

        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(CollapsedEvent, value); }
            remove { RemoveHandler(CollapsedEvent, value); }
        }

        public ExpandCollapseState State
        {
            get
            {
                return this.state;
            }
            set
            {
                ExpandCollapseState previousState = this.state;

                this.state = value;

                if ((this.peer != null) && this.state != previousState)
                {
                    this.peer.RaisePropertyChangedEvent(
                       ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                       previousState,
                       this.state);
                    if(this.state == ExpandCollapseState.Collapsed)
                    {
                        RoutedEventArgs collapsedEventArgs = new RoutedEventArgs(CollapsedEvent);
                        RaiseEvent(collapsedEventArgs);
                    } else
                    {
                        RoutedEventArgs expandedEventArgs = new RoutedEventArgs(ExpandedEvent);
                        RaiseEvent(expandedEventArgs);
                    }
                }
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            if(this.peer == null)
            {
                this.peer = new ExpandableToggleButtonAutomationPeer(this);
            }

            return this.peer;
        }

        protected override void OnClick()
        {
            this.State = (this.State == ExpandCollapseState.Collapsed ? ExpandCollapseState.Expanded :
                ExpandCollapseState.Collapsed);

            // base.OnClick();
        }
    }
    
    public class ExpandableToggleButtonAutomationPeer : ButtonAutomationPeer, IExpandCollapseProvider
    {
        private ExpandableToggleButton Button { get { return Owner as ExpandableToggleButton; } }

        public ExpandableToggleButtonAutomationPeer(ExpandableToggleButton owner) : base(owner) {}

        public override object GetPattern(PatternInterface patternInterface)
        {
            if(patternInterface == PatternInterface.ExpandCollapse)
            {
                return this;
            }
            return base.GetPattern(patternInterface);
        }

        public ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return Button.State;
            }
        }

        public void Expand()
        {
            Button.State = ExpandCollapseState.Expanded;
        }

        public void Collapse()
        {
            Button.State = ExpandCollapseState.Collapsed;
        }
    }
}
