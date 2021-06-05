using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcConnectionView.xaml
    /// </summary>
    public partial class IfcConnectionView : IViewFor<IfcConnectionViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcConnectionViewModel), typeof(IfcConnectionView), new PropertyMetadata(null));

        public IfcConnectionViewModel ViewModel
        {
            get => (IfcConnectionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcConnectionViewModel)value;
        }
        #endregion
        public IfcConnectionView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                ConnectionView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => ConnectionView.ViewModel = null));
            });
        }
    }
}
