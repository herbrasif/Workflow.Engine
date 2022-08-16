using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine
{
    public class WorkflowFactory<T> where T : IWorkflow, new()
    {
        private readonly ILogger logger;
        public List<IWorkflow> workflows = new List<IWorkflow>();

        public WorkflowFactory(ILogger<WorkflowFactory<T>> logger)
        {
            this.logger = logger;
        }

        public Task LoadFromJson(string json)
        {
            logger.LogTrace("WorkflowFactory {0} - Start load from Json", typeof(T));
            return Task.Run(() =>
            {
                try
                {
                    var importedWorkflows = JsonConvert.DeserializeObject<IEnumerable<T>>(json, DefaultJsonSerializer.Settings);
                    workflows.AddRange((IEnumerable<IWorkflow>)importedWorkflows);
                    logger.LogTrace("WorkflowFactory {0} - End of load from Json", typeof(T));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error on WorkflowFactory {0}", typeof(T));
                }
            });
        }

        public bool CheckEligibilityOnly(params object[] datas)
        {
            bool result = true;
            logger.LogTrace("WorkflowFactory {0} - Check eligibility start", typeof(T));
            this.workflows.ForEach(w =>
            {
                try
                {
                    w.SetDatas(datas);
                    logger.LogTrace("WorkflowFactory {0} - Check eligibility of workflow {1}", typeof(T), w.GetName());
                    result &= w.Eligibility();
                    logger.LogTrace("WorkflowFactory {0} - End of check eligibility of workflow {1}", typeof(T), w.GetName());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error on WorkflowFactory {0}", typeof(T));
                }
            });
            logger.LogTrace("WorkflowFactory {0} - End of Check eligibility", typeof(T));
            return result;
        }

        public async Task Run(params object[] datas)
        {
            logger.LogTrace("WorkflowFactory {0} - Run start", typeof(T));
            await Task.Run(() =>
            {
                this.workflows.ForEach(async w =>
                {
                    try
                    {
                        w.SetDatas(datas);
                        logger.LogTrace("WorkflowFactory {0} - Check eligibility of workflow {1}", typeof(T), w.GetName());
                        bool eligibility = w.Eligibility();
                        logger.LogTrace("WorkflowFactory {0} - End of check eligibility of workflow {1} - result : {2}", typeof(T), w.GetName(), eligibility);
                        logger.LogTrace("WorkflowFactory {0} - execute activity of workflow {1}", typeof(T), w.GetName());
                        await w.Activity();
                        logger.LogTrace("WorkflowFactory {0} - End of activity of workflow {1}", typeof(T), w.GetName());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error on WorkflowFactory {0}", typeof(T));
                    }
                });
                logger.LogTrace("WorkflowFactory {0} - End of run", typeof(T));
            });
        }

        public static string ToJson(IEnumerable<T> workflows)
        {
            return JsonConvert.SerializeObject(workflows, DefaultJsonSerializer.Settings);
        }
    }
}
