using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;

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
