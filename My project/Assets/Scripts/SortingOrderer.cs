using UnityEngine;

public class SortingOrderer : MonoBehaviour
{
    public int sortOrder = 0;
    public Renderer vfxRenderer;

    private void OnValidate()
    {
        if (vfxRenderer != null)
        {
            vfxRenderer.sortingOrder = sortOrder;
        }
        else if (TryGetComponent(out vfxRenderer))
        {
            vfxRenderer.sortingOrder = sortOrder;
        }
    }
}
