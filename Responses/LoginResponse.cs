namespace GigyaMultipleLogins.Responses
{
    public class LoginResponse : GigyaResponseBase
    {
        public string RegistrationToken { get; set; }
        public string UserId { get; set; }
        public bool IsAccountLocked { get; set; }
        public bool IsLoginFailed { get; set; }

        internal override void ApplyValues(object gigyaResponse)
        {
            base.ApplyValues(gigyaResponse);

            dynamic response = gigyaResponse;
            UserId = GetValue<string>(response.UID);
            RegistrationToken = GetValue<string>(response.regToken);

            //expose more high-level fields so that clients won't need to rely on Gigya codes
            //(full list of Gigya error codes: http://developers.gigya.com/display/GD/Response+Codes+and+Errors+REST)
            IsAccountLocked = (ErrorCode == 403120);
            IsLoginFailed = (ErrorCode > 300000);
        }

        public override string ToString()
        {
            return $"UserId={UserId}, Locked={IsAccountLocked}, {base.ToString()}";
        }
    }
}