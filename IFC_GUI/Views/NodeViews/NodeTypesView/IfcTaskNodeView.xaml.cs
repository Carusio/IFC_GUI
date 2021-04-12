using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IFC_GUI.Views.NodeViews.NodeTypesView
{
    /// <summary>
    /// Interaction logic for IfcTaskNodeView.xaml
    /// </summary>
    public partial class IfcTaskNodeView : IViewFor<IfcTaskNodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcTaskNodeViewModel), typeof(IfcTaskNodeView), new PropertyMetadata(null));

        public IfcTaskNodeViewModel ViewModel
        {
            get => (IfcTaskNodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcTaskNodeViewModel)value;
        }
        #endregion
        public IfcTaskNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.IfcTaskView.ViewModel).DisposeWith(d);

                IfcTaskView.ViewModel = this.ViewModel;
                Disposable.Create(() => IfcTaskView.ViewModel = null).DisposeWith(d);
            });
        }
    }
}
