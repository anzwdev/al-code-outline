using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;

namespace AnZwDev.AL.Symbols.Tests.SingleReferences
{
    public class TableLoadTests
    {

        [Theory]
        [InlineData("No namespaces BC13", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC13)]
        [InlineData("No namespaces BC18", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC18)]
        [InlineData("No namespaces BC20", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC20)]
        [InlineData("Namespaces", TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        public void AppFileGetTableFields(string description, string projectPath)
        {
            var description2 = description; // to avoid warning
            var symbols = ProjectLoaderHelpers.LoadFromAppFile(projectPath, false, null);
            TestGetTableFields(symbols);
        }

        [Theory]
        [InlineData("No namespaces BC13", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC13)]
        [InlineData("No namespaces BC18", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC18)]
        [InlineData("No namespaces BC20", TestALProjectsPaths.NoInterfacesSymbolsLoadProject01BC20)]
        [InlineData("Namespaces", TestALProjectsPaths.InterfacesSymbolsLoadProject01)]
        public void ProjectBuilsGetTableFields(string description, string projectPath)
        {
            var description2 = description; // to avoid warning
            var symbols = ProjectLoaderHelpers.LoadFromProjectBuild(projectPath, false);
            TestGetTableFields(symbols);
        }

        private void TestGetTableFields(ApplicationSymbol? symbols)
        {
            Assert.NotNull(symbols);

            var fullyQualifiedName = ALSymbolExpressionParser.ParseFullyQualifiedName("\"Test Table\"");

            var table = symbols.Tables.FindFirst(new ObjectReference(ObjectKind.Table, null, fullyQualifiedName, null));
            Assert.True(table != null, "Table not found");

            var fields = table.Fields;
            Assert.True(fields != null, "Fields list empty");
            Assert.True(fields.Count == 4, "Incorrect number of fields.");

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                Assert.NotNull(field);

                switch (field.Id)
                {
                    case 1:
                        Assert.Equal("IntField", field.Name);
                        //Assert.Equal("Integer"
                        break;
                    case 2:
                        Assert.Equal("TextField", field.Name);
                        //Assert.Equal("Text[50]"
                        break;
                    case 3:
                        Assert.Equal("TextField 2", field.Name);
                        //Assert.Equal("Text[50]"
                        break;
                    case 4:
                        Assert.Equal("Date Field", field.Name);
                        //Assert.Equal("Date"
                        break;
                    default:
                        Assert.Fail("Unexpected field id" + field.Id.ToString());
                        break;
                }
            }
        }

    }
}
