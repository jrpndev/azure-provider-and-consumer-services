using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class CustomerDTO
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
