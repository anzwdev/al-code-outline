using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class NavAppManifestNavxPackageConverter
    {

        public static NavxPackage CreateNavxPackageManifest(Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppManifest navAppManifest)
        {
            NavxPackage package = new NavxPackage();
            package.App = NavAppManifestNavxPackageConverter.CreateNavxApp(navAppManifest);
#if BC
            package.ResourceExposurePolicy = NavAppManifestNavxPackageConverter.CreateNavxResourceExposurePolicy(navAppManifest.ResourceExposurePolicy);
#endif
            List<NavxDependency> dependencies = new List<NavxDependency>();
            foreach (Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppDependency navAppDependency in navAppManifest.Dependencies)
            {
                dependencies.Add(NavAppManifestNavxPackageConverter.CreateNavxDependency(navAppDependency));
            }
            package.Dependencies = dependencies.ToArray();
            return package;
        }

        private static Core.VersionNumber CreateVersion(System.Version version)
        {
            if (version == null)
                return new Core.VersionNumber();
            return new Core.VersionNumber(version.Major, version.Minor, version.Build, version.Revision);       
        }

        private static NavxApp CreateNavxApp(Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppManifest navAppManifest)
        {
            NavxApp app = new NavxApp();
            app.Id = navAppManifest.AppId.ToString("D").ToLower();
            app.Name = navAppManifest.AppName;
            app.Publisher = navAppManifest.AppPublisher;
            app.Version = NavAppManifestNavxPackageConverter.CreateVersion(navAppManifest.AppVersion);
            app.CompatibilityId = NavAppManifestNavxPackageConverter.CreateVersion(navAppManifest.AppCompatibilityId).Version;
            app.Target = navAppManifest.Target.ToString();
            app.Runtime = NavAppManifestNavxPackageConverter.CreateVersion(navAppManifest.Runtime).GetVersionText(2);
            app.ShowMyCode = navAppManifest.ShowMyCode;

#if BC
            app.PropagateDependencies = navAppManifest.PropagateDependencies;
#endif

            return app;
        }

#if BC
        private static NavxResourceExposurePolicy CreateNavxResourceExposurePolicy(Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.ResourceExposurePolicy resourceExposurePolicy)
        {
            if (resourceExposurePolicy == null)
                return null;
            return new NavxResourceExposurePolicy()
            {
                AllowDebugging = resourceExposurePolicy.AllowDebugging,
                AllowDownloadingSource = resourceExposurePolicy.AllowDownloadingSource,
                IncludeSourceInSymbolFile = resourceExposurePolicy.IncludeSourceInSymbolFile
            };
        }
#endif

        private static NavxDependency CreateNavxDependency(Microsoft.Dynamics.Nav.CodeAnalysis.Packaging.NavAppDependency navAppDependency)
        {
            NavxDependency dependency = new NavxDependency();
            dependency.Id = navAppDependency.AppId.ToString("D").ToLower();
            dependency.Name = navAppDependency.Name;
            dependency.Publisher = navAppDependency.Publisher;
            if (navAppDependency.CompatibilityId != null)
                dependency.CompatibilityId = NavAppManifestNavxPackageConverter.CreateVersion(navAppDependency.CompatibilityId).Version;
            if (navAppDependency.MinVersion != null)
                dependency.MinVersion = NavAppManifestNavxPackageConverter.CreateVersion(navAppDependency.MinVersion).Version;
            return dependency;
        }

    }
}
