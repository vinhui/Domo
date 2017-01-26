using System;

namespace Domo.Modules
{
    /// <summary>
    /// Attribute to put on fields with the same type as the generics in the class
    /// This will automatically populate the field with an instance of the generic type
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoFillGenericAttribute : Attribute
    {
    }
}