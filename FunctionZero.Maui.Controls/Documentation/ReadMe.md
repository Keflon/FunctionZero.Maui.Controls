# Controls

## TreeViewZero

This control can databind to a tree of data. Each *trunk* node provides its children using any public property that supports IEnumerable.  

Given a hierarchy of `MyNode`
```csharp
public class MyNode
{
   public string Name {get; set;}
   public IEnumerable<MyNode> MyNodeChildren {get; set;}
}
```

Add the namespace:
```xml
xmlns:cz="clr-namespace:FunctionZero.Maui.Controls;assembly=FunctionZero.Maui.Controls"
```

You can display a tree of these nodes like this:
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
## TreeItemDataTemplate
This has 3 properties  
put a grid here
copy MS documentation.
## Tracking changes in the data
If the children of a node support `INotifyCollectionChanged`, the TreeView will track all changes automatically.  
If a property called 
DataBinding is fully supported.  
TreeViewZero will track changes to `Name`, `IsExpanded` and any 
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

   public ObservableCollection<MyLovelyNode> Children {get; set;}
}
```

If your tree of data consists of disparate nodes with different properties for their `Children`, 
use a `TreeDataTemplateSelector` and set `TargetType` for each TreeItemDataTemplate.  
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
## Customising TreeDataTemplateSelector
If you want full-control over the `TreeItemTemplate` per node, you can implement your own 
`TreeDataTemplateSelector` and override `OnSelectTemplate`. Here's an example that chooses a template 
based on whether the node has children or not:

```csharp
public class MyLovelyTreeDataTemplateSelector : TemplateProvider
   {
      /// These must be set in the xaml markup. (or code-behind, if that's how you roll)
      public TreeItemDataTemplate TrunkTemplate{ get; set; }
      public TreeItemDataTemplate LeafTemplate{ get; set; }

      public override TreeItemDataTemplate OnSelectTemplate(object item)
      {
         var vm = (MyLovelyNode)item;
         if((vm.Children != null) && (vm.Children.Count != 0))
         {
            return TrunkTemplate;
         }
         return LeafTemplate;
      }
   }
}
```
That's all there is to it.  
Take a look at [TreeDataTemplateSelector.cs](https://github.com/Keflon/FunctionZero.Maui.Controls/blob/master/FunctionZero.Maui.Controls/TreeDataTemplateSelector.cs) 
for an example of how to provide a collection of `TreeItemDataTemplate` instances to your TemplateProvider.