namespace Helper.Swagger
{
    public class SwaggerOptions
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        public string TermsOfServiceUrl { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail{ get; set; }

        public string ContactUrl { get; set; }

        public string LicenseName { get; set; }

        public string LicenseUrl { get; set; }
    }

    public class SwaggerUISettings
    {
        public string EndPointName { get; set; }
        public string EndPointUrl { get; set; }
        public string StylePath { get; set; }
    }
}
