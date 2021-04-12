using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels.NodeViewModels
{
    public class IfcInputViewModel : NodeInputViewModel
    {
        static IfcInputViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcInputView(), typeof(IViewFor<IfcInputViewModel>));
        }

        public IfcInputViewModel(PortType type)
        {
            this.Port = new IfcPortViewModel { PortType = type };
            this.MaxConnections = Int32.MaxValue;
        }
    }
}
