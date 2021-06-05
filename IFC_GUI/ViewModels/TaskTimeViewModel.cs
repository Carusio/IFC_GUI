using IFC_GUI.Models;
using IFC_GUI.Views.NodeViews.NodeTypesView;
using ReactiveUI;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class TaskTimeViewModel : ReactiveObject
    {
        static TaskTimeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new TaskTimeView(), typeof(IViewFor<TaskTimeViewModel>));
        }

        private TaskTimeModel _taskTimeModel;
        public TaskTimeModel TaskTimeModel
        {
            get => _taskTimeModel;
            set => this.RaiseAndSetIfChanged(ref _taskTimeModel, value);
        }

        public TaskTimeViewModel()
        {

        }
        public TaskTimeViewModel(TaskTimeModel ttm)
        {
            this.TaskTimeModel = ttm;
        }
    }
}
