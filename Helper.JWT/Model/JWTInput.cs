using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.JWT.Model
{
    public class JWTInput
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }

}
