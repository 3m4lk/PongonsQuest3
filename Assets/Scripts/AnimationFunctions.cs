using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    public GameObject[] objects;
    private void Awake()
    {
        GetComponent<Animator>().speed = GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().gameSpeed;
    }
    public void disableObject(int index)
    {
        objects[index].SetActive(false);
    }
    public void enableObject(int index)
    {
        objects[index].SetActive(true);
    }
}
