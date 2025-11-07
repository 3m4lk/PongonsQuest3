using TMPro;
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
    public int[] ints;
    public float[] floats;
    public bool[] bools;
    public string[] strings;

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
                        gameObjects[7].SetActive(false);
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
                if (bools[0])
                {
                    bools[0] = false;
                    manager.toggleWin(false);
                    isPlaying = false;
                }
                break;
            case mcg.Stream:
                // float 0: arrow position
                // float 1: post-select timer

                // int 0: target stream
                // int 1: last integer position

                // bool 0: has clicked
                // bool 1: win condition (for results screen)

                // transform 0: cursor
                // transform 0: cursorPos0
                // transform 0: cursorPos1

                // GOs 0-4: stream buttons
                // GO 5: NEEDY bg (for intro greenscreen animation)
                // GO 6: animation failure
                // GO 7: animation success
                // GO 8: NEEDY stream bg
                // GO 9: choice Pongon Shibbi
                // GO 10: choice buttons collective
                // GO 10: choice mask

                if (floats[1] != 0)
                {
                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);

                    if (floats[1] == 0)
                    {
                        manager.lowerTimer(3f);
                        // result-dependent screen show

                        if (bools[1])
                        {
                            // disable the cursor & buttons
                            gameObjects[9].SetActive(false);
                            gameObjects[10].SetActive(false);
                            transforms[0].gameObject.SetActive(false);
                            gameObjects[11].SetActive(false);

                            gameObjects[7].SetActive(true);
                            gameObjects[8].SetActive(true);
                        }
                        else gameObjects[6].SetActive(true);

                    }
                    return;
                }

                if (bools[0]) return;

                floats[0] = Mathf.Repeat(floats[0] + deltaTime * 3f, 5f);

                Vector3 pos = transforms[0].position;
                pos.y = Mathf.Lerp(transforms[1].position.y, transforms[2].position.y, floats[0] * 0.2f);
                transforms[0].position = pos;

                if (Mathf.FloorToInt(floats[0]) != ints[1])
                {
                    ints[1] = Mathf.FloorToInt(floats[0]);
                    // deselect all other buttons
                    // select current button
                    // play sound

                    for (int i = 0; i < 5; i++)
                    {
                        gameObjects[i].GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                        if (i == ints[1]) gameObjects[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    }
                }
                break;
            case mcg.Quiz:
                // int 0: current index
                // int 1: winner index

                // bool 0: has pressed
                // bool 1: is correct answer
                // bool 2: should ??? be spawned
                // bool 3: previous vertical mode state

                // float 0: results animation progress (kakashi "have a great day" meme on victory, screaming lion on failure)
                // float 1: ??? delay

                // strings 0: former ???

                // GOs 0-3: buttons
                // GO 4: quiz name
                // GO 5: win sprite
                // GO 6: failure sprite

                if (bools[2])
                {
                    if (floats[1] != 0)
                    {
                        floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);
                    }
                    else
                    {
                        bools[2] = false;
                        for (int i = 0; i < gameObjects.Length; i++)
                        {
                            if (gameObjects[i].GetComponentInChildren<TMP_Text>().text == "Gaster")
                            {
                                gameObjects[i].GetComponentInChildren<TMP_Text>().text = strings[0];
                                break;
                            }
                        }
                    }
                } // immediately replace ???

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
            case mcg.BCA:
                bools = new bool[1];
                bools[0] = true;
                break;
            case mcg.Stream:
                floats = new float[2];
                floats[0] = (int)(Random.Range(0f, 4f));
                floats[1] = 0;

                ints = new int[2];
                ints[0] = Random.Range(0, 5);
                ints[1] = Mathf.FloorToInt(floats[0]);

                bools = new bool[2];

                transforms[0].position = Vector3.Lerp(transforms[1].position, transforms[2].position, floats[0] * 0.2f);
                transforms[0].gameObject.SetActive(true);

                for (int i = 0; i < 5; i++)
                {
                    gameObjects[i].GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                    if (i == ints[1]) gameObjects[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                    gameObjects[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Placeholders/Microgames/Stream/streamCommon"); // common
                    string[] randDemot = new string[] { "Don't feel like it today...", "Let's stream... Not!", "Let's watch other streamers instead?", "Fuck daichi saito.", "Nah...", "No ideas, head empty...", "Let's just play smash instead?", "*yawn*", "I'd rather doomscroll tbh...", "Do we even Have to?", "No, no, no, uhh... No!", "I'm NEEDY for some laziness...", "Streamer GIRL? Not today...", "Streamer boy? Not today...", "Wanna OVERDOSE cheez-its instead?", "Not feeling like a STREAMER today...", "Emptiness OVERLOADs my brain...", "SUTORIIMINGO YAMERO!", "Brainfog, can't focus...", "I don't Wanna though...", "Shibbi, we are so DEMOTIVATED.", "Pongon, we are so BORED.", "We don't need AutoGreenScren frankly..?", "Streamishitai: The Ideas I'm Missing.", "Nuh uh...", "Nada...", "Nope...", "Neko Streamo Nuh-uh-no...", "Are you sleepy, princesses and princes?" };
                    gameObjects[i].GetComponentInChildren<TMP_Text>().text = randDemot[Random.Range(0, randDemot.Length)];
                    Random.seed = Random.Range(0, 1000000000 + System.DateTime.Now.Second);
                    if (i == ints[0])
                    {
                        gameObjects[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Placeholders/Microgames/Stream/streamTarget"); // target
                        gameObjects[i].GetComponentInChildren<TMP_Text>().text = "Silly Stream in Japan!!!!";
                    }
                }
                gameObjects[5].SetActive(true);

                break;
            case mcg.Quiz:
                gameObjects[4].GetComponent<TMP_Text>().text = "Microgame " + (manager.currentMicrogameIndex + 1) + ":\nWhat is Polygon Donut NOT often called?";

                //string[] randomWrong = new string[] { "Jeanice", "Hollow Knight: Silksong", "Impala 64", "Minecraft Pocket Edition", "Shibbi", "Jorjor Well", "Dolygon Ponut", "Gaster", "Marlok", "Gorn", "Khrobaron", "Dedede", "K. Rool", "Gianni Matragrano", "Thermonuclear Reactor", "Wriggle Nightbug" }; // immediately swap ??? to a different randomly chosen one

                string[] normalAnswers = new string[] { "Polygon Donut", "Pongon", "Mowmow", "pongondonute" };
                string[] wrongAnswers = new string[] { "Poiygon Donut", "Pungon", "Movmow", "pongondonate" };

                string[] finalAnswers = new string[4];
                for (int i = 0; i < finalAnswers.Length; i++)
                {
                    finalAnswers[i] = normalAnswers[i];
                }

                for (int i = 0; i < 2; i++)
                {
                    // Knuth shuffle algorithm :: courtesy of Wikipedia :)
                    for (int t = 0; t < finalAnswers.Length; t++)
                    {
                        string tmp = finalAnswers[t];
                        int r = Random.Range(t, finalAnswers.Length);
                        finalAnswers[t] = finalAnswers[r];
                        finalAnswers[r] = tmp;
                    }
                }

                ints = new int[2];
                ints[0] = 0;
                ints[1] = Random.Range(0, 4);

                bools = new bool[4];
                bools[2] = (Random.Range(1, 10) == 3);

                floats = new float[2];

                int randMystInt = Random.Range(0, 4);

                for (int i = 0; i < 4; i++)
                {
                    if (i == ints[1])
                    {
                        for (int a = 0; a < wrongAnswers.Length; a++)
                        {
                            if (finalAnswers[i] == normalAnswers[a])
                            {
                                finalAnswers[i] = wrongAnswers[a];
                                break;
                            }
                        }
                    } // impostor

                    if (bools[2] && i == randMystInt)
                    {
                        strings[0] = finalAnswers[i];
                        finalAnswers[i] = "Gaster";
                        floats[1] = 0.2f;
                        bools[2] = true;
                    } // ??? swap

                    gameObjects[i].GetComponentInChildren<TMP_Text>().text = finalAnswers[i];
                }
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
        string inputName = obj.action.name;

        /*switch (obj.action.name)
        {
            case "":
                break;
        }//*/

        switch (microgameType)
        {
            case mcg.Parry:
                if (floats[4] != 0) break;
                if (!(bools[1] || bools[2]) && (inputName == "Space" || inputName == "LClick"))
                {
                    gameObjects[2].SetActive(false);
                    gameObjects[3].SetActive(true);
                    if (floats[1] <= floats[2])
                    {
                        bools[1] = true;
                        floats[4] = 0.75f;
                        transforms[0].GetComponentInChildren<Image>().color = Color.yellow;
                        gameObjects[7].SetActive(true);
                    } // success
                    else
                    {
                        bools[2] = true;
                        gameObjects[6].SetActive(true);
                    } // miss
                }
                break;
            case mcg.Stream:
                if (inputName == "Space")
                {
                    bools[0] = true;
                    if (Mathf.FloorToInt(floats[0]) == ints[0])
                    {
                        manager.toggleWin(true);
                        print("Success!");
                        bools[1] = true;

                        gameObjects[ints[0]].GetComponent<Image>().sprite = Resources.Load<Sprite>("Placeholders/Microgames/Stream/streamSuccess");
                        gameObjects[ints[0]].GetComponentInChildren<TMP_Text>().text = "SILLY STREAM IN JAPAN!!!!";

                        // set currently hovered button to win sprite
                    } //success
                    else
                    {
                        manager.toggleWin(false);
                        print("Failure!");

                        gameObjects[Mathf.FloorToInt(floats[0])].GetComponent<Image>().sprite = Resources.Load<Sprite>("Placeholders/Microgames/Stream/streamNoStream");
                        // set currently hovered button to null sprite
                    } // failure
                    floats[1] = 0.6f;
                    // set a timer for 0.6 seconds, after which show a result-dependent screen
                }
                break;
            case mcg.Quiz:
                if (inputName == "Movement")
                {
                    //print(obj.action.ReadValue<Vector2>());

                    if (bools[3] != mode && mode)
                    {
                        ints[0] = (int)Mathf.Repeat(ints[0] - Mathf.Sign(obj.action.ReadValue<Vector2>().y), 4);

                        // update visual

                        for (int i = 0; i < 4; i++)
                        {
                            gameObjects[i].transform.GetChild(1).GetComponent<Image>().enabled = (i == ints[0]);
                        }
                    }

                    bools[3] = mode;
                }
                else if (inputName == "Space")
                {
                    manager.toggleWin(ints[0] == ints[1]);
                    gameObjects[5].SetActive(ints[0] == ints[1]);
                    gameObjects[6].SetActive(ints[0] != ints[1]);

                    // mark selector as correct / wrong (dependent on chosen option)
                    // highlight correct answer's text

                    manager.lowerTimer(3);
                }
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
