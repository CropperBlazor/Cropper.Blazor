using System.Reflection;

namespace Cropper.Blazor.Client.Components.Docs
{
    public class ApiMethod
    {
        public string Signature { get; set; }
        public ParameterInfo Return { get; set; }
        public string Documentation { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public ParameterInfo[] Parameters { get; set; }
    }
}
