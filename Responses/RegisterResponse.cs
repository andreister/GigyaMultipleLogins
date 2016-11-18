namespace GigyaMultipleLogins.Responses
{
    public class RegisterResponse : GigyaResponseBase
    {
        public string RegistrationToken { get; set; }
        public string UserId { get; set; }

        internal override void ApplyValues(object gigyaResponse)
        {
            base.ApplyValues(gigyaResponse);

            dynamic response = gigyaResponse;
            RegistrationToken = GetValue<string>(response.regToken);
            UserId = GetValue<string>(response.UID);

            if (!Succeeded && response.validationErrors != null)
            {
                ErrorCode = response.validationErrors[0].errorCode;
                ErrorMessage += " (" + response.validationErrors[0].message + ")";
            }
        }

        public override string ToString()
        {
            return $"UserId={UserId}, {base.ToString()}";
        }
    }
}