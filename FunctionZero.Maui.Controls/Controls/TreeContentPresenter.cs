using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class TreeContentPresenter : ContentPresenter
    {
        /// <summary>
        /// A standard ContentPresenter sets Content.BindingContext to the BindingContext of the TemplatedParent.
        /// This specialisation allows us to specify a Content.BindingContext, by setting the BindingContext of 'this'
        /// when declaring the (Tree)ContentPresenter.
        /// Note the BC of the TemplatedParent for a ContentTemplate used by the TreeView will be a TreeNodeContainer. 
        /// Our UI wants to bind to the 'Data' property on that instance.
        /// It does so like this: ... BindingContext="{TemplateBinding BindingContext.Data, Mode=OneWay}"
        /// OneWay is required (as opposed to OneTime) because the Content can be recycled by the ListViewZero.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Content.BindingContext = BindingContext;
        }
    }
}
