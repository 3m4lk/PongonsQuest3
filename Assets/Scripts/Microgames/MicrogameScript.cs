using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum mcg
{
    Null,
    Parry,
    BCA,
    Stream,
    Quiz,
    Shake,
    Drive,
    LetIn,
    Chase,
    Pray,
    Lag,
    Dance,
    Yap,
    Boss0,
    Boss1
}
public class MicrogameScript : MonoBehaviour
{
    private MicrogameManager manager;

    public mcg microgameType;
    public bool isPlaying;

    public Transform[] transforms;
    public GameObject[] gameObjects;
    public AnimationCurve[] curves;
    public float[] floats;
    public bool[] bools;

    public float gameSpeed = 1f;
    private void Awake()
    {
        manager = GameObject.Find("MicrogameManager").GetComponent<MicrogameManager>();
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime * gameSpeed;

        if (!isPlaying) return;
        switch (microgameType)
        {
            case mcg.Parry:
                // float 0: time;
                // float 1: progress;
                // float 2: parry window;
                // float 3: red flash period;
                // float 4: hitstun;

                // bool 0: red flash state;
                // bool 1: won parry;
                // bool 2: missed parry;

                // transform 0: truck;
                // transform 1: point0;
                // transform 2: point1;

                // gameObject 0: Shibbi 0;
                // gameObject 1: Shibbi 1;
                // gameObject 2: Pongon 0;
                // gameObject 3: Pongon 1;
                // gameObject 4: death explosion;
                // gameObject 5: victory explosion;
                // gameObject 6: parry fail;
                if (floats[4] != 0)
                {
                    floats[4] = Mathf.Max(floats[4] - deltaTime, 0f);

                    if (floats[4] == 0)
                    {
                        transforms[0].GetComponentInChildren<Image>().color = Color.white;
                        bools[0] = true;
                        floats[3] = 0.04f;
                        gameObjects[0].SetActive(false);
                        gameObjects[1].SetActive(true);
                    }
                    return;
                }

                if (!bools[1])
                {
                    if (floats[1] == 0)
                    {
                        print("failure");
                        isPlaying = false;
                        bools[2] = true;
                        gameObjects[4].SetActive(true);
                        manager.toggleWin(false);
                        gameObjects[0].SetActive(false);
                        gameObjects[1].SetActive(false);
                        gameObjects[2].SetActive(false);
                        gameObjects[3].SetActive(false);
                        gameObjects[6].SetActive(false);
                    }

                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);
                }
                else
                {
                    transforms[0].Rotate(Vector3.back, 360f * 4 * deltaTime);

                    floats[1] = Mathf.Min(floats[1] + deltaTime * 6f, floats[0]);

                    if (floats[1] == floats[0])
                    {
                        print("victory");
                        transforms[0].gameObject.SetActive(false);
                        gameObjects[5].SetActive(true);
                        manager.toggleWin(true);
                    }
                }

                float parryMult = curves[0].Evaluate(1f - (floats[1] / floats[0]));

                transforms[0].position = Vector3.Lerp(transforms[1].position, transforms[2].position, parryMult);
                transforms[0].localScale = Vector3.Lerp(transforms[1].localScale, transforms[2].localScale, parryMult);

                if (floats[1] <= floats[2])
                {
                    if ((floats[3] = Mathf.Max(floats[3] - Time.deltaTime, 0f)) == 0f)
                    {
                        floats[3] = 0.04f;

                        if (!bools[0])
                        {
                            transforms[0].GetComponentInChildren<Image>().color = Color.red;
                        } // flash red
                        else
                        {
                            transforms[0].GetComponentInChildren<Image>().color = Color.white;
                        }
                        bools[0] = !bools[0];
                    }
                }
                break;
            case mcg.BCA:
                break;
            case mcg.Stream:
                break;
            case mcg.Quiz:
                break;
            case mcg.Shake:
                break;
            case mcg.Drive:
                break;
            case mcg.LetIn:
                break;
            case mcg.Chase:
                break;
            case mcg.Pray:
                break;
            case mcg.Lag:
                break;
            case mcg.Dance:
                break;
            case mcg.Yap:
                break;
            case mcg.Boss0:
                break;
            case mcg.Boss1:
                break;
        }
    }
    public void startMG()
    {
        gameSpeed = manager.gameSpeed;
        switch (microgameType)
        {
            case mcg.Parry:
                floats = new float[5];
                floats[0] = 2.5f;
                floats[1] = floats[0];
                floats[2] = 0.6f;
                floats[3] = 0.04f;

                bools = new bool[3];
                break;
            case mcg.Stream:
                break;
            case mcg.Quiz:
                break;
            case mcg.Shake:
                break;
            case mcg.Drive:
                break;
            case mcg.LetIn:
                break;
            case mcg.Chase:
                break;
            case mcg.Pray:
                break;
            case mcg.Lag:
                break;
            case mcg.Dance:
                break;
            case mcg.Yap:
                break;
            case mcg.Boss0:
                break;
            case mcg.Boss1:
                break;
        }
        isPlaying = true;
    }

    public void handleInput(InputAction.CallbackContext obj)
    {
        if (!isPlaying) return;
        bool mode = obj.action.triggered;

        /*switch (obj.action.name)
        {
            case "":
                break;
        }//*/

        switch (microgameType)
        {
            case mcg.Parry:
                if (floats[4] != 0) break;
                if (!(bools[1] || bools[2]) && (obj.action.name == "Space" || obj.action.name == "LClick"))
                {
                    gameObjects[2].SetActive(false);
                    gameObjects[3].SetActive(true);
                    if (floats[1] <= floats[2])
                    {
                        bools[1] = true;
                        floats[4] = 0.75f;
                        transforms[0].GetComponentInChildren<Image>().color = Color.yellow;
                    }
                    else
                    {
                        bools[2] = true;
                        gameObjects[6].SetActive(true);
                    }
                }
                break;
            case mcg.Stream:
                break;
            case mcg.Quiz:
                break;
            case mcg.Shake:
                break;
            case mcg.Drive:
                break;
            case mcg.LetIn:
                break;
            case mcg.Chase:
                break;
            case mcg.Pray:
                break;
            case mcg.Lag:
                break;
            case mcg.Dance:
                break;
            case mcg.Yap:
                break;
            case mcg.Boss0:
                break;
            case mcg.Boss1:
                break;
        }
    }
}
