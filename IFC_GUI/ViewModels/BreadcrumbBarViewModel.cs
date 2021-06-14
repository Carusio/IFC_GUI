using DynamicData;
using IFC_GUI.Views;
using NodeNetwork;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;


namespace IFC_GUI.ViewModels
{
    /// <summary>
    /// Viewmodel for a single element of the BreadcrumbBar.
    /// </summary>
    public class BreadCrumbViewModel : ReactiveObject
    {
        #region Name
        /// <summary>
        /// Displayed name of the crumb.
        /// </summary>
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        private string _name = "";
        #endregion
    }

    /// <summary>
    /// ViewModel for the BreadcrumbBar.
    /// This UI element displays a path as a list of path elements (crumbs), allowing navigation by selection of path elements.
    /// </summary>
    public class BreadCrumbBarViewModel : ReactiveObject
    {
        static BreadCrumbBarViewModel()
        {
            NNViewRegistrar.AddRegistration(() => new BreadcrumbBarView(), typeof(IViewFor<BreadCrumbBarViewModel>));
        }

        /// <summary>
        /// The path that is currently displayed in the bar.
        /// Add or remove elements to modify the path.
        /// </summary>
        public ISourceList<BreadCrumbViewModel> ActivePath { get; } = new SourceList<BreadCrumbViewModel>();

        #region ActiveElement
        /// <summary>
        /// The deepest element of the currect path. (Last element of ActivePath)
        /// </summary>
        public BreadCrumbViewModel ActiveItem => _activeItem.Value;
        private readonly ObservableAsPropertyHelper<BreadCrumbViewModel> _activeItem;
        #endregion

        /// <summary>
        /// Navigate to the subpath represented by the selected crumb which is passed as a parameter.
        /// Only this crumb and its ancestors are kept, the rest of the path is removed.
        /// </summary>
        public ReactiveCommand<BreadCrumbViewModel, Unit> SelectCrumb { get; }

        public BreadCrumbBarViewModel()
        {
            SelectCrumb = ReactiveCommand.Create((BreadCrumbViewModel crumb) =>
            {
                ActivePath.Edit(l =>
                {
                    int index = l.IndexOf(crumb);
                    for (int i = l.Count - 1; i > index; i--)
                    {
                        l.RemoveAt(i);
                    }
                });
            });

            ActivePath.Connect().Select(_ => ActivePath.Count > 0 ? ActivePath.Items.ElementAt(ActivePath.Count - 1) : null)
                .ToProperty(this, vm => vm.ActiveItem, out _activeItem);
        }
    }
}
