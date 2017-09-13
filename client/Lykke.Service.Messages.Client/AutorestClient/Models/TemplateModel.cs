// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace swagger.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TemplateModel
    {
        /// <summary>
        /// Initializes a new instance of the TemplateModel class.
        /// </summary>
        public TemplateModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TemplateModel class.
        /// </summary>
        /// <param name="messageType">Possible values include: 'Sms',
        /// 'Email'</param>
        public TemplateModel(MessageType messageType, System.Guid? templateId = default(System.Guid?), string merchantId = default(string), string template = default(string), string templateType = default(string))
        {
            TemplateId = templateId;
            MerchantId = merchantId;
            Template = template;
            MessageType = messageType;
            TemplateType = templateType;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TemplateId")]
        public System.Guid? TemplateId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MerchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Sms', 'Email'
        /// </summary>
        [JsonProperty(PropertyName = "MessageType")]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TemplateType")]
        public string TemplateType { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}