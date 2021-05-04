using IFC_GUI.Models;
using IFC_GUI.Views.NodeViews.NodeTypesView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class IfcTaskTimeViewModel : ReactiveObject
    {
        static IfcTaskTimeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcTaskTimeView(), typeof(IViewFor<IfcTaskTimeViewModel>));
        }

        private TaskTimeModel _taskTimeModel;
        public TaskTimeModel TaskTimeModel
        {
            get => _taskTimeModel;
            set => this.RaiseAndSetIfChanged(ref _taskTimeModel, value);
        }

        public IfcTaskTimeViewModel()
        {

        }
    }
}
