using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.Tree
{
    public class SelfEnumerableTreePageVm : BasePageVm
    {
        public SampleNode SampleData { get; }

        public SelfEnumerableTreePageVm()
        {
            var rootNode = new SampleNode("Root");
            var child0 = new SampleNode("1-0");
            child0.Add(new SampleNode("1-0-0"));
            child0.Add(new SampleNode("1-0-1"));
            child0.Add(new SampleNode("1-0-2"));


            var child1 = new SampleNode("1-1");
            child1.Add(new SampleNode("1-1-0"));
            child1.Add(new SampleNode("1-1-1"));
            child1.Add(new SampleNode("1-1-2"));

            var child2 = new SampleNode("1-2");
            child2.Add(new SampleNode("1-2-0"));
            child2.Add(new SampleNode("1-2-1"));
            child2.Add(new SampleNode("1-2-2"));

            rootNode.Add(child0);
            rootNode.Add(child1);
            rootNode.Add(child2);

            SampleData = rootNode;
        }
    }

    public class SampleNode : List<SampleNode>
    {
        public SampleNode(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
