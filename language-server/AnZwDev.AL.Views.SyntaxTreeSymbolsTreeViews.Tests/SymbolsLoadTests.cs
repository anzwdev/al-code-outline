using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.Tests
{
    public class SymbolsLoadTests
    {
        [Fact]
        public void LoadSymbols()
        {
            var content = File.ReadAllText("C:\\Projects\\InProg\\AnZwDev.AL.DevelopmentTools.TestProjects\\Project01-BC20\\src\\table\\TestTable.Table.al");
            var syntaxTree = SyntaxTree.ParseObjectText(content);

            var builder = new SyntaxTreeSymbolsTreeViewBuilder();
            var node = builder.CreateView(syntaxTree);


        }
    }
}
