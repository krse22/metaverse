using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
                            routes.Add(new Route(route.RequestType, combinedRoute));
                            Debug.Log(combinedRoute);
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
            if (route.MatchRoute(requestType, incomingRoute))
            {
                Debug.Log($"ACTIVATING ROUTE: {Enum.GetName(typeof(RequestType), route.requestType)}|{incomingRoute}");
            }
        }
    }

    private static string CombineBaseRouteWithRoute(string baseRoute, string route)
    {
        if (route != "")
        {
            return $"{baseRoute}/{route}";
        }
        return baseRoute;
    }

   

}
