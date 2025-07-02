
using System;

namespace PolicyPro360.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowAnonymousCompanyAttribute : Attribute
    {
    }
}