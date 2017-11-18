namespace KilnTool
{
    namespace Kiln
    {
        public class KilnRepository
        {
            public int ixRepo { get; set; }
            public int vcs { get; set; }
            public bool fHasChangesets { get; set; }
            public string sHgUrl { get; set; }
            public string sGitUrl { get; set; }
            public string sHgSshUrl { get; set; }
            public string sGitSshUrl { get; set; }
            public string sSlug { get; set; }
            public string sGroupSlug { get; set; }
            public string sProjectSlug { get; set; }
            public int bytesSize { get; set; }
            public KilnPerson personCreator { get; set; }
            public KilnRepository[] repoBranches { get; set; }
            public string sStatus { get; set; }
            public string sName { get; set; }
            public string sDescription { get; set; }
            public int ixRepoGroup { get; set; }
            public int? ixParent { get; set; }
            public bool fCentral { get; set; }

            public override string ToString()
            {
                return sName;
            }
        }
    }
}
