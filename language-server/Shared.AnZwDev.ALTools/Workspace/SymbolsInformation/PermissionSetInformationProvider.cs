using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedPermissions;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{

    public class PermissionSetInformationProvider : BaseExtendableObjectInformationProvider<ALAppPermissionSet, ALAppPermissionSetExtension>
    {

        public PermissionSetInformationProvider() : base(x => x.PermissionSets, x => x.PermissionSetExtensions)
        {
        }

        public List<PermissionSetInformation> GetPermissionSets(ALProject project, bool includeNonAccessible)
        {
            var infoList = new List<PermissionSetInformation>();
            var objectsCollection = GetALAppObjectsCollection(project);
            var objectsEnumerable = objectsCollection.GetAll(includeNonAccessible);
            foreach (var obj in objectsEnumerable)
                infoList.Add(new PermissionSetInformation(obj));
            return infoList;
        }

        public MergedALAppPermissionSet GetMergedPermissionSet(ALProject project, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection = new Dictionary<string, MergedALAppPermissionSet>();
            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, includedPermissionSetsNames, excludedPermissionSetsNames);
        }

        /*
        public MergedALAppPermissionSet GetMergedPermissionSet(ALProject project, string permissionSetName)
        {
            Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection = new Dictionary<string, MergedALAppPermissionSet>();
            return GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, permissionSetName);
        }
        */

        private MergedALAppPermissionSet GetOrCreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, ALObjectReference permissionSetName)
        {
            var permissionSetFullNameKey = permissionSetName.GetFullName().ToLower();
            if (mergedPermissionSetsCollection.ContainsKey(permissionSetFullNameKey))
                return mergedPermissionSetsCollection[permissionSetFullNameKey];

            var mergedPermissionSet = CreateMergedPermissionSet(project, mergedPermissionSetsCollection, permissionSetName);
            
            if (mergedPermissionSet != null)
                mergedPermissionSetsCollection.Add(permissionSetFullNameKey, mergedPermissionSet);
            return mergedPermissionSet;
        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, string includedPermissionSetsNames, string excludedPermissionSetsNames)
        {
            MergedALAppPermissionSet mergedPermissionSet = new MergedALAppPermissionSet();
            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, mergedPermissionSet,
                null, includedPermissionSetsNames, excludedPermissionSetsNames);
        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, ALObjectReference permissionSetName)
        {
            var permissionSet = project
                .SymbolsWithDependencies
                .PermissionSets
                .FindFirst(permissionSetName.NamespaceName, permissionSetName.Name);
            if (permissionSet == null)
                return null;

            MergedALAppPermissionSet mergedPermissionSet = new MergedALAppPermissionSet(permissionSet);

            return CreateMergedPermissionSet(project, mergedPermissionSetsCollection, mergedPermissionSet,
                permissionSet,
                permissionSet.Properties?.GetProperty("IncludedPermissionSets")?.Value,
                permissionSet.Properties?.GetProperty("ExcludedPermissionSets")?.Value);
        }

        private MergedALAppPermissionSet CreateMergedPermissionSet(
            ALProject project, Dictionary<string, MergedALAppPermissionSet> mergedPermissionSetsCollection, 
            MergedALAppPermissionSet mergedPermissionSet, ALAppPermissionSet permissionSet,
            string includedPermissionSetsNames, string excludedPermissionSetsNames)
        { 
            HashSet<string> includedPermissionSets = new HashSet<string>();
            HashSet<string> excludedPermissionSets = new HashSet<string>();

            includedPermissionSets.AddRange(ALSyntaxHelper.SplitNamesList(includedPermissionSetsNames));
            excludedPermissionSets.AddRange(ALSyntaxHelper.SplitNamesList(excludedPermissionSetsNames));

            if (permissionSet != null)
            {

                var permissionSetExtensionsCollection = GetALAppObjectExtensionsCollection(project, permissionSet);
                foreach (var permissionSetExtension in permissionSetExtensionsCollection)
                {
                    mergedPermissionSet.IncludedPermissions.AddRange(permissionSetExtension.Permissions);

                    includedPermissionSets.AddRange(
                        ALSyntaxHelper.SplitNamesList(
                            permissionSetExtension.Properties?.GetProperty("IncludedPermissionSets")?.Value));

                    excludedPermissionSets.AddRange(
                        ALSyntaxHelper.SplitNamesList(
                            permissionSetExtension.Properties?.GetProperty("ExcludedPermissionSets")?.Value));
                }
            }
            
            foreach (var includedPermissionSetName in includedPermissionSets)
            {
                var objectReference = new ALObjectReference(permissionSet?.Usings, includedPermissionSetName);
                var includedMergedPermissionSet = GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, objectReference);
                if (includedMergedPermissionSet != null)
                    mergedPermissionSet.IncludedPermissions.AddRange(includedMergedPermissionSet.EffectivePermissions.GetAllPermissions());
            }

            foreach (var excludedPermissionSetName in excludedPermissionSets)
            {
                var objectReference = new ALObjectReference(permissionSet?.Usings, excludedPermissionSetName);
                var excludedMergedPermissionSet = GetOrCreateMergedPermissionSet(project, mergedPermissionSetsCollection, objectReference);
                if (excludedMergedPermissionSet != null)
                    mergedPermissionSet.ExcludedPermissions.AddRange(excludedMergedPermissionSet.EffectivePermissions.GetAllPermissions());
            }

            mergedPermissionSet.UpdateEffectivePermissions();

            return mergedPermissionSet;
        }

    }
}
