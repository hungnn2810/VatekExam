namespace IdentityService.Commons.Communication
{
    public class ApiErrorMessage
    {
        internal ApiErrorMessage()
        { }

        public string Code { get; set; }
        public string Value { get; set; }

        public ApiErrorMessage Formart(params string[] formatValues)
        {
            return new ApiErrorMessage
            {
                Code = Code,
                Value = string.Format(Value, formatValues)
            };
        }
    }
}

