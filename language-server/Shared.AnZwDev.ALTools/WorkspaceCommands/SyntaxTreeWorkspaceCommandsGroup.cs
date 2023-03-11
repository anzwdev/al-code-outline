using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SyntaxTreeWorkspaceCommandsGroup : SyntaxTreeWorkspaceCommand
    {

        private Dictionary<string, SyntaxTreeWorkspaceCommand> _commands;


        public SyntaxTreeWorkspaceCommandsGroup(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
        {
            this._commands = new Dictionary<string, SyntaxTreeWorkspaceCommand>();
        }

        public SyntaxTreeWorkspaceCommand AddCommand(SyntaxTreeWorkspaceCommand command)
        {
            this._commands.Add(command.Name, command);
            return command;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            string commandsParameterValue = parameters["commandsList"];
            char[] sep = { ',' };
            string[] commandsList = commandsParameterValue.Split(sep);
            for (int i=0;i<commandsList.Length; i++)
            {
                if (this._commands.ContainsKey(commandsList[i]))
                    node = this._commands[commandsList[i]].ProcessSyntaxNode(node, sourceCode, projectPath, filePath, span, parameters);
            }
            return base.ProcessSyntaxNode(node, sourceCode, projectPath, filePath, span, parameters);
        }

    }
}
