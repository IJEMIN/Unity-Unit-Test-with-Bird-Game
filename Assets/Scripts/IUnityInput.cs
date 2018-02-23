using UnityEngine;

public interface IUnityInputService
{
    bool GetButtonDown(string buttonName);
}

public class UnityInputService : IUnityInputService
{
    public bool GetButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }
    
}

