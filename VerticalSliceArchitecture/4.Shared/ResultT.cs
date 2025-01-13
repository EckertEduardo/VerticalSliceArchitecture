namespace VerticalSliceArchitecture.Shared
{
    public class Result<T> : Result
    {
        private readonly T? _value;
        public T? Value => IsSuccess ? _value! : default;

        protected internal Result(T? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static implicit operator Result<T>(T? value)
        {
            return Create(value);
        }
    }
}

