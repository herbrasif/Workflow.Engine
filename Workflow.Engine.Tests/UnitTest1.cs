using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Workflow.Engine.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(configure => { configure.SetMinimumLevel(LogLevel.Debug); });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            List<WorkflowTest> workflows = new List<WorkflowTest>();
            workflows.Add(new WorkflowTest()
            {
                Name = "Workflow of test",
                EligibilityLines = new List<EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>>()
                        {
                           new EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>(ELogicalOperator.AND, EWorkflowTestProperty.Alpha, EWorkflowTestEligibility.Equals, "Alpha")
                           , new EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>(ELogicalOperator.AND, EWorkflowTestProperty.Alpha, EWorkflowTestEligibility.Contains, "lph")
                           , new EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>(ELogicalOperator.AND,
                                new List<EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>>() {
                                    new EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>(ELogicalOperator.AND, EWorkflowTestProperty.Beta, EWorkflowTestEligibility.Contains, "alpha")
                                    , new EligibilityLine<EWorkflowTestProperty, EWorkflowTestEligibility>(ELogicalOperator.OR, EWorkflowTestProperty.Beta, EWorkflowTestEligibility.Equals, "Beta")
                                }
                            )
                        },
                ActivityLines = new List<ActivityLine<EWorkflowTestProperty, EWorkflowTestActivity>>()
                        {
                            new ActivityLine<EWorkflowTestProperty, EWorkflowTestActivity>(EWorkflowTestProperty.Beta, EWorkflowTestActivity.Set, "Ceta"),
                            new ActivityLine<EWorkflowTestProperty, EWorkflowTestActivity>(EWorkflowTestProperty.Alpha, EWorkflowTestActivity.Set, "Ceta")
                        }
            });

            string json = WorkflowFactory<WorkflowTest>.ToJson(workflows);

            WorkflowFactory<WorkflowTest> factory = new WorkflowFactory<WorkflowTest>(serviceProvider.GetService<ILogger<WorkflowFactory<WorkflowTest>>>());
            await factory.LoadFromJson(json);
            Test t = new Test();

            Assert.True(factory.CheckEligibilityOnly(t));
            await factory.Run(t);
            Assert.Equal("Ceta", t.Beta);
        }
    }
}