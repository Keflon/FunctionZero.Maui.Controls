using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class TreeDataTemplateSelector : TemplateProvider, IList<TreeItemDataTemplate>
    {
        private List<TreeItemDataTemplate> _templateList = new List<TreeItemDataTemplate>();

        public TreeItemDataTemplate OnSelectTemplate(object item)
        {
            foreach (var template in _templateList)
                if (template.TargetType.IsAssignableFrom(item.GetType()))
                    return template;

            return null;
        }

        #region IList implementation

        public TreeItemDataTemplate this[int index] { get => ((IList<TreeItemDataTemplate>)_templateList)[index]; set => ((IList<TreeItemDataTemplate>)_templateList)[index] = value; }

        public int Count => ((ICollection<TreeItemDataTemplate>)_templateList).Count;

        public bool IsReadOnly => ((ICollection<TreeItemDataTemplate>)_templateList).IsReadOnly;

        public void Add(TreeItemDataTemplate item)
        {
            ((ICollection<TreeItemDataTemplate>)_templateList).Add(item);
        }

        public void Clear()
        {
            ((ICollection<TreeItemDataTemplate>)_templateList).Clear();
        }

        public bool Contains(TreeItemDataTemplate item)
        {
            return ((ICollection<TreeItemDataTemplate>)_templateList).Contains(item);
        }

        public void CopyTo(TreeItemDataTemplate[] array, int arrayIndex)
        {
            ((ICollection<TreeItemDataTemplate>)_templateList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<TreeItemDataTemplate> GetEnumerator()
        {
            return ((IEnumerable<TreeItemDataTemplate>)_templateList).GetEnumerator();
        }

        public int IndexOf(TreeItemDataTemplate item)
        {
            return ((IList<TreeItemDataTemplate>)_templateList).IndexOf(item);
        }

        public void Insert(int index, TreeItemDataTemplate item)
        {
            ((IList<TreeItemDataTemplate>)_templateList).Insert(index, item);
        }

        public bool Remove(TreeItemDataTemplate item)
        {
            return ((ICollection<TreeItemDataTemplate>)_templateList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<TreeItemDataTemplate>)_templateList).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_templateList).GetEnumerator();
        }

        #endregion
    }
}
