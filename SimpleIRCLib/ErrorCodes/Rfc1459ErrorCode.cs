namespace SimpleIRCLib.ErrorCodes
{
    public record class Rfc1459ErrorCode
    {
        public int Code { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public Rfc1459ErrorCode(int code, string name, string description)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }
    }
}
