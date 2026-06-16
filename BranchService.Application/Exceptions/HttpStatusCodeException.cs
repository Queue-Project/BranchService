using System.Net;

namespace BranchService.Application.Exceptions;

public class HttpStatusCodeException: Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public HttpStatusCodeException(HttpStatusCode statusCode)
    {
        this.StatusCode = statusCode;
    }
    
    public HttpStatusCodeException(HttpStatusCode statusCode, string message) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner) : this(statusCode,inner.ToString())
    {
        
    }
}