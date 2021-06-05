using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;

namespace IFC_GUI.ViewModels.NodeViewModels
{
    //all available porttypes in this application
    public enum PortType
    {
        None
    }
    public class IfcPortViewModel : PortViewModel
    {
        static IfcPortViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcPortView(), typeof(IViewFor<IfcPortViewModel>));
        }

        #region PortType
        public PortType PortType
        {
            get => _portType;
            set => this.RaiseAndSetIfChanged(ref _portType, value);
        }
        private PortType _portType;
        #endregion
    }
}
