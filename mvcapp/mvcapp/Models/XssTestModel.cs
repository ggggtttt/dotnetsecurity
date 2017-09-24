using System.Web.Mvc;

namespace mvcapp.Models
{
    public class XssTestModel
    {
        public string Value { get; set; }

        [AllowHtml]
        public string XmlValue { get; set; }

        [AllowHtml]
        public string XmlValue2 { get; set; }
    }
}
