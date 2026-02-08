using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HubTravelController : MonoBehaviour
{
    [SerializeField] private List<LvlTrigger> lvlTriggers;

    public void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            foreach (var lvlTrigger in lvlTriggers)
            {
                lvlTrigger.LoadNextScene();
            }
        }
    }
}
