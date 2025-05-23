﻿using AnZwDev.ALTools;
using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.CodeTransformations.Namespaces;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.DuplicateCodeSearch;
using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using System.Runtime.Serialization;
using System.Data;

namespace AZALDevToolsTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string extensionPath = "C:\\Projects\\MicrosoftALVersions\\LatestBC";

            LanguageServerHost host = new(extensionPath);

            ALDevToolsServer alDevToolsServer = new(extensionPath);

            //get list of errors and warnings
            //CompilerCodeAnalyzersLibrary lib = new CompilerCodeAnalyzersLibrary("Compiler");
            CodeAnalyzersLibrary lib = alDevToolsServer.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary("Compiler");

            ImageInformationProvider provider = new();
            List<ImageInformation> images = provider.GetActionImages();

            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";
            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50100.MyPageEvS.al";
            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\PTEMagic.Codeunit.al";






            //test project
            ALProjectSource[] projects =
            {
                //new ALProjectSource("C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject", null, null, null)
                new ALProjectSource("C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23", null, null, null)
                //"C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18"                
            };
            host.ALDevToolsServer.Workspace.LoadProjects(projects);
            host.ALDevToolsServer.Workspace.ResolveDependencies();

            DCDuplicateCodeAnalyzer duplicateAnalyzer = new DCDuplicateCodeAnalyzer(3, AnZwDev.ALTools.ALSymbols.Internal.ConvertedObsoleteState.None);
            var duplicatesList = duplicateAnalyzer.FindDuplicates(host.ALDevToolsServer.Workspace, null);



            var libraryPath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\.alpackages\\Microsoft_Base Application_23.0.12034.12747.app";
            ALAppSymbolReference symbolReference = host.ALDevToolsServer.Workspace.SymbolReferencesCache.GetSymbolReference(libraryPath);
            ALSymbolsLibrary library;
            if (symbolReference != null)
                library = symbolReference.ToALSymbolsLibrary();
            else
                library = new ALSymbolsLibrary();

            //ALPackageSymbolsLibrary library = this.Server.AppPackagesCache.GetSymbols(parameters.path, false);
            if (library != null)
            {
                var libraryId = host.ALDevToolsServer.SymbolsLibraries.AddLibrary(library);
                var librarySymbols = library.GetObjectsTree(); // .Root;
            }


            /*
            Helpers.CopyFolder("C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23.Backup", "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23");
            AddProjectNamespacesConverter projectNamespacesConverter = new AddProjectNamespacesConverter();
            projectNamespacesConverter.AddNamespacesToProject(
                host.ALDevToolsServer.Workspace,
                host.ALDevToolsServer.Workspace.Projects[0],
                "AN.Demo",
                true);
            */

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\Pag50104.MyPrefixMyPageCard.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\permissionset-Ext50101.MyPermSetExt03.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50101.asUFDGHQEUGF.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\net.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\ObjectIdTestPage.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\FunctionsTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50103.MyTestPage2.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Rep50100.MyReport.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\MyTestCU.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Cod50104.IdentifiersTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Rep50100.MyReport.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\MyTable.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\PageEmptySectionsTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\WithTestsCU.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\CUWithTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\CSVXmlPort.al";

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Pag50105.MyItem3.al";

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Tab50100.T1.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Cod50100.MyNewcodeunit.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Cod50112.rgrg.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\Cod50105.UnusedVariablesTest.al";

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\BC184TestProject\\StatementsTest.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\PTEMagic.Codeunit.al";

            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\SortProceduresTests.Codeunit.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\InterfaceDemo.Interface.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\DemoPage.Page.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\SortingTestPage.Page.al";
            //filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\MyTestPage.Page.al";
            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23\\CustomerLabel.Table.al";

            string content = FileUtils.SafeReadAllText(filePath);
            Dictionary<string, string> pm = new();
            //pm.Add("sourceFilePath", filePath);
            //pm.Add("skipFormatting", "true");
            //pm.Add("sortMode", "mainTypeNameOnly");

            pm.Add("removeGlobalVariables", "true");
            pm.Add("removeLocalVariables", "true");
            pm.Add("removeLocalMethodParameters", "true");

            pm.Add("triggersSortMode", "NaturalOrder");
            pm.Add("globalVariablesSortMode", "First");
            pm.Add("triggersNaturalOrder", "[]");

            pm.Add("includeInterfaces", "true");

            pm.Add("reuseToolTips", "true");

            //pm.Add("triggersSortMode", "NaturalOrder");
            pm.Add("sortSingleNodeRegions", "true");
            //pm.Add("triggersNaturalOrder", "");

            ALProject project = host.ALDevToolsServer.Workspace.Projects[0];

            //Helpers.CopyFolder("C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23.Backup", "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC23");
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new(true);
            ALSymbol symbols = syntaxTreeReader.ProcessSourceFile(filePath, project);

            ALProjectSymbolsLibrarySource symbolsSource = new ALProjectSymbolsLibrarySource(project);
            ALSymbol symbol = new ALSymbol(ALSymbolKind.TableObject, "Data Privacy Entities");
            var location = symbolsSource.GetSymbolSourceProjectLocation(symbol);


            //pm.Add("dependencyName0", "Microsoft - System");
            //pm.Add("dependencyName1", "Microsoft - Application");
            //pm.Add("dependencyName2", "Microsoft - System Application");
            //pm.Add("dependencyName3", "Microsoft - Base Application");

            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeWith", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addAllObjectsPermissions", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("fixIdentifiersCase", content, projects[0].folderPath, filePath, null, pm, null);

            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeUnusedVariables", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("convertObjectIdsToNames", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addParentheses", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortVariables", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeBeginEnd", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addToolTips", content, projects[0].folderPath, filePath, null, pm, null, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("refreshToolTips", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortProperties", content, projects[0], filePath, null, pm);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeEmptyLines", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeEmptySections", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeStrSubstNoFromError", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeRedundantAppAreas", content, projects[0].folderPath, filePath, null, pm, null, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addTableDataCaptionFields", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addDropDownFieldGroups", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addReferencedTablesPermissions", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("generateCSVXmlPortHeaders", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeRedundantDataClassification", content, projects[0].folderPath, filePath, null, pm, null);
            WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("sortProcedures", content, projects[0].folderPath, filePath, null, pm, null, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("fixIdentifiersCase", content, projects[0].folderPath, filePath, null, pm, null, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("oneStatementPerLine", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addMissingCaseLines", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addTooTipsEndingDots", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeProceduresSemicolon", content, projects[0].folderPath, filePath, null, pm, null);
            //WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("addUsingRegion", content, projects[0].folderPath, filePath, null, pm, null);

            TableInformationProvider tableInformationProvider = new();
            List<TableFieldInformaton> fields = tableInformationProvider.GetTableFields(project, new ALObjectReference(null, "Item"), false, false, true, true, true, false, null);
            List<TableFieldInformaton> fields2 = fields.Where(p => (p.Name.StartsWith("Description"))).ToList();


            var allObj = project
                .SymbolsWithDependencies
                .Tables;

            var reportSymbolReference = new SymbolReference()
            {
                id = 50300,
                name = "MyReport"
            };

            ReportInformationProvider reportProvider = new ReportInformationProvider();
            var reportData = reportProvider.GetReportDataItemInformationDetails(
                project,
                reportSymbolReference.ToALObjectReference(),
                "CustLedgerEntry", false, true);



            PageInformationProvider pageInformationProvider = new PageInformationProvider();
            var pageSymbolReference = new SymbolReference()
            {
                //id = 50305,
                //name = "NotWorkingTickets 2TNP"
                name = "Posted Sales Credit Memo"
            };
            var pageFields = pageInformationProvider.GetPageDetails(
                project,
                pageSymbolReference.ToALObjectReference(),
                false, true, false, null);

            var pageReference = new ALObjectReference(null, "MyTestPage");
            var page = project
                .SymbolsWithDependencies
                .Pages
                .FindFirst(pageReference);

            if (page != null)
            {
                var toolTipsList = pageInformationProvider.GetPageFieldAvailableToolTips(project, "Page", page.GetIdentifier(), new ALObjectReference(), "Rec.\"No.\"");
                PageInformation pageInformation = pageInformationProvider.GetPageDetails(project, new ALObjectReference(null, "wqefewf"), true, true, true, null);
            }
            ObjectIdInformationProvider objectIdInformationProvider = new();
            long id = objectIdInformationProvider.GetNextObjectId(project, "Page");

            ReportInformationProvider reportInformationProvider = new();
            ReportInformation reportInformation = reportInformationProvider.GetFullReportInformation(project, new ALObjectReference(null, "Sales Order"));


            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
