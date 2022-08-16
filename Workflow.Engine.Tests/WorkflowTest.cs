using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine.Tests
{
    public enum EWorkflowTestProperty
    {
        Alpha,
        Beta
    }

    public enum EWorkflowTestEligibility
    {
        Equals,
        Contains
    }
    public enum EWorkflowTestActivity
    {
        Set
    }

    public class WorkflowTest : Workflow<EWorkflowTestProperty, EWorkflowTestEligibility, EWorkflowTestActivity, Test, Test>
    {
        public WorkflowTest()
        {
            this.DefineEligibilities();
            this.DefineActivities();
        }

        private void DefineEligibilities()
        {
            this.EligibilityMethods.Add(EWorkflowTestProperty.Alpha, new Dictionary<EWorkflowTestEligibility, delegElibigility>()
            {
                { EWorkflowTestEligibility.Equals, (string value) => this.Item1.Alpha == value },
                { EWorkflowTestEligibility.Contains, (string value) => this.Item1.Alpha.Contains(value) }
            });
            this.EligibilityMethods.Add(EWorkflowTestProperty.Beta, new Dictionary<EWorkflowTestEligibility, delegElibigility>()
            {
                { EWorkflowTestEligibility.Equals, (string value) => this.Item1.Beta == value },
                { EWorkflowTestEligibility.Contains, (string value) => this.Item1.Beta.Contains(value)}
            });
        }

        private void DefineActivities()
        {
            this.ActivityMethods.Add(EWorkflowTestProperty.Alpha, new Dictionary<EWorkflowTestActivity, delegActivity>()
            {
                { EWorkflowTestActivity.Set, async (string value) => await Task.Run(() => throw new Exception("check failed")) }
            });
            this.ActivityMethods.Add(EWorkflowTestProperty.Beta, new Dictionary<EWorkflowTestActivity, delegActivity>()
            {
                { EWorkflowTestActivity.Set, async (string value) => await Task.Run(() =>  this.Item1.Beta = value) }
            });
        }
    }
}
