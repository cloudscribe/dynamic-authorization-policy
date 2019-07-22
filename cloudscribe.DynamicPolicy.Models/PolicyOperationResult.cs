namespace cloudscribe.DynamicPolicy.Models
{
    public class PolicyOperationResult
    {
        public PolicyOperationResult(bool succeeded, string message = "")
        {
            Succeeded = succeeded;
            Message = message;
        }
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
    }
}
