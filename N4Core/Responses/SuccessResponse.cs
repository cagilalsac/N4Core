#nullable disable

using N4Core.Responses.Bases;

namespace N4Core.Responses
{
    public class SuccessResponse : Response
    {
        public SuccessResponse(string message, int id) : base(true, message, id)
        {
        }

        public SuccessResponse(string message) : base(true, message, default)
        {
        }

        public SuccessResponse(int id) : base(true, string.Empty, id)
        {
        }

        public SuccessResponse() : base(true, string.Empty, default)
        {
        }
    }

    public class SuccessResponse<TResponseType> : Response<TResponseType>
    {
        public SuccessResponse(string message, TResponseType data, int id) : base(true, message, data, id)
        {
        }

        public SuccessResponse(string message, TResponseType data) : base(true, message, data, default)
        {
        }

        public SuccessResponse(string message) : base(true, message, default, default)
        {
        }

        public SuccessResponse(int id) : base(true, string.Empty, default, id)
        {
        }

        public SuccessResponse(TResponseType data) : base(true, string.Empty, data, default)
        {
        }

        public SuccessResponse() : base(true, string.Empty, default, default)
        {
        }
    }
}
