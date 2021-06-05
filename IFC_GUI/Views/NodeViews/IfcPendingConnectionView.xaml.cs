using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcPendingConnectionView.xaml
    /// </summary>
    public partial class IfcPendingConnectionView : IViewFor<IfcPendingConnectionViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcPendingConnectionViewModel), typeof(IfcPendingConnectionView), new PropertyMetadata(null));

        public IfcPendingConnectionViewModel ViewModel
        {
            get => (IfcPendingConnectionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcPendingConnectionViewModel)value;
        }
        #endregion
        public IfcPendingConnectionView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                PendingConnectionView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => PendingConnectionView.ViewModel = null));
            });
        }
    }
}
