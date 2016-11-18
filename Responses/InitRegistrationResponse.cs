namespace GigyaMultipleLogins.Responses
{
    public class InitRegistrationResponse : GigyaResponseBase
    {
        public string RegistrationToken { get; set; }

        internal override void ApplyValues(object gigyaResponse)
        {
            base.ApplyValues(gigyaResponse);

            dynamic response = gigyaResponse;
            RegistrationToken = GetValue<string>(response.regToken);
        }
    }
}