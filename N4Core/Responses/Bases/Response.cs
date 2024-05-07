using N4Core.Records.Bases;

namespace N4Core.Responses.Bases
{
    public abstract class Response : Record
    {
        public bool IsSuccessful { get; }
        public string Message { get; set; }

        protected Response(bool isSuccessful, string message, int id) : base(id)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

    public abstract class Response<TResponseType> : Response, IResponseData<TResponseType>
    {
        public TResponseType Data { get; }

        protected Response(bool isSuccessful, string message, TResponseType data, int id) : base(isSuccessful, message, id)
        {
            Data = data;
        }
    }
}
