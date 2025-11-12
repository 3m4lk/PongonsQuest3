using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationFunctions : MonoBehaviour
{
    public GameObject[] objects;
    public float speed;
    private Vector2 velo;
    private bool applyVelo;
    private float changeDire = 1f, gameSpeed;

    public bool isTouhou;
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

            Vector2 targetPos = GetComponent<Rigidbody2D>().position + velo * Time.fixedDeltaTime;
            if (isTouhou)
            {
                targetPos.x = Mathf.Clamp(targetPos.x, 60f, 310f);
                targetPos.y = Mathf.Clamp(targetPos.y, 60f, 420f);
            }
            GetComponent<Rigidbody2D>().MovePosition(targetPos);
        }
    }
    public void setVelo(Vector2 input)
    {
        applyVelo = true;
        velo = input;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouhou) return;
        Vector2 veloDire = (GetComponent<Rigidbody2D>().linearVelocity.normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;

        GetComponent<Rigidbody2D>().linearVelocity = veloDire * speed;
        changeDire = Random.Range(0.6f, 1.4f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isTouhou)
        {
            MicrogameManager manager = GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>();
            if (speed != 0 && collision.name == "Shibbi") // && manager.microgames[manager.currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().bools[0])
            {
                manager.microgames[manager.currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().doWin();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (objects[0].GetComponent<MicrogameScript>().floats[3] == 0)
            {
                print("harmed");
                objects[0].GetComponent<MicrogameScript>().ints[0]--;

                for (int i = 0; i < 5; i++)
                {
                    objects[0].GetComponent<MicrogameScript>().gameObjects[5 + i].SetActive(i < objects[0].GetComponent<MicrogameScript>().ints[0]);
                }

                if (objects[0].GetComponent<MicrogameScript>().ints[0] <= 0)
                {
                    print("died!");
                    objects[0].GetComponent<MicrogameScript>().bools[0] = false;
                    objects[0].GetComponent<MicrogameScript>().getManager().toggleWin(false);
                    objects[0].GetComponent<MicrogameScript>().getManager().lowerTimer(3);

                    objects[0].GetComponent<MicrogameScript>().gameObjects[11].GetComponent<TMP_Text>().text = "erm. what the                                <size=0>;</size>\n<size=32><color=red>HYPER DEMON! <size=0>;</size>\n</color></size>??????";

                    objects[0].GetComponent<MicrogameScript>().gameObjects[17].GetComponent<AudioSource>().Play();
                    gameObject.SetActive(false);
                } // failure (dead)
                else
                {
                    objects[0].GetComponent<MicrogameScript>().gameObjects[15 + Random.Range(0, 2)].GetComponent<AudioSource>().Play();
                    objects[0].GetComponent<MicrogameScript>().floats[3] = 1f;
                    if (collision.name != "boss") Destroy(collision.gameObject);
                } // just hurt
            }
        }
    }
    public void moveScenes(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void deacSelf()
    {
        gameObject.SetActive(false);
    }
}
