using Newtonsoft.Json;

namespace DreamQuiz
{
    public class UserDto
    {
        [JsonProperty("userId")] public string UserId { get; set; }
        [JsonProperty("thirdPartyId")] public string ThirdPartyId { get; set; }
        [JsonProperty("userName")] public string UserName { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("fullName")] public string FullName { get; set; }
        [JsonProperty("role")] public string Role { get; set; }
        [JsonProperty("birthDate")] public string BirthDate { get; set; }
        [JsonProperty("isEmailVerified")] public string IsEmailVerified { get; set; }
        [JsonProperty("pictureUrl")] public string PictureUrl { get; set; }
        [JsonProperty("createdOn")] public string CreatedOn { get; set; }
        [JsonProperty("lastModified")] public string LastModified { get; set; }
    }
}