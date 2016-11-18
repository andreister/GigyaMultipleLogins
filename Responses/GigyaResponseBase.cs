namespace GigyaMultipleLogins.Responses
{
    public class GigyaResponseBase
    {
        public string CallId { get; set; }
        public bool Succeeded { get; set; }
        public long ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        internal virtual void ApplyValues(object gigyaResponse)
        {
            dynamic response = gigyaResponse;

            CallId = GetValue<string>(response.callId);
            Succeeded = GetValue<long>(response.statusCode) == 200;
            ErrorCode = GetValue<long>(response.errorCode);
            ErrorMessage = GetValue<string>(response.errorDetails);
        }

        protected TValue GetValue<TValue>(dynamic obj)
        {
            return (obj != null) ? obj.Value : default(TValue);
        }

        public override string ToString()
        {
            return $"Succeeded={Succeeded}, ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}, CallId={CallId}";
        }
    }
}