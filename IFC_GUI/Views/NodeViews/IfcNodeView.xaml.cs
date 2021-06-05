using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcNodeView.xaml
    /// </summary>
    public partial class IfcNodeView : IViewFor<IfcNodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcNodeViewModel), typeof(IfcNodeView), new PropertyMetadata(null));

        public IfcNodeViewModel ViewModel
        {
            get => (IfcNodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcNodeViewModel)value;
        }
        #endregion
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public IfcNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);

                NodeView.ViewModel = this.ViewModel;
                Disposable.Create(() => NodeView.ViewModel = null).DisposeWith(d);
            });
        }
    }
}
