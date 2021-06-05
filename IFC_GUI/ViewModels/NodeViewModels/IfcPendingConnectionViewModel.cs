using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;

namespace IFC_GUI.ViewModels.NodeViewModels
{
    public class IfcPendingConnectionViewModel : PendingConnectionViewModel
    {
        static IfcPendingConnectionViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcPendingConnectionView(), typeof(IViewFor<IfcPendingConnectionViewModel>));
        }
        public IfcPendingConnectionViewModel(NetworkViewModel parent) : base(parent)
        {

        }
    }
}
