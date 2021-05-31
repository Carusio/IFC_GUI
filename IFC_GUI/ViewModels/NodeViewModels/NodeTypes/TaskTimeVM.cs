using IFC_GUI.Models;
using IFC_GUI.Views.NodeViews.NodeTypesView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class TaskTimeVM : ReactiveObject
    {
        static TaskTimeVM()
        {
            Splat.Locator.CurrentMutable.Register(() => new TaskTimeV(), typeof(IViewFor<TaskTimeVM>));
        }

        private TaskTimeModel _taskTimeModel;
        public TaskTimeModel TaskTimeModel
        {
            get => _taskTimeModel;
            set => this.RaiseAndSetIfChanged(ref _taskTimeModel, value);
        }

        public TaskTimeVM()
        {

        }
        public TaskTimeVM(TaskTimeModel ttm)
        {
            this.TaskTimeModel = ttm;
        }
    }
}
