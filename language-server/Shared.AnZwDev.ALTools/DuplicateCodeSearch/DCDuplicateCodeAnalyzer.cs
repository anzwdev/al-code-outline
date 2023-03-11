using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicateCodeAnalyzer
    {

        public int MinNoOfStatements { get; }
        public ConvertedObsoleteState SkipObsoleteCodeLevel { get; }

        public DCDuplicateCodeAnalyzer(int minNoOfStatements, ConvertedObsoleteState skipObsoleteCodeLevel)
        {
            if (minNoOfStatements < 2)
                minNoOfStatements = 2;
            this.MinNoOfStatements = minNoOfStatements;
            this.SkipObsoleteCodeLevel = skipObsoleteCodeLevel;
        }

        public List<DCDuplicate> FindDuplicates(ALWorkspace workspace, string path)
        {
            DCStatementKeyDictionaryBuilder statementsBuilder = new DCStatementKeyDictionaryBuilder(SkipObsoleteCodeLevel);

            ALProject singleProject = null;
            if (!String.IsNullOrWhiteSpace(path))
                singleProject = workspace.FindProject(path);

            if (singleProject != null)
                statementsBuilder.VisitProjectFolder(singleProject.RootPath);
            else
                foreach (ALProject project in workspace.Projects)
                    statementsBuilder.VisitProjectFolder(project.RootPath);

            DCDuplicatePairCollection duplicatePairs = new DCDuplicatePairCollection();
            foreach (DCStatementKey statementKey in statementsBuilder.StatementsDictionary.Values)
                if (!statementKey.Ignore)
                    FindDuplicates(duplicatePairs, statementKey);

            DCDuplicateCollection duplicates = new DCDuplicateCollection();
            foreach (DCDuplicatePair pair in duplicatePairs.Values)
                duplicates.Add(pair);

            List<DCDuplicate> duplicateList = duplicates.Values.ToList();
            duplicateList.Sort(new DCDuplicateComparer());
            return duplicateList;
        }

        protected void FindDuplicates(DCDuplicatePairCollection duplicatePairs, DCStatementKey statementKey)
        {
            if (statementKey.StatementInstances.Count < 2)
                return;

            for (int sourceIndex = 0; sourceIndex < (statementKey.StatementInstances.Count - 1); sourceIndex++)
                for (int destIndex = sourceIndex + 1; destIndex < statementKey.StatementInstances.Count; destIndex++)
                    FindDuplicates(duplicatePairs, statementKey.StatementInstances[sourceIndex], statementKey.StatementInstances[destIndex]);
        }

        protected void FindDuplicates(DCDuplicatePairCollection duplicatePairs, DCStatementInstance sourceInstance, DCStatementInstance destInstance)
        {
            int sourceStartIndex = sourceInstance.StatementInstanceIndex;
            int sourceEndIndex = sourceStartIndex;
            int destStartIndex = destInstance.StatementInstanceIndex;
            int destEndIndex = destStartIndex;
            int sourceMaxIndex = sourceInstance.StatementsBlock.Statements.Count - 1;
            int destMaxIndex = destInstance.StatementsBlock.Statements.Count - 1;
            int noOfStatements = 1;

            while (
                (sourceStartIndex > 0) && 
                (destStartIndex > 0) && 
                (sourceInstance.StatementsBlock.Statements[sourceStartIndex - 1].Key.Value == destInstance.StatementsBlock.Statements[destStartIndex - 1].Key.Value))
            {
                sourceStartIndex--;
                destStartIndex--;
                if (!sourceInstance.StatementsBlock.Statements[sourceStartIndex].Key.Ignore)
                    noOfStatements++;
            }

            while (
                (sourceEndIndex < sourceMaxIndex) &&
                (destEndIndex < destMaxIndex) &&
                (sourceInstance.StatementsBlock.Statements[sourceEndIndex + 1].Key.Value == destInstance.StatementsBlock.Statements[destEndIndex + 1].Key.Value))
            {
                sourceEndIndex++;
                destEndIndex++;
                if (!sourceInstance.StatementsBlock.Statements[sourceEndIndex].Key.Ignore)
                    noOfStatements++;
            }

            if (noOfStatements >= this.MinNoOfStatements)
            {
                DCCodeBlockType codeBlockType = DCCodeBlockType.CodeBlock;
                if ((sourceStartIndex == 0) && (sourceEndIndex == (sourceInstance.StatementsBlock.Statements.Count - 1)))
                    codeBlockType = sourceInstance.StatementsBlock.CodeBlockType;

                duplicatePairs.Add(
                        new DocumentRange(
                            sourceInstance.StatementsBlock.SourceFilePath,
                            sourceInstance.StatementsBlock.Statements[sourceStartIndex].Range.start,
                            sourceInstance.StatementsBlock.Statements[sourceEndIndex].Range.end),
                        new DocumentRange(
                            destInstance.StatementsBlock.SourceFilePath,
                            destInstance.StatementsBlock.Statements[destStartIndex].Range.start,
                            destInstance.StatementsBlock.Statements[destEndIndex].Range.end),
                        noOfStatements,
                        codeBlockType);
            }
        }


    }
}
