﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Tools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeWeb.Core.CustomPolicy
{
    public class AllowPrivatePolicy : IAuthorizationRequirement
    {
    }

    public class AllowPrivateHandler : AuthorizationHandler<AllowPrivatePolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowPrivatePolicy requirement)
        {
            try
            {
                string[] allowedUsers = context.Resource as string[];

                if (allowedUsers.Any(user => user.Equals(context.User.Identity.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
    }
}
