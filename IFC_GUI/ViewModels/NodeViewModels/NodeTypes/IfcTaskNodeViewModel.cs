using DynamicData;
using IFC_GUI.Models;
using IFC_GUI.Views.NodeViews.NodeTypesView;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class IfcTaskNodeViewModel : IfcNodeViewModel
    {
        static IfcTaskNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcTaskNodeView(), typeof(IViewFor<IfcTaskNodeViewModel>));
        }

        public IfcInputViewModel Input { get; }
        public IfcOutputViewModel Output { get; }

        private TaskModel _taskModel;
        public TaskModel TaskModel 
        { get => _taskModel;
          set => this.RaiseAndSetIfChanged(ref _taskModel, value); 
        }

        public IfcTaskNodeViewModel() : base(NodeType.TaskNode)
        {
            this.Name = "TaskNode";
            this.Resizable = ResizeOrientation.HorizontalAndVertical;
            this.TaskModel = new TaskModel();

            Input = new IfcInputViewModel(PortType.None)
            {
                Name = "Is successor from"
            };
            this.Inputs.Add(Input);

            Output = new IfcOutputViewModel(PortType.None)
            {
                Name = "Is predessecor to"
            };
            this.Outputs.Add(Output);
        }

        public IfcTaskNodeViewModel(TaskModel tm) : base(NodeType.TaskNode)
        {
            this.Name = "TaskNode";
            this.Resizable = ResizeOrientation.HorizontalAndVertical;
            this.TaskModel = tm;

            Input = new IfcInputViewModel(PortType.None)
            {
                Name = "Is successor from"
            };
            this.Inputs.Add(Input);


            Output = new IfcOutputViewModel(PortType.None)
            {
                Name = "Is predessecor to"
            };
            this.Outputs.Add(Output);
        }
    }
}
