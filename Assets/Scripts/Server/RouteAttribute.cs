using System;

public class RouteAttribute : Attribute
{
    public string Route { get; }
    public RequestType RequestType { get; }
    public string RequestTypeString { get; }

    public RouteAttribute(string route, RequestType requestType)
    {
        Route = route;
        RequestType = requestType;
        RequestTypeString = Enum.GetName(typeof(RequestType), requestType);
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class GetAttribute : RouteAttribute
{
    public GetAttribute(string route) : base(route, RequestType.Get) { }
}

[AttributeUsage(AttributeTargets.Method)]
public class PostAttribute : RouteAttribute
{
    public PostAttribute(string route) : base(route, RequestType.Post) { }
}

[AttributeUsage(AttributeTargets.Method)]
public class PutAttribute : RouteAttribute
{
    public PutAttribute(string route) : base(route, RequestType.Put) { }
}

[AttributeUsage(AttributeTargets.Method)]
public class DeleteAttribute : RouteAttribute
{
    public DeleteAttribute(string route) : base(route, RequestType.Delete) { }
}