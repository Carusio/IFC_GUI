using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcOutputView.xaml
    /// </summary>
    public partial class IfcOutputView : IViewFor<IfcOutputViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcOutputViewModel), typeof(IfcOutputView), new PropertyMetadata(null));

        public IfcOutputViewModel ViewModel
        {
            get => (IfcOutputViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcOutputViewModel)value;
        }
        #endregion
        public IfcOutputView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeOutputView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => NodeOutputView.ViewModel = null));
            });
        }
    }
}
