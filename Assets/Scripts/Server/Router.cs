using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class Router : MonoBehaviour
{
    public static List<Route> routes;

    private void Start()
    {
        ListenAndServe(25000);
    }

    public static void ListenAndServe(int port)
    {
        Debug.Log("Searching for controllers attached to game objects in the scene");
        routes = new List<Route>();
        FindControllersInScene();

        TCPServer.InitializeServer(port);
    }

    public static void FindControllersInScene()
    {
        var monoBehaviours = FindObjectsOfType<MonoBehaviour>();

        foreach (var monoBehaviour in monoBehaviours)
        {
            var controllerClass = monoBehaviour.GetType();
            var attributes = controllerClass.GetCustomAttributes(typeof(ControllerAttribute), true);

            if (attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute is ControllerAttribute controllerAttribute)
                    {
                        MethodInfo[] methods = controllerClass.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                        MethodInfo[] routerMethods = methods.Where(method => method.GetCustomAttributes().OfType<RouteAttribute>().Any()).ToArray();

                        foreach (MethodInfo method in routerMethods)
                        {
                            RouteAttribute route = method.GetCustomAttribute<RouteAttribute>();
                            string combinedRoute = CombineBaseRouteWithRoute(controllerAttribute.Route, route.Route);

                            var parameters = method.GetParameters();
                            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Request))
                            {
                                routes.Add(new Route(route.RequestType, combinedRoute, ConvertMethodInfoToAction(monoBehaviour, method)));
                                Debug.Log(combinedRoute);
                            } else
                            {
                                Debug.LogError($"Failed to create route {route.RequestType}|{combinedRoute} because it doesn't have Requst parameter");
                            }

 
                            // Debug.Log($"{route.RequestTypeString}|{controllerAttribute.Route}{route.Route}");    
                        }
                    }
                }
            }
        }
    }

    public static void InvokeRoutes(byte requestId, byte requestType, string incomingRoute, string jsonBody)
    {
        foreach (Route route in routes)
        {
            Dictionary<string, string> reqParams;
            string[] parts = incomingRoute.Split(new[] { '?' }, 2);
            string afterQuestionMark = parts.Length > 1 ? parts[1] : string.Empty;
            string basicRoute = parts[0];

            if (route.MatchRoute(requestType, basicRoute, out reqParams))
            {
                Request request = CreateRequest(reqParams, basicRoute, afterQuestionMark, jsonBody);
                route.methodToInvokeOnRoute.Invoke(request);
                // Debug.Log($"ACTIVATING ROUTE: {Enum.GetName(typeof(RequestType), route.requestType)}|{incomingRoute}");
            }
        }
    }

    private static Request CreateRequest(Dictionary<string, string> reqParams, string incomingRoute, string querystring, string jsonBody) 
    {
        Request request = new Request();
        request.requestParams = reqParams;
        request.body = jsonBody;
        request.query = querystring;
        return request;
    }

    private static string CombineBaseRouteWithRoute(string baseRoute, string route)
    {
        if (route != "")
        {
            return $"{baseRoute}/{route}";
        }
        return baseRoute;
    }

    // Avoid reflection to boost performance
    public static Action<Request> ConvertMethodInfoToAction(MonoBehaviour instance, MethodInfo method)
    {
        Action<Request> action = (Request request) =>
        {
            method.Invoke(instance, new object[] { request });
        };

        return action;
    }

}
