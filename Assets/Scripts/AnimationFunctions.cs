using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    public GameObject[] objects;
    public float speed;
    private Vector2 velo;
    private bool applyVelo;
    private float changeDire = 1f, gameSpeed;
    private void Awake()
    {
        gameSpeed = GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().gameSpeed;
        if (GetComponent<Animator>()) GetComponent<Animator>().speed = gameSpeed;
    }
    public void disableObject(int index)
    {
        objects[index].SetActive(false);
    }
    public void enableObject(int index)
    {
        objects[index].SetActive(true);
    }
    private void FixedUpdate()
    {
        if (speed != 0)
        {
            if (GetComponent<Rigidbody2D>().linearVelocity.magnitude < speed) GetComponent<Rigidbody2D>().linearVelocity = GetComponent<Rigidbody2D>().linearVelocity.normalized * speed;
            if (changeDire != 0)
            {
                changeDire = Mathf.Max(changeDire - Time.fixedDeltaTime * gameSpeed, 0f);

                if (changeDire == 0)
                {
                    Vector2 veloDire = ((GetComponent<Rigidbody2D>().position - GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().microgames[GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().gameObjects[0].GetComponent<Rigidbody2D>().position).normalized
                        + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized; // what the fuck

                    GetComponent<Rigidbody2D>().linearVelocity = veloDire * speed;

                    changeDire = Random.Range(0.6f, 1.4f);
                }
            }
        }

            if (applyVelo)
            {
                applyVelo = false;
                GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + velo * Time.fixedDeltaTime);
            }
    }
    public void setVelo(Vector2 input)
    {
        applyVelo = true;
        velo = input;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 veloDire = (GetComponent<Rigidbody2D>().linearVelocity.normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;

        GetComponent<Rigidbody2D>().linearVelocity = veloDire * speed;
        changeDire = Random.Range(0.6f, 1.4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (speed != 0 && collision.name == "Shibbi")
        {
            GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().microgames[GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>().currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().doWin();
            gameObject.SetActive(false);
        }
    }
}
