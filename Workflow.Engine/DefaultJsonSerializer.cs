using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Engine
{
    /// <summary>
    /// Default serializer to use
    /// </summary>
    public static class DefaultJsonSerializer
    {
        /// <summary>
        /// Get JsonSerializerSettings with custom configuratioh
        /// </summary>
        public static JsonSerializerSettings Settings
        {
            get
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()); // save enum to string instead of number to be more readable
                return settings;
            }
        }
    }
}
