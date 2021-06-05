using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcInputView.xaml
    /// </summary>
    public partial class IfcInputView : IViewFor<IfcInputViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcInputViewModel), typeof(IfcInputView), new PropertyMetadata(null));

        public IfcInputViewModel ViewModel
        {
            get => (IfcInputViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcInputViewModel)value;
        }
        #endregion
        public IfcInputView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeInputView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => NodeInputView.ViewModel = null));
            });
        }
    }
}
