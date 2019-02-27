using System;

[AttributeUsage(AttributeTargets.Class)]  
public class NodeCategoryAttribute : Attribute
{
    public string[] categories;

    public int Length => categories.Length;
    
    public string this[int index] => categories[index];
  
    public NodeCategoryAttribute(params string[] categories)
    {
        this.categories = categories;
    }
}  
