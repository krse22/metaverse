using System;

public enum StatusCodes
{
    // Successful 2xx
    Ok = 200,
    Created = 201,

    // Client error 4xx
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    ToManyRequests = 429,
    
    // Server error 5xx
    InternalServerError = 500,
}

public class Response
{
    public static int globalTimeoutTimeMs = 5000;

    public byte ResponseId;
    public Action<Response> onResponse;

    public StatusCodes statusCode;
    public string body;

    public void StartTimeoutCounter()
    {
        // Some object will be needed where it will invoke timeout
        onResponse.Invoke(this);
    }

    public void InvokeResponse() 
    {
        onResponse.Invoke(this);
    }

}
