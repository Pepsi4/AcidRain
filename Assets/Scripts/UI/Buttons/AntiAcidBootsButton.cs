using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AntiAcidBootsButton : MonoBehaviour
{
    [SerializeField] private AntiAcidBoots boots;
    [SerializeField] private EventTrigger buttonEventTrigger;

    public void OnButtonPress()
    {
        boots.ActivateBoost();
    }

    public void OnButtonRelease()
    {
        boots.DeactivateBoost();
    }

    private void Start()
    {
        AddEventListener(buttonEventTrigger, EventTriggerType.PointerDown, OnButtonPress);
        AddEventListener(buttonEventTrigger, EventTriggerType.PointerUp, OnButtonRelease);
    }

    private void OnDestroy()
    {
        RemoveEventListener(buttonEventTrigger, EventTriggerType.PointerDown, OnButtonPress);
        RemoveEventListener(buttonEventTrigger, EventTriggerType.PointerUp, OnButtonRelease);
    }

    private void AddEventListener(EventTrigger trigger, EventTriggerType eventType, System.Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((temp) => callback());
        trigger.triggers.Add(entry);
    }

    private void RemoveEventListener(EventTrigger trigger, EventTriggerType eventType, System.Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((temp) => callback());
        trigger.triggers.Remove(entry);
    }
}
