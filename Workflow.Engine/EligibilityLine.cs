using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine
{
    /// <summary>
    /// Represent a condition that must be respected to allow workflow to execute his action
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted (FirstName, LastName, ...)</typeparam>
    /// <typeparam name="TEEligibilityType">Enum of type of eligibility available (equals, contains, ...)</typeparam>
    public class EligibilityLine<TEProperty, TEEligibilityType>
    {
        /// <summary>
        /// Logical operator to link to lines between them, only support AND, OR
        /// </summary>
        public ELogicalOperator Operator { get; set; }
        /// <summary>
        /// Property targeted by elibility
        /// </summary>
        public TEProperty? Property { get; set; }
        /// <summary>
        /// Type of eligibility
        /// </summary>
        public TEEligibilityType? Type { get; set; }
        /// <summary>
        /// Value to compare in condition
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// List of sub group of eligibilities (Property and Type are ignored if a group in defined)
        /// </summary>
        public List<EligibilityLine<TEProperty, TEEligibilityType>> Group { get; set; } = null;

        /// <summary>
        /// Defautl constructor, mostly used by serialization
        /// </summary>
        public EligibilityLine()
        {

        }

        /// <summary>
        /// Constructor with parameters for a line
        /// </summary>
        /// <param name="ope">Logical operator to link to lines between them</param>
        /// <param name="property">Property targeted by elibility</param>
        /// <param name="type">Type of eligibility</param>
        /// <param name="value">Value to compare in condition</param>
        public EligibilityLine(ELogicalOperator ope, TEProperty property, TEEligibilityType type, string value)
        {
            Operator = ope;
            Property = property;
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Constructor with parameters for a sub group
        /// </summary>
        /// <param name="ope">Logical operator to link to lines between them</param>
        /// <param name="group">List of sub group of eligibilities</param>
        public EligibilityLine(ELogicalOperator ope, List<EligibilityLine<TEProperty, TEEligibilityType>> group)
        {
            Operator = ope;
            Group = group;
        }
    }
}
