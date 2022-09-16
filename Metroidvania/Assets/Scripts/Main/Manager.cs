using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    private void OnEnable()
    {
        SetManager();
    }

    private void OnDestroy()
    {
        ResetManager();
    }

    protected abstract void SetManager();
    protected abstract void ResetManager();
}