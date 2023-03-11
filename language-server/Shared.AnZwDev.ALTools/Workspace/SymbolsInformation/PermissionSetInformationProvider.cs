using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedPermissions;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PermissionSetInformationProvider
    {

        public List<PermissionSetInformation> GetPermissionSets(ALProject project, bool includeNonAccessible)
        {
            List<PermissionSetInformation> infoList = new List<PermissionSetInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddPermissionSets(infoList, dependency.Symbols, includeNonAccessible || dependency.InternalsVisible);
            }
            if (project.Symbols != null)
                AddPermissionSets(infoList, project.Symbols, true);
            return infoList;
        }

        private void AddPermissionSets(List<PermissionSetInformation> infoList, ALAppSymbolReference symbols, bool includeInternal)
        {
            if (symbols.PermissionSets != null)
            {
                for (int i = 0; i < symbols.PermissionSets.Count; i++)
                {
                    if ((includeInternal) || (symbols.PermissionSets[i].GetAccessMode() != ALAppAccessMode.Internal))
                        infoList.Add(new PermissionSetInformation(symbols.PermissionSets[i]));
                }
            }
        }

        public MergedALAppPermissionSet GetMergedPermissionSet(ALProject project, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection = new Dictionary<string, MergedALAppPermissionSet>();
            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, includedPermissionSetsNames, excludedPermissionSetsNames);
        }

        public MergedALAppPermissionSet GetMergedPermissionSet(ALProject project, string permissionSetName)
        {
            Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection = new Dictionary<string, MergedALAppPermissionSet>();
            return GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, permissionSetName);
        }

        private MergedALAppPermissionSet GetOrCreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, string permissionSetName)
        {
            permissionSetName = permissionSetName.ToLower();
            if (mergedPermissionSetsCollection.ContainsKey(permissionSetName))
                return mergedPermissionSetsCollection[permissionSetName];

            var mergedPermissionSet = CreateMergedPermissionSet(project, mergedPermissionSetsCollection, permissionSetName);
            if (mergedPermissionSet != null)
                mergedPermissionSetsCollection.Add(permissionSetName, mergedPermissionSet);
            return mergedPermissionSet;
        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            MergedALAppPermissionSet mergedPermissionSet = new MergedALAppPermissionSet();
            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, mergedPermissionSet,
                "", includedPermissionSetsNames, excludedPermissionSetsNames);

        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, string permissionSetName)
        {
            var permissionSet = project.AllSymbols.PermissionSets.FindObject(permissionSetName);
            if (permissionSet == null)
                return null;

            MergedALAppPermissionSet mergedPermissionSet = new MergedALAppPermissionSet(permissionSet);

            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, mergedPermissionSet,
                permissionSetName,
                permissionSet.Properties?.GetProperty("IncludedPermissionSets")?.Value,
                permissionSet.Properties?.GetProperty("ExcludedPermissionSets")?.Value);
        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, 
            MergedALAppPermissionSet mergedPermissionSet, string permissionSetName, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        { 
            HashSet<string> includedPermissionSets = new HashSet<string>();
            HashSet<string> excludedPermissionSets = new HashSet<string>();

            includedPermissionSets.AddRange(ALSyntaxHelper.DecodeNamesList(includedPermissionSetsNames));
            excludedPermissionSets.AddRange(ALSyntaxHelper.DecodeNamesList(excludedPermissionSetsNames));

            if (!String.IsNullOrWhiteSpace(permissionSetName))
            {
                var permissionSetExtensionsCollection = project.AllSymbols.PermissionSetExtensions.FindAllExtensions(permissionSetName);
                if (permissionSetExtensionsCollection != null)
                    foreach (var permissionSetExtension in permissionSetExtensionsCollection)
                    {
                        mergedPermissionSet.IncludedPermissions.AddRange(permissionSetExtension.Permissions);

                        includedPermissionSets.AddRange(
                            ALSyntaxHelper.DecodeNamesList(
                                permissionSetExtension.Properties?.GetProperty("IncludedPermissionSets")?.Value));

                        excludedPermissionSets.AddRange(
                            ALSyntaxHelper.DecodeNamesList(
                                permissionSetExtension.Properties?.GetProperty("ExcludedPermissionSets")?.Value));
                    }
            }

            foreach (var includedPermissionSetName in includedPermissionSets)
            {
                var includedMergedPermissionSet = GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, includedPermissionSetName);
                if (includedMergedPermissionSet != null)
                    mergedPermissionSet.IncludedPermissions.AddRange(includedMergedPermissionSet.EffectivePermissions.GetAllPermissions());
            }

            foreach (var excludedPermissionSetName in excludedPermissionSets)
            {
                var excludedMergedPermissionSet = GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, excludedPermissionSetName);
                if (excludedMergedPermissionSet != null)
                    mergedPermissionSet.ExcludedPermissions.AddRange(excludedMergedPermissionSet.EffectivePermissions.GetAllPermissions());
            }

            mergedPermissionSet.UpdateEffectivePermissions();

            return mergedPermissionSet;
        }

    }
}
