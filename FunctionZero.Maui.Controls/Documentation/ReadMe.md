# Controls

## TreeViewZero

This control can databind to a tree of data as long as each node has a property 
that exposes the node children.  
Given a hierarchy of `MyLovelyNode`
```csharp
public class MyLovelyNode
{
   public string Name {get; set;}
   public IEnumerable<MyLovelyNode> Children {get; set;}
}
```

Add the namespace:
```xml
xmlns:cz="clr-namespace:FunctionZero.Maui.Controls;assembly=FunctionZero.Maui.Controls"
```

Instantiate the control and provide it with a root-node and a datatemplate for each node:
```xml
<cz:TreeViewZero ItemsSource="{Binding RootNode}">
    <cz:TreeViewZero.TreeItemTemplate>
        <tv:TreeItemDataTemplate ChildrenPropertyName="Children">
            <DataTemplate>
                <Label Text="{Binding Name}" />
            </DataTemplate>
        </cz:TreeItemDataTemplate>
    </cz:TreeViewZero.TreeItemTemplate>
</tv:TreeViewZero>
```


DataBinding is fully supported.  
TreeViewZero will track changes to `Name`, `IsExpanded` and any 
modifications to the `Children` collection on the following node:
```csharp
public class MyLovelyExpandyNode : BaseClassWithInpc
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
For any node, the first `TreeItemDataTemplate` whose `TargetType` is assignable from the node is used.
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

If you want full-control over the `TreeItemTemplate` per node, you can implement your own 
`TreeDataTemplateSelector` like this:
```csharp

```