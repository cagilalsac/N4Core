namespace N4Core.Records.Bases
{
    public abstract class Record : IRecord
    {
        public int Id { get; set; }
        public string? Guid { get; set; }

        protected Record(int id)
        {
            Id = id;
        }

        protected Record()
        {
        }
    }
}
