using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;

namespace IFC_GUI.ViewModels.NodeViewModels
{
    public class IfcOutputViewModel : NodeOutputViewModel
    {
        static IfcOutputViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcOutputView(), typeof(IViewFor<IfcOutputViewModel>));
        }

        public IfcOutputViewModel(PortType type)
        {
            this.Port = new IfcPortViewModel { PortType = type };
            this.MaxConnections = Int32.MaxValue;
        }
    }
}
