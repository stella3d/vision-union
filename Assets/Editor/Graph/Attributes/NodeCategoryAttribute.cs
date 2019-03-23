using System;

[AttributeUsage(AttributeTargets.Class)]  
public class NodeCategoryAttribute : Attribute
{
    public readonly string[] categories;

    public int Length => categories.Length;

    public string this[int index] => categories[index];
  
    public NodeCategoryAttribute(string topLevel, string secondLevel, string thirdLevel)
    {
        categories = new [] {topLevel, secondLevel, thirdLevel};
    }
}  
