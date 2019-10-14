using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy.Parameters;
using Cosmos.Extensions;
using Cosmos.Validations.Parameters.Internals;

namespace Cosmos.Validations.Parameters
{
    /// <summary>
    /// Valid date
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ValidDateAttribute : ParameterInterceptorAttribute, IValidationParameter
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task Invoke(ParameterAspectContext context, ParameterAspectDelegate next)
        {
            if (context.Parameter.IsDateTimeType())
                context.Parameter.TryTo<DateTime?>().SafeValue().CheckValidDate(context.Parameter.Name, Message);
            return next(context);
        }
    }
}