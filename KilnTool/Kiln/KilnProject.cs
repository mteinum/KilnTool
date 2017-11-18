namespace KilnTool
{
    namespace Kiln
    {
        public class KilnProject
        {
            public int ixProject { get; set; }
            public string sSlug { get; set; }
            public KilnRepositoryGroup[] repoGroups { get; set; }
            public string sName { get; set; }
            public string sDescription { get; set; }
            public string permissionDefault { get; set; }

            public override string ToString()
            {
                return sName;
            }
        }
    }
}
