#nullable disable

namespace N4Core.Files.Bases
{
    public abstract class FileDirectoryBase
    {
        public string[] FileDirectories { get; private set; }
        public bool HasFileDirectories => FileDirectories is not null && FileDirectories.Any();

        public string FilePath
        {
            get
            {
                if (HasFileDirectories)
                {
                    List<string> path = FileDirectories.ToList();
                    path.Insert(0, "wwwroot");
                    return Path.Combine(path.ToArray());
                }
                return string.Empty;
            }
        }

        public void SetFileDirectories(params string[] fileDirectories) => FileDirectories = fileDirectories?.ToArray();
    }
}
