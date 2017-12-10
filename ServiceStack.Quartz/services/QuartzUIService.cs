// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/. 
namespace ServiceStack.Quartz
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Quartz.Util;
    using ServiceStack.IO;
    using ServiceStack.Templates;
    using ServiceStack.VirtualPath;

    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QuartzUIService : Service
    {
        private readonly TemplateContext _ctx;
        //public ITemplatePages Pages { get; set; }

        public QuartzUIService()
        {
            // init a custom template context
            _ctx = new TemplateContext
            {
                VirtualFiles = this.VirtualFileSources,
                DebugMode = true,
                RenderExpressionExceptions = true,
                CheckForModifiedPages = true,
                AssignExceptionsTo = "ex",
                TemplateFilters =
                {
                    new TemplateProtectedFilters(),
                    new TemplateInfoFilters(),
                    new TemplateServiceStackFilters(),
                }
            }.Init();
            _ctx.TemplateFilters.Insert(0, new QuartzFilters());

            // data that doesn't change after init
            // groups/jobs registered
            // groups/triggers registered

            // things that change
            // job data, state, history, status
        }

        public object Get(ViewQuartzPage request)
        {
            var page = _ctx.GetPage(Request.PathInfo.Replace("/quartz", "/qui"));

            object model = null;
            if (request.Page.IsNullOrWhiteSpace())
                model = Gateway.Send(new GetQuartzSummary());
            else if (request.Page.EqualsIgnoreCase("jobs"))
                model = Gateway.Send(new GetQuartzJobs());
            else if (request.Page.EqualsIgnoreCase("triggers"))
                model = Gateway.Send(new GetQuartzTriggers());
            else if (request.Page.EqualsIgnoreCase("history"))
                model = Gateway.Send(new GetQuartzHistory());

            return new PageResult(page)
            {
                Model = model,
                Args =
                {
                    ["PathInfo"] = Request.PathInfo // add the pathinfo
                }
            }.Result;
        }
    }

    public class QuartzFilters : TemplateFilter
    {
        public bool matchesPathInfo(TemplateScopeContext scope, string pathInfo)
        {
            var page = scope.Page;
            return scope.GetValue("PathInfo")?.ToString().TrimEnd('/') == pathInfo?.TrimEnd('/');
        }

        public object ifMatchesPathInfo(TemplateScopeContext scope, object returnTarget, string pathInfo) =>
            matchesPathInfo(scope, pathInfo) ? returnTarget : null;
    }

    [Route("/quartz")]
    [Route("/quartz/{page}")]
    public class ViewQuartzPage
    {
        public string Page { get; set; }
    }
}