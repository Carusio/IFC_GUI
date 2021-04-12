using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
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
    public class IfcConnectionViewModel : ConnectionViewModel
    {
        static IfcConnectionViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcConnectionView(), typeof(IViewFor<IfcConnectionViewModel>));
        }
        public IfcConnectionViewModel(NetworkViewModel parent, IfcInputViewModel input, IfcOutputViewModel output) : base(parent, input, output)
        {
            
        }
    }
}
