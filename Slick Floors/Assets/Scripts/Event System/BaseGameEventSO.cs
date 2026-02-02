using UnityEngine;
using UnityEngine.Events;

public class BaseGameEventSO<T> : ScriptableObject
{
    private UnityAction<T> onEventRaised = delegate { };

    public void RegisterListener(UnityAction<T> listener) =>
        onEventRaised += listener;

    public void UnregisterListener(UnityAction<T> listener) =>
        onEventRaised -= listener;

    public void Raise(T item) =>
        onEventRaised.Invoke(item);
}
