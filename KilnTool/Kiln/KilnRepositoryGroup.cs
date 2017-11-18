namespace KilnTool
{
    namespace Kiln
    {
        public class KilnRepositoryGroup
        {
            public int ixRepoGroup { get; set; }
            public string sName { get; set; }
            public string sSlug { get; set; }
            public int ixProject { get; set; }
            public int nOrder { get; set; }
            public KilnRepository[] repos { get; set; }

            public override string ToString()
            {
                return sName;
            }
        }
    }
}
