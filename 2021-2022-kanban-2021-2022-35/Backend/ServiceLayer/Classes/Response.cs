using System.Text.Json;
using System.Text.Json.Serialization;


public class Response
{
    [JsonIgnore]
    public JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ErrorMessage;
    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

    public object ReturnValue { get; set; }
    
    public Response(){
        ReturnValue = null;
        ErrorMessage = null;
            }
    public Response(string msg)
    {

        ReturnValue = null;
        ErrorMessage = msg;
    }
    public void toResponse(object obj)
    {
        ReturnValue = obj ;
        ErrorMessage = null;
    }



}
