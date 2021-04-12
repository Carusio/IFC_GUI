using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class TestNodeViewModel : IfcNodeViewModel
    {
        static TestNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcNodeView(), typeof(IViewFor<TestNodeViewModel>));
        }

        public TestNodeViewModel() : base(NodeType.TestNode)
        {
            this.Name = "Test_Node";
            this.Resizable = ResizeOrientation.HorizontalAndVertical;

        }
    }
}
