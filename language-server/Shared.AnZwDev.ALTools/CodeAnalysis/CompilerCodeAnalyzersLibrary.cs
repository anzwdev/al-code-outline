using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using AnZwDev.ALTools.Logging;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class CompilerCodeAnalyzersLibrary : CodeAnalyzersLibrary
    {

        public CompilerCodeAnalyzersLibrary(string name) : base(name)
        {
        }

        public override void Load()
        {
            try
            {
                //create error enum
                Type diagnosticDescriptorType = typeof(DiagnosticDescriptor);
                Type errorCodeType = diagnosticDescriptorType.Assembly.GetType("Microsoft.Dynamics.Nav.CodeAnalysis.ErrorCode");
                Array errorCodesList = Enum.GetValues(errorCodeType);

                //get NavDiagnosticInfo type
                Type navDiagnosticInfoType = diagnosticDescriptorType.Assembly.GetType("Microsoft.Dynamics.Nav.CodeAnalysis.NavDiagnosticInfo");
                Type[] constructorParametersTypes = { errorCodeType };
                ConstructorInfo constructor = navDiagnosticInfoType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, constructorParametersTypes, null);

                //this.GetNavDiagnosticInfoConstructor(navDiagnosticInfoType);

                PropertyInfo diagnosticDescriptorProperty = navDiagnosticInfoType.GetProperty("Descriptor");

                object[] constructorParameters = new object[1];

                //create rules for each enum value
                for (int i = 0; i < errorCodesList.Length; i++)
                {
                    constructorParameters[0] = errorCodesList.GetValue(i);
                    string errorCodeName = constructorParameters[0].ToString();
                    object navDiagnosticInfo = constructor.Invoke(constructorParameters);
                    DiagnosticDescriptor diagnosticDescriptor = diagnosticDescriptorProperty.GetValue(navDiagnosticInfo) as DiagnosticDescriptor;
                    if (diagnosticDescriptor != null)
                    {
                        CodeAnalyzerRule rule = new CodeAnalyzerRule(diagnosticDescriptor);
                        if (String.IsNullOrWhiteSpace(rule.title))
                            rule.title = errorCodeName;
                        else
                            rule.title = errorCodeName + " - " + rule.title;
                        this.Rules.Add(rule);
                    }
                }
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
            }
        }

    }
}
