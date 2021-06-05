using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcPortView.xaml
    /// </summary>
    public partial class IfcPortView : IViewFor<IfcPortViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcPortViewModel), typeof(IfcPortView), new PropertyMetadata(null));

        public IfcPortViewModel ViewModel
        {
            get => (IfcPortViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcPortViewModel)value;
        }
        #endregion

        public IfcPortView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.PortView.ViewModel).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.IsMirrored, v => v.PortView.RenderTransform,
                    isMirrored => new ScaleTransform(isMirrored ? -1.0 : 1.0, 1.0))
                    .DisposeWith(d);
            });
        }
    }
}
