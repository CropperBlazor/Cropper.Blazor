using System.Reflection;

namespace Cropper.Blazor.Client.Components.Docs
{
    public class ApiProperty
    {
        public string Name { get; set; }
        public Type Type { get; set; }
<<<<<<< HEAD
        public PropertyInfo? PropertyInfo { get; set; }
=======
        public PropertyInfo PropertyInfo { get; set; }
>>>>>>> origin/master
        public string Description { get; set; }
        public object Default { get; set; }
        public bool IsTwoWay { get; set; }
    }
}
