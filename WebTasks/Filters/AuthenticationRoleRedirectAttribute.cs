﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace WebTasks.Filters
{
    public class AuthenticationRoleRedirectAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}