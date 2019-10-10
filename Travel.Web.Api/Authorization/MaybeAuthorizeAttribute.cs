using System;

namespace Travel.Web.Api.Authorization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class MaybeAuthorizeAttribute : Attribute
    {
    }
}
