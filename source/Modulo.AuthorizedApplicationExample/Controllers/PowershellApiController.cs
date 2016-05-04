using Hangfire;
using System.Web.Http;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Serilog;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Modulo.AuthorizedApplicationExample.Areas.Api
{

    public class PowershellApiController : ApiController
    {
        ToPhysicalPath ToPhysicalPath;

        public PowershellApiController(ToPhysicalPath toPhysicalPath)
        {
            ToPhysicalPath = toPhysicalPath;
        }

        public class GetScriptViewModel
        {
            public string FormAction { get; set; }
            public string FormMethod { get; set; }
            public List<string> Parameters { get; set; }

            public GetScriptViewModel()
            {
                Parameters = new List<string>();
            }
        }

        //[HttpGet, Route("~/api/script/{scriptName}")]
        //public IHttpActionResult Get(string scriptName)
        //{
        //    var basePath = ToPhysicalPath("~/Scripts");
        //    var foundFiles = Directory.EnumerateFiles(basePath, scriptName + ".*");

        //    var viewModel = new GetScriptViewModel()
        //    {
        //        FormAction = Url.Content("~/api/script/" + scriptName),
        //        FormMethod = "POST"
        //    };

        //    foreach (var filePath in foundFiles)
        //    {
        //        var fileInfo = new FileInfo(filePath);
        //        if (fileInfo.Extension.ToLower() == ".ps1")
        //        {
        //            var result = filePath.IsSubPathOf(basePath);

        //            if (result == false)
        //            {
        //                continue;
        //            }

        //            var fileContent = File.ReadAllText(fileInfo.FullName);

        //            Collection<PSParseError> errors;
        //            var tokens = PSParser.Tokenize(fileContent, out errors);

        //            if ((tokens[0].Type == PSTokenType.Keyword) & (tokens[0].Content == "param"))
        //            {
        //                var parameters = tokens.Skip(2).TakeWhile(x => x.Type != PSTokenType.GroupEnd).Where(x => x.Type == PSTokenType.Variable);
        //                foreach (var item in parameters)
        //                {
        //                    viewModel.Parameters.Add(item.Content);
        //                }
        //            }

        //            return new ViewResult(Request, "ScriptForm", viewModel);
        //        }
        //    }

        //    return NotFound();
        //}

        [ConfigKeyAuthorizeAttribute]
        [HttpPost, Route("~/api/script/{scriptName}")]
        public IHttpActionResult Run(string scriptName, [FromBody]object[] values)
        {
            var basePath = ToPhysicalPath("~/Scripts");
            var foundFiles = Directory.EnumerateFiles(basePath, scriptName + ".*");
            
            foreach (var filePath in foundFiles)
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Extension.ToLower() == ".ps1")
                {
                    var result = filePath.IsSubPathOf(basePath);

                    if (result == false)
                    {
                        continue;
                    }

                    var executionId = Guid.NewGuid();
                    var parameters = values.OfType<KeyValuePair<string, string>>().Select(x => x.Value).ToArray();
                    BackgroundJob.Enqueue(() => RunPowershell(executionId, filePath, parameters));

                    return new StatusCodeResult(System.Net.HttpStatusCode.Accepted, this);
                }
            }

            return NotFound();
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public void RunPowershell(Guid executionId, string scriptPath, object[] parameters)
        {
            using (var powershell = PowerShell.Create())
            {
                var log = Log.Logger
                    .ForContext("ExecutionId", executionId)
                    .ForContext("PowershellId", powershell.InstanceId);

                powershell.Runspace.SessionStateProxy.SetVariable("VerbosePreference", "Continue");

                var fileContent = File.ReadAllText(scriptPath);

                powershell.Streams.Debug.DataAdded += (s, e) =>
                {
                    var msg = powershell.Streams.Debug[e.Index];
                    log.Debug(msg.Message);
                };
                powershell.Streams.Error.DataAdded += (s, e) =>
                {
                    var msg = powershell.Streams.Error[e.Index];
                    log.Error(msg.Exception, "{ErrorId}", msg.FullyQualifiedErrorId);
                };
                powershell.Streams.Progress.DataAdded += (s, e) =>
                {
                    var msg = powershell.Streams.Progress[e.Index];
                    log.Information("Progress: {Activity} {Operation} {Status} {PercentComplete}", msg.CurrentOperation, msg.Activity, msg.StatusDescription, msg.PercentComplete);
                };
                powershell.Streams.Verbose.DataAdded += (s, e) =>
                {
                    var msg = powershell.Streams.Verbose[e.Index];
                    log.Verbose(msg.Message);
                };
                powershell.Streams.Warning.DataAdded += (s, e) =>
                {
                    var msg = powershell.Streams.Warning[e.Index];
                    log.Warning(msg.Message);
                };

                powershell.AddScript(fileContent);
                powershell.AddParameters(parameters);
                var output = powershell.Invoke();

                foreach (var item in output)
                {
                }

                log.Information("Output {Output}", output.Select(x => x.ToString()));
            }
        }
    }
}
