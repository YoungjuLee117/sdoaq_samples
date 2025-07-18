namespace SDOAQNet.Tool
{
    public sealed class SdoaqEventArgs : System.EventArgs
    {
        public SDOAQ.SDOAQ_API.eErrorCode ErrorCode { get; }
        public string ErrorMessage { get; }
        public SdoaqEventArgs(SDOAQ.SDOAQ_API.eErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
