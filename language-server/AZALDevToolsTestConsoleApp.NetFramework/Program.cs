﻿using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Server;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.WorkspaceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsTestConsoleApp.NetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            //string extensionPath = "C:\\Users\\azwie\\.vscode\\extensions\\ms-dynamics-smb.al-5.0.329509";
            //string extensionPath = "C:\\Users\\azwie\\Downloads\\VSCode-win32-x64-1.45.1\\data\\extensions\\microsoft.al-0.13.82793";
            string extensionPath = "C:\\Projects\\MicrosoftALVersions\\LatestBC";

            LanguageServerHost host = new LanguageServerHost(extensionPath);

            ALDevToolsServer alDevToolsServer = new ALDevToolsServer(extensionPath);

            //string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\small\\Pag50000.MySmallTableList.al";

            CodeAnalyzersLibrariesCollection caLibCol = new CodeAnalyzersLibrariesCollection(alDevToolsServer);
            CodeAnalyzersLibrary caLib = caLibCol.GetCodeAnalyzersLibrary("${CodeCop}");

            
            string filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18\\Pag50104.MyPrefixMyPageCard.al";
            string content = FileUtils.SafeReadAllText(filePath);
            Dictionary<string, string> pm = new Dictionary<string, string>();
            pm.Add("sourceFilePath", filePath);
            string projectPath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18";
            WorkspaceCommandResult o = host.ALDevToolsServer.WorkspaceCommandsManager.RunCommand("removeWith", content, projectPath, filePath, null, pm, null, null);

            //Workspace tests
            ALProjectSource[] projects =
            {
                //"C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\"
                new ALProjectSource("C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC18", null, null, null)
            };
            

            ALWorkspace workspace = new ALWorkspace();
            workspace.LoadProjects(projects);
            workspace.ResolveDependencies();

            ALProject project = workspace.Projects[0];

            filePath = "C:\\Projects\\Sandboxes\\al-test-projects\\SmallBC16\\MyPage.al";
            ALSymbolInfoSyntaxTreeReader syntaxTreeReader = new ALSymbolInfoSyntaxTreeReader(true);
            ALSymbol symbols = syntaxTreeReader.ProcessSourceFile(filePath, project);

            ALFullSyntaxTree syntaxTree = new ALFullSyntaxTree();
            syntaxTree.Load("", filePath, project);

            PageInformationProvider pageInformationProvider = new PageInformationProvider();
            PageInformation pageInformation = pageInformationProvider.GetPageDetails(project, new ALObjectReference(null, "MyPageCard"), true, true, true, null);

            /*
            XmlPortInformationProvider xmlPortInformationProvider = new XmlPortInformationProvider();
            XmlPortTableElementInformation xmlPortTableElementInformation = xmlPortInformationProvider.GetXmlPortTableElementDetails(project, "wefew", "Item", true, true);
            */

            ReportInformationProvider reportInformationProvider = new ReportInformationProvider();
            ReportDataItemInformation reportDataItemInformation = reportInformationProvider.GetReportDataItemInformationDetails(project, new ALObjectReference(null, "Sales Invoice NA"), "T1", true, true);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
