using UnityEngine;

public class TrackedImageBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private OpenBox uidoc;

    public void sendName(string name)
    {
        uidoc.setName(name);
    }
}
