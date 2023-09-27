using UnityEngine;

public static class LayerExtensions
{
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
