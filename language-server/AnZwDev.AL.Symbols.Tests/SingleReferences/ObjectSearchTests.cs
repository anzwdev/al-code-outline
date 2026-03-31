using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.AL.Syntax;
using System.IO.Compression;

namespace AnZwDev.AL.Symbols.Tests.SingleReferences
{
    public class ObjectSearchTests
    {

        [Theory]
        [InlineData("No namespaces BC13", null, null, true, TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC13)]
        [InlineData("No namespaces BC18", null, null, true, TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC18)]
        [InlineData("No namespaces BC20", null, null, true, TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC20)]
        [InlineData("Namespaces, no ns in params", null, null, true, TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        [InlineData("Namespaces, use ns", "ALProject02.NS1", null, true, TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        [InlineData("Namespaces, use usings", null, "ALProject02.NS1", true, TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        [InlineData("Namespaces, use incorrect ns", "ALProject02.NS2", null, false, TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        [InlineData("Namespaces, use incorrect using", null, "ALProject02.NS2", false, TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        public void TestAppFileObjectSearch(string description, string? directns, string? usingns, bool checkIfObjectExists, string path)
        {
            var description2 = description; // to avoid warning
            var symbols = ProjectLoaderHelpers.LoadFromAppFile(path, false, null);

            IntTestObjectsSearch(symbols, description, directns, usingns, checkIfObjectExists);
        }

        private void IntTestObjectsSearch(ApplicationSymbol? symbols, string description, string? directns, string? usingns, bool checkIfObjectExists)
        {
            Assert.NotNull(symbols);

            CheckObjectExists(symbols.Codeunits, ObjectKind.Codeunit, directns, usingns, "\"Variables Tests\"", checkIfObjectExists);
            CheckObjectExists(symbols.Tables, ObjectKind.Table, directns, usingns, "\"Test Table\"", checkIfObjectExists);
            CheckObjectExists(symbols.ControlAddIns, ObjectKind.ControlAddIn, directns, usingns, "ControlAddInTests", checkIfObjectExists);
            CheckObjectExists(symbols.EnumTypes, ObjectKind.EnumType, directns, usingns, "EnumTests", checkIfObjectExists);
            CheckObjectExists(symbols.EnumExtensionTypes, ObjectKind.EnumExtensionType, directns, usingns, "EnumExtensionTests", checkIfObjectExists);
            CheckObjectExists(symbols.Interfaces, ObjectKind.Interface, directns, usingns, "InterfaceTests", checkIfObjectExists);
            CheckObjectExists(symbols.Pages, ObjectKind.Page, directns, usingns, "PageControlsNewActionsTests", checkIfObjectExists);
            CheckObjectExists(symbols.PageCustomizations, ObjectKind.PageCustomization, directns, usingns, "CustomizationTests", checkIfObjectExists);
            CheckObjectExists(symbols.PageExtensions, ObjectKind.PageExtension, directns, usingns, "PageExtensionControlsTests", checkIfObjectExists);
            CheckObjectExists(symbols.PermissionSets, ObjectKind.PermissionSet, directns, usingns, "PermissionSetTest", checkIfObjectExists);
            //CheckObjectExists(symbols.Profiles, ObjectKind.Profile, "ProfileTests");
            CheckObjectExists(symbols.Queries, ObjectKind.Query, directns, usingns, "QueryTests", checkIfObjectExists);
            CheckObjectExists(symbols.Reports, ObjectKind.Report, directns, usingns, "ReportTests", checkIfObjectExists);
            CheckObjectExists(symbols.ReportExtensions, ObjectKind.ReportExtension, directns, usingns, "ReportExtensionTests", checkIfObjectExists);
            CheckObjectExists(symbols.TableExtensions, ObjectKind.TableExtension, directns, usingns, "TableExtensionTests", checkIfObjectExists);
            CheckObjectExists(symbols.ProfileExtensions, ObjectKind.ProfileExtension, directns, usingns, "ProfileExtTests", checkIfObjectExists);
            CheckObjectExists(symbols.XmlPorts, ObjectKind.XmlPort, directns, usingns, "XmlPortTests", checkIfObjectExists);
        }

        private void CheckObjectExists<T>(ObjectSymbolsCollection<T> objectsList, ObjectKind objectKind, string? directns, string? usingns, string objectName, bool checkObjectExists = true) where T : ObjectSymbol
        {
            HashSet<string>? usings = null;

            if (!string.IsNullOrWhiteSpace(directns))
                objectName = directns + "." + objectName;
            
            if (!string.IsNullOrWhiteSpace(usingns))
                usings = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { usingns };

            var fullyQualifiedName = ALSymbolExpressionParser.ParseFullyQualifiedName(objectName);

            var obj = objectsList.FindFirst(new ObjectReference(objectKind, null, fullyQualifiedName, usings));
            if (checkObjectExists)
                Assert.True(obj != null, $"{objectKind} {objectName} not found.");
            else
                Assert.True(obj == null, $"{objectKind} {objectName} should not be found.");
        }

    }
}