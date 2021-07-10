using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project1
{
    public class SwaggerClientOperationNameGenerator : IOperationNameGenerator
    {
        public bool SupportsMultipleClients { get; } = true;

        /// <summary>Converts the path to an operation name.</summary>
        /// <param name="path">The HTTP path.</param>
        /// <returns>The operation name.</returns>
        public static string ConvertPathToName(string path)
        {
            var name = Regex.Replace(path, @"\{.*?\}", "")
                .Split('/', '-', '_')
                .Where(part => !part.Contains("{") && !string.IsNullOrWhiteSpace(part))
                .Aggregate("", (current, part) =>
                {
                    part.Replace("get", "Get");
                    part.Replace("post", "Post");
                    part.Replace("delete", "Delete");
                    part.Replace("update", "Update");
                    return (current + CapitalizeFirst(part));
                });

            if (string.IsNullOrEmpty(name))
            {
                name = "Index"; // Root path based operation?
            }

            return name;
        }

        /// <summary>Capitalizes first letter.</summary>
        /// <param name="name">The name to capitalize.</param>
        /// <returns>Capitalized name.</returns>
        internal static string CapitalizeFirst(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var capitalized = name;
            return char.ToUpperInvariant(capitalized[0]) + (capitalized.Length > 1 ? capitalized.Substring(1) : "");
        }



        public string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            return string.Empty;
        }



        public string GetOperationName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            var operationName = ConvertPathToName(path);

            operationName += CapitalizeFirst(httpMethod.ToString());
            if (string.IsNullOrEmpty(httpMethod.ToString()))
            {
                Console.WriteLine("fff");
            }

            return operationName;
        }
    }
}
