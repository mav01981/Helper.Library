namespace Helper.Web
{
    using System;

    [Serializable]
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
