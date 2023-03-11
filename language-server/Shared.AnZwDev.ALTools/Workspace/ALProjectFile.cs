using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectFile
    {

        #region Public properties

        public ALProject Project { get; set; }
        public string RelativePath { get; private set; }

        public string FullPath
        {
            get { return (this.Project != null) ? Path.Combine(this.Project.RootPath, this.RelativePath) : this.RelativePath; }
        }

        private List<ALAppObject> _symbols;
        public List<ALAppObject> Symbols
        {
            get { return _symbols; }
            private set
            {
                if (_symbols != value)
                {
                    if (_symbols != null)
                        this.Project.Symbols.RemoveObjects(_symbols);
                    _symbols = value;
                    if (_symbols != null)
                        this.Project.Symbols.AddObjects(_symbols);
                }
            }
        }

        public List<ALAppDirective> Directives { get; private set; }

        private bool _isDirty;
        public bool IsDirty 
        { 
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (!_isDirty)
                    _syntaxTree = null;
            }
        }

        #endregion

        protected SyntaxTree _syntaxTree;

        #region Initialization

        public ALProjectFile() : this(null, null)
        {
        }

        public ALProjectFile(ALProject project, string relativePath)
        {
            this.Project = project;
            this.RelativePath = relativePath;
            this.IsDirty = false;
            _syntaxTree = null;
        }

        #endregion

        #region File content

        public string ReadAllText()
        {
            return FileUtils.SafeReadAllText(this.FullPath);
        }

        public void WriteAllText(string content)
        {
            File.WriteAllText(this.FullPath, content);
        }

        #endregion

        #region Symbols compilation

        public void CompileSymbolReferences(bool cleanDirtyState)
        {
            if ((this.IsDirty) && (_syntaxTree != null))
            {
                (this.Symbols, this.Directives) = this.Project.Workspace.SymbolReferenceCompiler.CreateObjectsAndDirectivesList(this.FullPath, _syntaxTree);
                if (cleanDirtyState)
                    this.IsDirty = false;
            }
            else
                this.CompileSymbolReferences(this.ReadAllText());
        }

        public void CompileSymbolReferences(string source)
        {
            (this.Symbols, this.Directives) = this.Project.Workspace.SymbolReferenceCompiler.CreateObjectsAndDirectivesList(this.FullPath, source);
            this.IsDirty = false;
        }

        #endregion

        public void OnAdd()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences(true);
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences(true);
        }

        public ALSymbol OnChange(string content, bool returnSymbols)
        {
            this.IsDirty = true;
            if (content == null)
                content = this.ReadAllText();
            _syntaxTree = SyntaxTree.ParseObjectText(content);
            this.CompileSymbolReferences(false);

            if (returnSymbols)
                return this.CreateSymbols();

            return null;
        }

        public ALSymbol CreateSymbols()
        {
            if (this._syntaxTree != null)
            {
                try
                {
                    ALSymbolInfoSyntaxTreeReader symbolTreeBuilder = new ALSymbolInfoSyntaxTreeReader(true);
                    return symbolTreeBuilder.ProcessSyntaxTree(_syntaxTree);
                }
                catch (Exception e)
                {
                    MessageLog.LogError(e);

                    return new ALSymbol()
                    {
                        fullName = e.Message
                    };
                }
            }
            return null;
        }

        public void OnSave()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences(true);
        }

        public void OnDelete()
        {
            this.IsDirty = false;
            this.Symbols = null;
            this.Directives = null;
        }

        public void OnRename(string newRelativePath)
        {
            this.RelativePath = newRelativePath;
        }

    }
}
