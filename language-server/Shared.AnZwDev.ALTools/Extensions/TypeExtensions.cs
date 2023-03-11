using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class TypeExtensions
    {

        public static string TryGetPropertyValueAsString(this Type nodeType, object obj, string propertyName)
        {
            object value = nodeType.TryGetPropertyValue(obj, propertyName);
            if (value != null)
                return value.ToString();
            return null;
        }

        public static T TryGetPropertyValue<T>(this Type type, object obj, string propertyName)
        {
            object value = TryGetPropertyValue(type, obj, propertyName);
            if (value != null)
                return (T)value;
            return default(T);
        }

        public static object TryGetPropertyValue(this Type type, object obj, string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
                return propertyInfo.GetValue(obj);
            return null;
        }

        public static bool ValidParameterTypes(this Type[] parameterTypes, ParameterInfo[] parameterInfoList)
        {
            if (parameterTypes.Length != parameterInfoList.Length)
                return false;
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                Type parameterType = parameterInfoList[i].ParameterType;
                if ((parameterTypes[i] != null) &&
                    (parameterTypes[i] != parameterType) &&
                    (!parameterTypes[i].IsSubclassOf(parameterType)) &&
                    (!parameterType.IsGenericParameter))
                    return false;
            }
            return true;
        }

        public static T CallStaticMethod<T>(this Type type, string name, params object[] parameters)
        {
            Type[] parameterTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if ((object)parameters[i] == Type.Missing)
                    parameterTypes[i] = null;
                else
                    parameterTypes[i] = parameters[i].GetType();
            }
            MethodInfo[] methodList = type.GetMethods(BindingFlags.Public | BindingFlags.Static); //parameterTypes);

            for (int i = 0; i < methodList.Length; i++)
            {
                MethodInfo method = methodList[i];
                if (method.Name == name)
                {
                    ParameterInfo[] parameterInfoList = method.GetParameters();
                    if (ValidParameterTypes(parameterTypes, parameterInfoList))
                    {
                        if (method.ContainsGenericParameters)
                            method = method.MakeGenericMethod(parameterInfoList, parameters);
                        return (T)method.Invoke(null, parameters);
                    }
                }
            }

            return default(T);
        }

    }
}
