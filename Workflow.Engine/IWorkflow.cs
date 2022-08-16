using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine
{
    /// <summary>
    /// Interface to define base of a workflow
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Give the name of a workflow
        /// </summary>
        /// <returns></returns>
        public string GetName();
        /// <summary>
        /// Set datas to use in workflow run
        /// </summary>
        /// <param name="datas">Datas will be objects of same type of workflow</param>
        public void SetDatas(params object[] datas);
        /// <summary>
        /// Check eligibility of workflow to allow him or not to execute his activities
        /// </summary>
        /// <returns></returns>
        public bool Eligibility();
        /// <summary>
        /// Actions to perform when a workflow is eligilible
        /// </summary>
        /// <returns></returns>
        public Task Activity();
    }
}
