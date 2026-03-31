namespace AnZwDev.AL.Workspaces.Documents
{
    public class WorkspaceDocument
    {

        public Workspace Workspace { get; }
        public int Uid { get; internal set; } = 0;

        public WorkspaceDocument(Workspace workspace)
        {
            Workspace = workspace;
        }

    }
}
