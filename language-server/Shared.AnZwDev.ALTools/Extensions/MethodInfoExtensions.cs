using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class MethodInfoExtensions
    {

        public static MethodInfo MakeGenericMethod(this MethodInfo method, ParameterInfo[] methodParameters, object[] parametersValues)
        {
            //collect generic parameters
            Dictionary<string, Type> paramTypes = new Dictionary<string, Type>();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType.IsGenericParameter)
                {
                    if (!paramTypes.ContainsKey(methodParameters[i].ParameterType.Name))
                        paramTypes.Add(methodParameters[i].ParameterType.Name, parametersValues[i].GetType());
                }
            }
            //create generic parameter types
            Type[] types = method.GetGenericArguments();
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = paramTypes[types[i].Name];
            }

            return method.MakeGenericMethod(types);
        }


    }
}
