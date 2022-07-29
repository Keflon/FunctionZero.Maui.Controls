# Controls

## TreeViewZero

This control allows you to visualise a tree of any data. Each *trunk* node must provide its children using a public property that supports the IEnumerable interface.  
If the children are in a collection that supports `INotifyCollectionChanged` the control will track changes to the underlying tree data.  
Children are lazy-loaded and the UI is virtualised.  
  
asdfsd

### TreeViewZero exposes the following properties
Property | Type | Bindable | Purpose
:----- | :---- | :----: | :-----
ItemsSource        | string       | Yes | The name of the property used to find the node children
TreeItemTemplate   | TemplateProvider | Yes | Set this to a `TreeItemDataTemplate` or a `TreeDataTemplateSelector`
ItemContainerStyle | Style            | Yes | An optional `Style` that can be applied to the `TreeNodeZero` objects that represent each node.
IsRootVisible      | bool             | Yes | Specifies whether the root node should be shown or omitted.

### TreeItemDataTemplate
`TreeItemDataTemplate` tells a tree-node how to draw itself, how to get its children and whether it should bind `IsExpanded` to the underlying data.  
It declares the following properties:

Property | Type | Purpose
:----- | :---- | :-----
ChildrenPropertyName    | string       | The name of the property used to find the node children
IsExpandedPropertyName  | string       | The name of the property used to store whether the node is expanded
ItemTemplate            | DataTemplate | The DataTemplate used to draw this node
TargetType              | Type         | When used in a `TreeDataTemplateSelector`, identifies the least-derived nodes the ItemTemplate can be applied to.

### Create a TreeViewZero

Given a hierarchy of `MyNode`
```csharp
public class MyNode
{
   public string Name { get; set;}
   public IEnumerable<MyNode> MyNodeChildren { get; set; }
}
```

Add the namespace:
```xml
xmlns:cz="clr-namespace:FunctionZero.Maui.Controls;assembly=FunctionZero.Maui.Controls"
```

Then declare a `TreeViewZero` like this:
```xml
<cz:TreeViewZero ItemsSource="{Binding RootNode}">
    <cz:TreeViewZero.TreeItemTemplate>
        <cz:TreeItemDataTemplate ChildrenPropertyName="MyNodeChildren">
            <DataTemplate>
                <Label Text="{Binding Name}" />
            </DataTemplate>
        </cz:TreeItemDataTemplate>
    </cz:TreeViewZero.TreeItemTemplate>
</cz:TreeViewZero>
```

## Tracking changes in the data
If the children of a node support `INotifyCollectionChanged`, the TreeView will track all changes automatically.  
If the properties on your node support `INotifyPropertyChanged` then they too will be tracked.  

For example, TreeViewZero will track changes to `Name`, `IsExpanded` and any 
modifications to the `Children` collection on the following node:
```csharp
public class MyObservableNode : BaseClassWithInpc
{
   private string _name;
   public string Name
   {
      get => _name;
      set => SetProperty(ref _name, value);
   }

   private bool _isExpanded;
   public bool IsExpanded
   {
      get => _isExpanded;
      set => SetProperty(ref _isExpanded, value);
   }

   public ObservableCollection<MyObservableNode> Children {get; set;}
}
```
### TreeDataTemplateSelector
If your tree of data consists of disparate nodes with different properties for their `Children`, 
use a `TreeDataTemplateSelector` and set `TargetType` for each `TreeItemDataTemplate`.  

Note: In this example, the tree data can contain nodes of type `LevelZero`, `LevelOne` and `LevelTwo` where each type has a different property to provide its children.  

The first `TargetType` assignable from the node is used. Put another way, the first `TargetType` the node can be cast to, wins.
```xml
<cv:TreeViewZero ItemsSource="{Binding SampleTemplateTestData}" >
    <cv:TreeViewZero.TreeItemTemplate>
        <cv:TreeDataTemplateSelector>
            <cv:TreeItemDataTemplate ChildrenPropertyName="LevelZeroChildren" TargetType="{x:Type test:LevelZero}" IsExpandedPropertyName="IsLevelZeroExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Yellow" />
                </DataTemplate>
            </cv:TreeItemDataTemplate>

            <cv:TreeItemDataTemplate ChildrenPropertyName="LevelOneChildren" TargetType="{x:Type test:LevelOne}" IsExpandedPropertyName="IsLevelOneExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Cyan" />
                </DataTemplate>
            </cv:TreeItemDataTemplate>

            <cv:TreeItemDataTemplate ChildrenPropertyName="LevelTwoChildren" TargetType="{x:Type test:LevelTwo}" IsExpandedPropertyName="IsLevelTwoExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Pink" />
                </DataTemplate>
            </cv:TreeItemDataTemplate>

            <cv:TreeItemDataTemplate ChildrenPropertyName="LevelThreeChildren" TargetType="{x:Type test:LevelThree}" IsExpandedPropertyName="IsLevelThreeExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Crimson" />
                </DataTemplate>
            </cv:TreeItemDataTemplate>
        </cv:TreeDataTemplateSelector>
    </cv:TreeViewZero.TreeItemTemplate>
</cv:TreeViewZero>
```
### Customising TreeDataTemplateSelector
If you want **full-control** over the `TreeItemTemplate` per node, you can easily implement your own 
`TreeDataTemplateSelector` and override `OnSelectTemplate`. Here's an example that chooses a template 
based on whether the node has children or not:

```csharp
public class MyTreeDataTemplateSelector : TemplateProvider
{
   /// These should be set in the xaml markup. (or code-behind, if that's how you roll)
   public TreeItemDataTemplate TrunkTemplate{ get; set; }
   public TreeItemDataTemplate LeafTemplate{ get; set; }

   public override TreeItemDataTemplate OnSelectTemplate(object item)
   {
      /// Do something based on the incoming node ...
      if(item is MyTreeNode mtn)
      {
         if((mtn.Children != null) && (mtn.Children.Count != 0))
         {
            return TrunkTemplate;
         }
      }
      return LeafTemplate;
   }
}
```
Take a look at [TreeDataTemplateSelector.cs](https://github.com/Keflon/FunctionZero.Maui.Controls/blob/master/FunctionZero.Maui.Controls/TreeDataTemplateSelector.cs) 
for an example of how to provide a *collection* of `TreeItemDataTemplate` instances to your TemplateProvider.