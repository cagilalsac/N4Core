using N4Core.Results;

namespace N4Core.Services.Bases
{
    public abstract class ServiceResult
    {
        public virtual ErrorResult Error(string message)
        {
            return new ErrorResult(message);
        }

        public virtual ErrorResult Error()
        {
            return new ErrorResult();
        }

        public virtual ErrorResult<TResultType> Error<TResultType>(string message, TResultType data)
        {
            return new ErrorResult<TResultType>(message, data);
        }

        public virtual ErrorResult<TResultType> Error<TResultType>(string message)
        {
            return new ErrorResult<TResultType>(message);
        }

        public virtual ErrorResult<TResultType> Error<TResultType>(TResultType data)
        {
            return new ErrorResult<TResultType>(data);
        }

        public virtual ErrorResult<TResultType> Error<TResultType>()
        {
            return new ErrorResult<TResultType>();
        }

        public virtual SuccessResult Success(string message)
        {
            return new SuccessResult(message);
        }

        public virtual SuccessResult Success()
        {
            return new SuccessResult();
        }

        public virtual SuccessResult<TResultType> Success<TResultType>(string message, TResultType data)
        {
            return new SuccessResult<TResultType>(message, data);
        }

        public virtual SuccessResult<TResultType> Success<TResultType>(string message)
        {
            return new SuccessResult<TResultType>(message);
        }

        public virtual SuccessResult<TResultType> Success<TResultType>(TResultType data)
        {
            return new SuccessResult<TResultType>(data);
        }

        public virtual SuccessResult<TResultType> Success<TResultType>()
        {
            return new SuccessResult<TResultType>();
        }
    }
}
