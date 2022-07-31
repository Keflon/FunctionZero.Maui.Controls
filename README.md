# Controls
[NuGet package](https://www.nuget.org/packages/FunctionZero.Maui.Controls)

## TreeViewZero
![Sample image](https://github.com/Keflon/FunctionZero.Maui.Controls/blob/master/AndroidTree.png?raw=true)

This control allows you to visualise a tree of any data. Each *trunk* node must provide its children using a public property that supports the IEnumerable interface.  
If the children are in a collection that supports `INotifyCollectionChanged` the control will track changes to the underlying tree data.  
Children are lazy-loaded and the UI is virtualised.  

### TreeViewZero exposes the following properties
Property | Type | Bindable | Purpose
:----- | :---- | :----: | :-----
ItemsSource        | string       | Yes | The name of the property used to find the node children
TreeItemTemplate   | TemplateProvider | Yes | Set this to a `TreeItemDataTemplate` or a `TreeDataTemplateSelector`
ItemContainerStyle | Style            | Yes | An optional `Style` that can be applied to the `TreeNodeZero` objects that represent each node.
IsRootVisible      | bool             | Yes | Specifies whether the root node should be shown or omitted.
IndentMultiplier   | double           | Yes (OneTime) | How far the TreeNode should be indented for each nest level. Default is 15.0

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
                <!--Tip: The HeightRequest ensures the chevrons aren't too small to tap with your finger-->
                <Label Text="{Binding Name}" HeightRequest="100" />
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

   private bool _isMyNodeExpanded;
   public bool IsMyNodeExpanded
   {
      get => _isMyNodeExpanded;
      set => SetProperty(ref _isMyNodeExpanded, value);
   }

   public ObservableCollection<MyObservableNode> Children {get; set;}
}
```
This is how to bind the `IsMyNodeExpanded` from our data, to `IsExpanded` on the TreeNode ...

```xml
<cz:TreeViewZero.TreeItemTemplate>
    <cz:TreeItemDataTemplate ChildrenPropertyName="Children" IsExpandedPropertyName="IsMyNodeExpanded">
        <DataTemplate>
            ...
        </DataTemplate>
    </cz:TreeItemDataTemplate>
</cz:TreeViewZero.TreeItemTemplate>
```
### TreeDataTemplateSelector
If your tree of data consists of disparate nodes with different properties for their `Children`, 
use a `TreeDataTemplateSelector` and set `TargetType` for each `TreeItemDataTemplate`.  

Note: In this example, the tree data can contain nodes of type `LevelZero`, `LevelOne` and `LevelTwo` where each type has a different property to provide its children.  

The first `TargetType` assignable from the node is used. Put another way, the first `TargetType` the node can be cast to, wins.
```xml
<cz:TreeViewZero ItemsSource="{Binding SampleTemplateTestData}" >
    <cz:TreeViewZero.TreeItemTemplate>
        <cz:TreeDataTemplateSelector>
            <cz:TreeItemDataTemplate ChildrenPropertyName="LevelZeroChildren" TargetType="{x:Type test:LevelZero}" IsExpandedPropertyName="IsLevelZeroExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Yellow" />
                </DataTemplate>
            </cz:TreeItemDataTemplate>

            <cz:TreeItemDataTemplate ChildrenPropertyName="LevelOneChildren" TargetType="{x:Type test:LevelOne}" IsExpandedPropertyName="IsLevelOneExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Cyan" />
                </DataTemplate>
            </cz:TreeItemDataTemplate>

            <cz:TreeItemDataTemplate ChildrenPropertyName="LevelTwoChildren" TargetType="{x:Type test:LevelTwo}" IsExpandedPropertyName="IsLevelTwoExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Pink" />
                </DataTemplate>
            </cz:TreeItemDataTemplate>

            <cz:TreeItemDataTemplate ChildrenPropertyName="LevelThreeChildren" TargetType="{x:Type test:LevelThree}" IsExpandedPropertyName="IsLevelThreeExpanded">
                <DataTemplate>
                    <Label Text="{Binding Name}" BackgroundColor="Crimson" />
                </DataTemplate>
            </cz:TreeItemDataTemplate>
        </cz:TreeDataTemplateSelector>
    </cz:TreeViewZero.TreeItemTemplate>
</cz:TreeViewZero>
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

## Styling the TreeNodeContainer
Do this if you want to change the way the whole Tree-Node is drawn, e.g. to replace the *chevron*. 
It is a two-step process.
1. Create a `ControlTemplate` for a `TreeNodeZero`
1. Apply it to the `TreeViewZero`

### Step 1 - Create a `ControlTemplate` ...

The `BindingContext` of the templated parent is a `TreeNodeContainer` and has the following properties you can access: 
 

Property    | Type   | Purpose
:----- | :----: | :-----
Indent      | int    | How deep the node should be indented. It is equal to `NestLevel`, or `NestLevel-1` if the Tree Root is not shown.
NestLevel   | int    | The depth of the node in the data.
IsExpanded  | bool   | This property reflects whether the TreeNode is expanded.
ShowChevron | bool   | Whether the chevron is drawn. True if the node has children.
Data        | object | This is the tree-node data for this TreeViewZero instance, i.e. your data!

You can base the `ControlTemplate` on the default, show here, or bake your own entirely.  
```xml
<ControlTemplate x:Key="defaultControlTemplate">
    <HorizontalStackLayout HeightRequest="{Binding Height, Mode=OneWay, Source={x:Reference tcp}}"
        Padding="{TemplateBinding BindingContext.Indent, Converter={StaticResource NestLevelConverter}, ConverterParameter={x:Reference tcp}, Mode=OneWay}">
        <controls:Chevron 
            IsExpanded="{TemplateBinding BindingContext.IsExpanded, Mode=TwoWay}" 
            ShowChevron="{TemplateBinding BindingContext.ShowChevron, Mode=TwoWay}" 
        />
        <ContentPresenter VerticalOptions="Start" x:Name="tcp" HorizontalOptions="StartAndExpand" BindingContext="{TemplateBinding BindingContext.Data}" />
    </HorizontalStackLayout>
</ControlTemplate>
```
**Note:** The `NestLevelConverter` in this example is given a `ConverterParameter` set to 'any control within the template', 
so it can look up the visual-tree to find the `TreeViewZero`, so it can get access to the `IndentMultiplier` property. 
If you know a better way please let me know.  
If you give it a hard-coded number it'll use that instead, or you could TemplateBind to BindingContext.Data to get 
access to the underlying data. or you could write your own converter.
### Step 2 - give it to the TreeView ...
```xml
<cz:TreeViewZero ItemsSource="{Binding SampleData}">
    <cz:TreeViewZero.TreeItemContainerStyle>
        <Style TargetType="cz:TreeNodeZero">
            <Setter Property="ControlTemplate" Value="{StaticResource NodeTemplate}"/>
        </Style>
    </cz:TreeViewZero.TreeItemContainerStyle>

    <cz:TreeViewZero.TreeItemTemplate>
       ...
    </cz:TreeViewZero.TreeItemTemplate>
</cz:TreeViewZero>
```

## Known issues:
There are two known issues, both in the **WinUI** platform, both relating to the underlying `CollectionView` used by `TreeViewZero`.  
1. The `CollectionView` has a minimum item-spacing bug, reported [here](https://github.com/dotnet/maui/issues/4520)
2. The `CollectionView` is not recycling containers, reported [here](https://github.com/dotnet/maui/issues/8151)  

I'll update the source and [NuGet package](https://www.nuget.org/packages/FunctionZero.Maui.Controls) once these bugs are fixed, if necessary.
  
