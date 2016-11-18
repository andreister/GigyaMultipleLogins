namespace GigyaMultipleLogins.Responses
{
    public class FinalizeRegistrationResponse : GigyaResponseBase
    {
        public bool IsRegistered { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }

        internal override void ApplyValues(object gigyaResponse)
        {
            base.ApplyValues(gigyaResponse);

            dynamic response = gigyaResponse;
            IsRegistered = GetValue<bool>(response.isRegistered);
            IsVerified = GetValue<bool>(response.isVerified);
            IsActive = GetValue<bool>(response.isActive);
        }

        public override string ToString()
        {
            return $"IsRegistered={IsRegistered}, IsVerified={IsVerified}, IsActive={IsActive}, {base.ToString()}";
        }
    }
}