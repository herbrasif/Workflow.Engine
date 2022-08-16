using System.Collections.Generic;

namespace Workflow.Engine
{
#pragma warning disable S1104 // Fields should not have public accessibility
    /// <summary>
    /// Methods to define an eligibility check
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate bool delegElibigility(string value);
    /// <summary>
    /// Methods to define an activity
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate Task delegActivity(string value);

    /// <summary>
    /// A workflow is a class who will register eligiblities condition and actions tu run when eligibility check succeed
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted</typeparam>
    /// <typeparam name="TEEligibilityType">Enum of type of eligibility that can be used</typeparam>
    /// <typeparam name="TEActivityType">Enum of type of activities that can be used</typeparam>
    /// <typeparam name="T1">Type of item to work with</typeparam>
    public abstract class Workflow<TEProperty, TEEligibilityType, TEActivityType, T1> : IWorkflow
        where TEProperty : Enum where TEEligibilityType : Enum where TEActivityType : Enum
        where T1 : class
    {
        /// <summary>
        /// Name of the workflow, used to find him easily by humans
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Item who will be used in eligibility and activity
        /// </summary>
        public T1 Item1 { get; set; }

        #region Eligibility
        /// <summary>
        /// List of Eligibility methods available for this type of workflow, defined by inheritance
        /// </summary>
        protected Dictionary<TEProperty, Dictionary<TEEligibilityType, delegElibigility>> EligibilityMethods = new Dictionary<TEProperty, Dictionary<TEEligibilityType, delegElibigility>>();

        /// <summary>
        /// List of Eligibility methods to check
        /// </summary>
        public List<EligibilityLine<TEProperty, TEEligibilityType>> EligibilityLines = new List<EligibilityLine<TEProperty, TEEligibilityType>>();
        #endregion

        #region Activity
        /// <summary>
        /// List of activities available for this type of workflow, defined by inheritance
        /// </summary>
        protected Dictionary<TEProperty, Dictionary<TEActivityType, delegActivity>> ActivityMethods = new Dictionary<TEProperty, Dictionary<TEActivityType, delegActivity>>();
        /// <summary>
        /// List of activities to execute
        /// </summary>
        public List<ActivityLine<TEProperty, TEActivityType>> ActivityLines = new List<ActivityLine<TEProperty, TEActivityType>>();
        #endregion

        /// <summary>
        /// Check eligibility of the workflow
        /// </summary>
        /// <returns></returns>
        public bool Eligibility()
        {
            return this.Eligibility(EligibilityLines);
        }

        /// <summary>
        /// Check eligibility for each lines defined
        /// </summary>
        /// <param name="lines">List of eligibilities</param>
        /// <returns></returns>
        private bool Eligibility(List<EligibilityLine<TEProperty, TEEligibilityType>> lines)
        {
            if (lines == null)
                lines = EligibilityLines;

            int i = 0;
            int max = lines.Count;
            do
            {
                bool isEligible = false;
                if (lines[i].Group != null)
                    isEligible = this.Eligibility(lines[i].Group);
                else
                    isEligible = EligibilityMethods[lines[i].Property][lines[i].Type](lines[i].Value);

                if (i == max - 1)
                    return isEligible;

                if (isEligible) {
                    if (lines[i + 1].Operator == ELogicalOperator.OR)
                        return true;
                }
                else if (lines[i + 1].Operator == ELogicalOperator.AND)
                    return false;

                i++;
            } while (i < max);
            return false;
        }

        /// <summary>
        /// Define datas to work with, you can pass multiple objects if your workflow implement multiple generic type (T1, T2, T3, ...)
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SetDatas(params object[] datas)
        {
            this.Item1 = datas[0] as T1;
        }

        /// <summary>
        /// Get name of the workflow, used in log to find him easily by humans
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.Name;
        }

        /// <summary>
        /// Run all activities of the workflow
        /// </summary>
        /// <returns></returns>
        public async Task Activity()
        {
            foreach (var activity in ActivityLines)
            {
                await this.ActivityMethods[activity.Property][activity.Type](activity.Value);
            }
        }
    }

    #region Extended Workflows
    /// <summary>
    /// A workflow is a class who will register eligiblities condition and actions tu run when eligibility check succeed
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted</typeparam>
    /// <typeparam name="TEEligibilityType">Enum of type of eligibility that can be used</typeparam>
    /// <typeparam name="TEActivityType">Enum of type of activities that can be used</typeparam>
    /// <typeparam name="T1">Type of main item to work with</typeparam>
    /// <typeparam name="T2">Type of second item to work with</typeparam>
    public abstract class Workflow<TEProperty, TEEligibilityType, TEActivityType, T1, T2> : Workflow<TEProperty, TEEligibilityType, TEActivityType, T1>
        where TEProperty : Enum where TEEligibilityType : Enum where TEActivityType : Enum
        where T1 : class where T2 : class
    {
        /// <summary>
        /// Type of second item to work with
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// Define datas to work with, you can pass multiple objects if your workflow implement multiple generic type (T1, T2, T3, ...)
        /// </summary>
        /// <param name="datas"></param>
        public override void SetDatas(params object[] datas)
        {
            this.Item1 = datas[0] as T1;
            this.Item2 = datas[1] as T2;
        }
    }

    /// <summary>
    /// A workflow is a class who will register eligiblities condition and actions tu run when eligibility check succeed
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted</typeparam>
    /// <typeparam name="TEEligibilityType">Enum of type of eligibility that can be used</typeparam>
    /// <typeparam name="TEActivityType">Enum of type of activities that can be used</typeparam>
    /// <typeparam name="T1">Type of main item to work with</typeparam>
    /// <typeparam name="T2">Type of second item to work with</typeparam>
    /// <typeparam name="T3">Type of third item to work with</typeparam>
    public abstract class Workflow<TEProperty, TEEligibilityType, TEActivityType, T1, T2, T3> : Workflow<TEProperty, TEEligibilityType, TEActivityType, T1, T2>
        where TEProperty : Enum where TEEligibilityType : Enum where TEActivityType : Enum
        where T1 : class where T2 : class where T3 : class
    {
        /// <summary>
        /// Type of third item to work with
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// Define datas to work with, you can pass multiple objects if your workflow implement multiple generic type (T1, T2, T3, ...)
        /// </summary>
        /// <param name="datas"></param>
        public override void SetDatas(params object[] datas)
        {
            this.Item1 = datas[0] as T1;
            this.Item2 = datas[1] as T2;
            this.Item3 = datas[2] as T3;
        }
    }

    /// <summary>
    /// A workflow is a class who will register eligiblities condition and actions tu run when eligibility check succeed
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted</typeparam>
    /// <typeparam name="TEEligibilityType">Enum of type of eligibility that can be used</typeparam>
    /// <typeparam name="TEActivityType">Enum of type of activities that can be used</typeparam>
    /// <typeparam name="T1">Type of main item to work with</typeparam>
    /// <typeparam name="T2">Type of second item to work with</typeparam>
    /// <typeparam name="T3">Type of third item to work with</typeparam>
    /// <typeparam name="T4">Type of four item to work with</typeparam>
    public abstract class Workflow<TEProperty, TEEligibilityType, TEActivityType, T1, T2, T3, T4> : Workflow<TEProperty, TEEligibilityType, TEActivityType, T1, T2, T3>
        where TEProperty : Enum where TEEligibilityType : Enum where TEActivityType : Enum
        where T1 : class where T2 : class where T3 : class where T4 : class
    {
        /// <summary>
        /// Type of four item to work with
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// Define datas to work with, you can pass multiple objects if your workflow implement multiple generic type (T1, T2, T3, ...)
        /// </summary>
        /// <param name="datas"></param>
        public override void SetDatas(params object[] datas)
        {
            this.Item1 = datas[0] as T1;
            this.Item2 = datas[1] as T2;
            this.Item3 = datas[2] as T3;
            this.Item4 = datas[3] as T4;
        }
    }
    #endregion
#pragma warning restore S1104 // Fields should not have public accessibility
}