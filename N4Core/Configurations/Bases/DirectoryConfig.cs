namespace N4Core.Configurations.Bases
{
    public abstract class DirectoryConfig
    {
        public List<string> Directories { get; set; } = new List<string>() { "files" };

        public string Path
        {
            get
            {
                if (Directories.Any())
                {
                    List<string> path = Directories.ToList();
                    path.Insert(0, "wwwroot");
                    return System.IO.Path.Combine(path.ToArray());
                }
                return "";
            }
        }

        public void SetDirectories(params string[] directories)
        {
            Directories = directories.ToList();
        }
    }
}
