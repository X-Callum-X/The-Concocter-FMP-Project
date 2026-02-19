using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpToElement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable elementToJumpTo;

    [Header("Visualisation")]
    [SerializeField] private bool showVisualisation;
    [SerializeField] private Color navigationColor = Color.cyan;

    private void OnDrawGizmos()
    {
        if (!showVisualisation)
        {
            return;
        }

        if (elementToJumpTo == null)
        {
            return;
        }

        Gizmos.color = navigationColor;
        Gizmos.DrawLine(gameObject.transform.position, elementToJumpTo.transform.position);
    }

    private void Reset()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
        {
            Debug.Log("Did not find an Event System in your Scene.", this);
        }
    }

    public void JumpTo()
    {
        if (eventSystem == null)
        {
            Debug.Log("This item has no event system referenced yet.", this);
        }

        if (elementToJumpTo == null)
        {
            Debug.Log("Where should this jump to?", this);
        }

        eventSystem.SetSelectedGameObject(elementToJumpTo.gameObject);
    }
}
