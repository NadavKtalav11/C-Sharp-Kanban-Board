using System.Text.Json.Serialization;

namespace Backend.Service
{
    ///<summary>This class extends <c>Response</c> and represents the result of a call to a non-void function. 
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    ///<typeparam name="T">The type of the returned value of the function, stored by the list.</typeparam>
    public class Response<T> 
    {
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public readonly string? ErrorMessage;
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? ReturnValue;
        
        [JsonConstructor]
        public Response( T returnValue , string? errorMessage)
        {
            this.ReturnValue = returnValue;
            ErrorMessage = errorMessage;
        }

       

    }
}
