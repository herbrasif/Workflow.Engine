using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine
{
    /// <summary>
    /// Represent an action to execute when a workflow is eligible
    /// </summary>
    /// <typeparam name="TEProperty">Enum of properties that can be targeted (FirstName, LastName, ...)</typeparam>
    /// <typeparam name="TEActivityType">Enum of type of activity available (set, sendMail, ...)</typeparam>
    public class ActivityLine<TEProperty, TEActivityType>
    {
        /// <summary>
        /// Property targeted by this action
        /// </summary>
        public TEProperty Property { get; set; }
        /// <summary>
        /// Type of action to execute
        /// </summary>
        public TEActivityType Type { get; set; }
        /// <summary>
        /// Value to pass in action
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Default constructor, mostly used to serialization
        /// </summary>
        public ActivityLine()
        {

        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="property">Property targeted by this action</param>
        /// <param name="type">Type of action to execute</param>
        /// <param name="value">Default constructor, mostly used to serialization</param>
        public ActivityLine(TEProperty property, TEActivityType type, string value)
        {
            Property = property;
            Type = type;
            Value = value;
        }
    }
}
