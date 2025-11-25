using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
                // GO 7: ...?
                // GO 8: parry sound
                // GO 9: explosion sound
                // GO 10: fail sound

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

                        gameObjects[9].GetComponent<AudioSource>().pitch = gameSpeed;
                        gameObjects[9].GetComponent<AudioSource>().Play();
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
                    if ((floats[3] = Mathf.Max(floats[3] - deltaTime, 0f)) == 0f)
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
                // GO 11: (choice mask apparently... something got fucked up?)

                // GO 12: choice sound
                // GO 13: success sound
                // GO 14: failure sound
                // GO 15: choice switch sound
                // GO 16: greenscreen toggle sound (played twice on both background switches)
                // GO 17: success music
                // GO 18: failure music

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

                            gameObjects[13].GetComponent<AudioSource>().Play();

                            GetComponent<AudioSource>().Stop();
                            gameObjects[17].GetComponent<AudioSource>().Play();
                        }
                        else
                        {
                            gameObjects[14].GetComponent<AudioSource>().Play();
                            gameObjects[6].SetActive(true);

                            GetComponent<AudioSource>().Stop();
                            gameObjects[18].GetComponent<AudioSource>().Play();
                        }

                    }
                    return;
                }

                if (bools[0]) return;

                floats[0] = Mathf.Repeat(floats[0] + deltaTime * 5f, 5f);

                Vector3 pos = transforms[0].position;
                pos.y = Mathf.Lerp(transforms[1].position.y, transforms[2].position.y, floats[0] * 0.2f);
                transforms[0].position = pos;

                if (Mathf.FloorToInt(floats[0]) != ints[1])
                {
                    ints[1] = Mathf.FloorToInt(floats[0]);
                    // deselect all other buttons
                    // select current button
                    // play sound

                    gameObjects[15].GetComponent<AudioSource>().Play();

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
                // GO 9: choice switch sound

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
                // float 0: progress
                // float 1: intro duration
                // float 2: outro duration

                // bool 0: cursorState
                // bool 1: has won

                // GO 0: Pongon
                // GO 1: Shibbi
                // GO 2: drink Pongon
                // GO 3: drink Shibbi
                // GO 4: meter
                // GO 5: cursor prompt
                // GO 6: ???
                // GO 7: ???
                // GO 8: drink Pongon alt
                // GO 8: drink Shibbi alt

                // transform 0: shaker main
                // transform 1 & 2: shaker main clamps
                // transform 3: shaker main shadow
                // transform 4: shaker top
                // transform 5: shaker bottom
                // transform 6: shaker top shadow
                // transform 7: shaker bottom shadow
                // transform 8: og shaker pos
                // transform 9: local shaker top clamp


                // if progress is above (maxProgress - some small margin) && shaker is somewhere near the middle, initiate finish animation
                if (floats[2] != 0)
                {
                    floats[2] = Mathf.Max(floats[2] - deltaTime, 0f);

                    float aniMult = 0;
                    if (floats[2] != 0) aniMult = floats[2] / 0.85f;

                    transforms[4].localPosition = Vector3.Lerp(transforms[9].localPosition, Vector3.zero, aniMult);
                    transforms[5].localPosition = Vector3.Lerp(-transforms[9].localPosition, Vector3.zero, aniMult);
                    transforms[6].localPosition = Vector3.Lerp(transforms[9].localPosition, Vector3.zero, aniMult);
                    transforms[7].localPosition = Vector3.Lerp(-transforms[9].localPosition, Vector3.zero, aniMult);

                    float fasterMult = Mathf.Min((1f - aniMult) * 1.55f, 1f);

                    gameObjects[2].transform.localScale = Vector3.one * fasterMult;
                    gameObjects[3].transform.localScale = Vector3.one * fasterMult;
                    gameObjects[2].GetComponent<CanvasGroup>().alpha = fasterMult * 1.5f;
                    gameObjects[3].GetComponent<CanvasGroup>().alpha = fasterMult * 1.5f;

                    return;
                }
                if (floats[1] != 0)
                {
                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);

                    float aniMult = 0;
                    if (floats[1] != 0) aniMult = floats[1] / 0.7f;

                    transforms[4].localPosition = Vector3.Lerp(Vector3.zero, transforms[9].localPosition, aniMult);
                    transforms[5].localPosition = Vector3.Lerp(Vector3.zero, -transforms[9].localPosition, aniMult);
                    transforms[6].localPosition = Vector3.Lerp(Vector3.zero, transforms[9].localPosition, aniMult);
                    transforms[7].localPosition = Vector3.Lerp(Vector3.zero, -transforms[9].localPosition, aniMult);

                    if (floats[1] == 0)
                    {
                        // play sound
                        gameObjects[0].SetActive(false);
                        gameObjects[1].SetActive(false);

                        gameObjects[5].SetActive(true); // enable cursor prompt
                    }

                    return;
                }
                if (bools[1]) return;
                floats[0] = Mathf.Max(floats[0] - deltaTime * 200f, 0f);

                if (floats[0] == 0) gameObjects[4].GetComponent<Image>().fillAmount = 0;
                else gameObjects[4].GetComponent<Image>().fillAmount = floats[0] / 1250f;

                //GameObject.Find("teText").GetComponent<TMP_Text>().text = floats[0] + "";
                break;
            case mcg.Drive:

                // float 0: wheel angle
                // float 1: vertical position
                // float 2: horizontal position
                // float 3: wheel release return time
                // float 4: wheel release og point
                // float 5: horizontal position target
                // float 6: Pongon blast timer
                // float 7: Fuji crumble timer
                // float 8: Pongon's horizontal position (for blast)
                // float 9: Pongon's LOCAL horizontal position (for blast rotation)

                // bool 0: is mouse held
                // bool 1: has gone off tracks (if Mathf.Abs(horizontalPosition) > 0.05f && !bools[1]: bools[1] = true; gameObjects[1].SetActive(true))
                // bool 2: has eyes emoji been triggered
                // bool 3: stage 1 completed?
                // bool 4: stage 1 failed?

                // curve 0: wheel release return curve
                // curve 1: Pongon blast curve (scale)
                // curve 2: Pongon blast curve (horizontal)
                // curve 1: Pongon blast curve (vertical)

                // transform 0: wheel
                // transform 1: arm
                // transform 2: ground
                // transform 3: billboards parent
                // transform 4: camera
                // transform 5: ground reverse
                // transform 6: billboards parent reverse
                // transform 7: fuji bg
                // transform 8: fuji bg reverse
                // transform 9: fuji mountain
                // transform 10: fuji pos 0
                // transform 11: fuji pos 1
                // transform 12: Pongon blast target

                // GO 0: interior (for hit animation)
                // GO 1: dust effects & derailment sound (train derailment, see bools[1] note)
                // GO 2: Pongon
                // GO 3: Pongon eyes emoji
                // GO 4: air freshener physics
                // GO 5: air freshener vis
                // GO 6: Pongon blast vis
                // GO 7: Pongon blast explosion
                // GO 8: honk sound


                gameObjects[5].transform.localRotation = gameObjects[4].transform.localRotation;

                if (!bools[0])
                {
                    floats[3] = Mathf.Max(floats[3] - deltaTime, 0f);
                    floats[0] = Mathf.Lerp(0.5f, floats[4], curves[0].Evaluate(floats[3] * 2f));
                }

                Physics2D.gravity = new Vector2(-(floats[0] - 0.5f) * 2f, -1f).normalized * 9.81f;

                //GameObject.Find("gravRef").transform.localPosition = Physics2D.gravity * 3f;

                transforms[4].localRotation = Quaternion.Euler(-30f, Mathf.Lerp(-20f, 20f, floats[0]), Mathf.Lerp(5f, -5f, floats[0]));
                floats[2] = Mathf.Clamp(floats[2] + (floats[0] - 0.5f) * deltaTime * 3f, 0f, 1f);

                if (!bools[1] && (floats[2] > 0.57f || floats[2] < 0.43f))
                {
                    bools[1] = true;
                    //print("derail!");
                    gameObjects[1].GetComponent<Animator>().speed = gameSpeed;
                    gameObjects[1].SetActive(true);
                    audioPlay(gameObjects[1].GetComponent<AudioSource>());
                    // play the sounds as well!
                }

                transforms[2].localPosition = new Vector3(Mathf.Lerp(4f, -4f, floats[2]), -11.1f, 4.5f);
                transforms[5].localPosition = new Vector3(Mathf.Lerp(4f, -4f, floats[2]), transforms[5].localPosition.y, transforms[5].localPosition.z);

                transforms[7].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(5f, -5f, floats[0]));
                transforms[8].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-1.5f, 1.5f, floats[0]));

                for (int i = 0; i < transforms[3].childCount; i++)
                {
                    transforms[3].GetChild(i).rotation = Quaternion.Euler(15f, 0f, 0f);
                    transforms[6].GetChild(i).rotation = Quaternion.Euler(195f, 180f, 0f);
                }

                transforms[0].localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(90f, -45f, floats[0]));
                transforms[1].localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-70f, -16f, floats[0]));

                floats[1] = Mathf.Min(floats[1] + deltaTime, 10f); // change to min later

                transforms[2].localRotation = Quaternion.Euler(floats[1] * 45f, 180f, 90f);
                transforms[5].localRotation = Quaternion.Euler(-floats[1] * 45f - 30f, 180f, 90f);
                transforms[7].localPosition = new Vector2(105f, Mathf.Lerp(90f, 160f, floats[1] * 0.1f));
                transforms[8].localPosition = new Vector2(515f, Mathf.Lerp(305f, 355f, floats[1] * 0.1f));

                if (!bools[3])
                {
                    if (!bools[2] && ((floats[2] > floats[5] && floats[5] > 0.5f) || (floats[2] < floats[5] && floats[5] < 0.5f)) && floats[1] > 3.7f)
                    {
                        bools[2] = true;
                        // toggle eyes emoji
                        gameObjects[2].SetActive(false);
                        gameObjects[3].SetActive(true);
                        audioPlay(gameObjects[3].GetComponent<AudioSource>());
                    }

                    // 3.7f -> 3.9f

                    if (floats[1] >= 4.3f && floats[1] <= 4.45f && (floats[2] < 0.2f && floats[5] < 0.5f || floats[2] > 0.8f && floats[5] > 0.5f))
                    {
                        bools[3] = true;
                        manager.toggleWin(true);
                        gameObjects[2].SetActive(false);
                        gameObjects[3].SetActive(false);
                        gameObjects[0].GetComponent<Animator>().SetTrigger("driveHit");
                        gameObjects[4].GetComponent<Rigidbody2D>().angularVelocity -= Mathf.Sign(floats[5] - 0.5f) * 4000f;

                        floats[8] = gameObjects[3].transform.position.x;
                        floats[9] = gameObjects[3].transform.localPosition.x;

                        gameObjects[6].transform.position = gameObjects[2].transform.position;
                        gameObjects[6].SetActive(true);
                        audioPlay(gameObjects[6].GetComponent<AudioSource>());

                        floats[6] = 2.2f;

                        print("Success!");
                    }
                    else if (floats[1] > 4.45f)
                    {
                        bools[3] = true;
                        print("Failure...");
                        gameObjects[2].SetActive(false);
                        gameObjects[3].SetActive(false);
                        bools[4] = true;
                        // toggle Pongon back sprite
                    }
                }
                else if (!bools[4])
                {
                    // when Pongon hits Mt. Fuji, start shaking animation in transforms[9] & lerp it from transforms[10] to transforms[11] (localPos & localRot)
                    if (floats[7] == 0)
                    {
                        floats[6] -= deltaTime;

                        Vector3 blastPos = Vector3.up * Mathf.LerpUnclamped(transforms[12].localPosition.y, -1.28f, curves[3].Evaluate(floats[6] / 2.2f));

                        blastPos.z = Mathf.Lerp(transforms[12].localPosition.z, 1.2f, curves[1].Evaluate(floats[6] / 2.2f));
                        // do blastPos.x based on Pongon's initial position & apply curve
                        gameObjects[6].transform.localPosition = blastPos;
                        blastPos = gameObjects[6].transform.position;

                        blastPos.x = Mathf.LerpUnclamped(transforms[12].position.x, floats[8], curves[2].Evaluate(floats[6] / 2.2f));

                        gameObjects[6].transform.position = blastPos;
                        gameObjects[6].transform.localScale = Vector3.one * curves[1].Evaluate(floats[6] / 2.2f);
                        //gameObjects[6].transform.localScale = Vector3.zero;
                        // also do rotation lerping; linear, Pongon's position-dependent

                        gameObjects[6].transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(40f, -470f, floats[6] / 2.2f) * Mathf.Sign(floats[9]));

                        if (floats[6] <= 0 && !transforms[9].GetComponent<Animator>().enabled)
                        {
                            transforms[9].GetComponent<Animator>().enabled = true;
                            floats[7] = 1.5f - floats[6];

                            gameObjects[6].SetActive(false);
                            gameObjects[7].SetActive(true);
                            audioPlay(gameObjects[7].GetComponent<AudioSource>(), 0.4f);
                        }
                    }
                    else
                    {
                        floats[7] = Mathf.Max(floats[7] - deltaTime, 0f);

                        transforms[9].localPosition = Vector2.Lerp(transforms[11].localPosition, transforms[10].localPosition, floats[7] / 1.2f);
                        transforms[9].localRotation = Quaternion.Lerp(transforms[11].localRotation, transforms[10].localRotation, floats[7] / 1.2f);
                    }
                }

                // if horPos is above 0.85f (in direction specified) && vertPos > 6.7f, change Pongon to eyes emoji

                //GameObject.Find("posText").GetComponent<TMP_Text>().text = "vertPos: " + floats[1] + "\nhorPos: " + floats[2];

                break;
            case mcg.LetIn:

                // int 0: current selected option (interrogation, let in, don't let in) // (microgame always results in failure if let in / not let in without interrogating)
                // int 1: Pongon's choice (food / not food dialogue)
                // int 2: slowprint index
                // int 3: talk sound delay
                // int 4: Shibbi's choice (food / not food)

                // float 0: slowprint progress

                // bool 0: has interrogated
                // bool 1: is game finished
                // bool 2: old mode (multiinput prevention for movement)

                // string 0: slowprint target
                // string 1: interrogation text

                // GOs 0-2: option buttons
                // GO 3: slowprint text & sound
                // GO 4: choice move sound
                // GO 5: choice sound
                // GO 6: Pongon vis
                // GO 7: Pongon burn vis
                // GOs 8-9: burn sounds
                // GO 10: hint food
                // GO 11: hint no food
                // GO 12: eat sound
                // GO 13: door sound

                if (ints[2] != strings[0].Length)
                {
                    int lastIndex = ints[2];

                    floats[0] -= deltaTime;

                    /*if (strings[0] == "I just bought this game from Gamerhalt! It's called: GD Colon Gets Killed Behind a Convenience Store in Super Tokyo at 3:46 AM, Reincarnates as a Fumo Plushie of Himself and Kills The Supreme God of Hyperdeath!"
                        || strings[0] == "I was just wondering if you wanted to HANG out with me and play with touys and fill our brains with youtube SHORTS and play Marlok the Wizard for the PC?"
                        || strings[0] == "I was just wondering if you wanted to HANG out with me and let me eat all your food and fill our bellies with DIET cola and play Marlok the Wizard for the PC?") floats[0] -= deltaTime * 2f; // abominable but i don't care; dialogue goes faster on those longer lines//*/ // ...NOT!

                    if (strings[0].Length > 120) floats[0] -= deltaTime * 2f; // dialogue goes faster on longer lines

                    for (; floats[0] < 0 && ints[2] != strings[0].Length; floats[0] += 0.16f / 3f, ints[2]++, ints[3]++)
                    {
                        if (ints[3] == 3)
                        {
                            ints[3] = 0;
                            if (ints[2] < strings[0].Length - 3) audioPlay(gameObjects[3].GetComponentInChildren<AudioSource>());
                        }
                    } // pretty much just for incrementing relevant values and playing the talk sound
                    if (lastIndex != ints[2])
                    {
                        string colInvis = "#00", colHalfTrans = "#80";//, colMain = "#69FFB2FF";
                        //string output = "<color=" + colMain + ">";
                        string output = default;

                        for (int i = 0; i < Mathf.Clamp(ints[2], 0, strings[0].Length); i++)
                        {
                            output += strings[0][i];
                        }
                        output += "<alpha=" + colHalfTrans + ">";
                        for (int i = ints[2]; i < Mathf.Clamp(ints[2] + 1, 0, strings[0].Length); i++)
                        {
                            output += strings[0][Mathf.Max(i, 0)];
                        }
                        output += "</color><alpha=" + colInvis + ">";
                        for (int i = ints[2] + 1; i < strings[0].Length; i++)
                        {
                            output += strings[0][Mathf.Max(i, 0)];
                        }

                        gameObjects[3].GetComponentInChildren<TMP_Text>().text = output + "</color>";
                    } // update text
                }

                break;
            case mcg.Chase:

                // float 0: input x
                // float 1: input y
                // // float 2: changeDire

                // bool 0: setup check

                // transform 0: Pongon visual

                // GO 0: Shibbi Rb
                // GO 1: YOU prompt
                // GO 2: jumpscare
                // GO 3: chase music

                /*if (!bools[0])
                {
                    bools[0] = true;

                    print(floats[2]);

                    transforms[0].GetComponentInParent<AnimationFunctions>().speed *= gameSpeed;
                    transforms[0].GetComponentInParent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(Mathf.Min(0f, floats[2]), Mathf.Max(0f, floats[2])), Random.Range(-1f, 1f)) * transforms[0].GetComponentInParent<AnimationFunctions>().speed;
                    transforms[0].GetComponentInParent<AnimationFunctions>().enabled = true;

                    gameObjects[0].name = "Shibbi";
                }//*/

                float randRange = 2f;
                transforms[0].localPosition = new Vector3(Random.Range(-randRange, randRange), Random.Range(-randRange, randRange), Random.Range(-randRange, randRange));

                gameObjects[0].GetComponent<AnimationFunctions>().setVelo(new Vector2(floats[0], floats[1]) * gameSpeed * 190f);
                break;
            case mcg.Pray:

                // 0: hold space to bring hands up
                // 1: hold mouse at left / right position and hold it till opposite hand moves into place
                // 2: repeat with opposite side
                // 3: release space

                // float 0: space held time
                // float 1: hand L progress
                // float 2: hand R progress

                // bool 0: left bubble held
                // bool 1: right bubble held
                // bool 2: space held
                // bool 3: finished

                // transform 0: hands container
                // transform 1: hand L
                // transform 2: hand R

                // GO 0: bubble container
                // GO 1: bubble L
                // GO 2: bubble R
                // GO 3: prayer sound
                // GO 4: hint visual
                // GO 5: prayer finish sound

                // curve 0: hands raise curve

                if (bools[3])
                {
                    floats[0] = Mathf.Clamp(floats[0] - deltaTime * 0.5f, 0f, 1.5f);
                    transforms[0].localPosition = Vector2.up * Mathf.Lerp(-170f, 0f, curves[0].Evaluate(floats[0] / 1.5f));
                    gameObjects[0].GetComponent<CanvasGroup>().alpha = curves[0].Evaluate(floats[0] / 4.5f);
                }
                else
                {
                    if (bools[2])
                    {
                        floats[0] = Mathf.Clamp(floats[0] + deltaTime, 0f, 1.5f);

                        if (floats[0] == 1.5f)
                        {
                            float prevVal = floats[1];
                            if (bools[0])
                            {
                                floats[1] = Mathf.Clamp(floats[1] + deltaTime, 0f, 3f);

                                if (floats[1] == 3f && prevVal != floats[1]) audioPlay(gameObjects[3].GetComponent<AudioSource>());
                            }
                            if (bools[1])
                            {
                                prevVal = floats[2];
                                floats[2] = Mathf.Clamp(floats[2] + deltaTime, 0f, 3f);
                                if (floats[2] == 3f && prevVal != floats[2]) audioPlay(gameObjects[3].GetComponent<AudioSource>());
                            }
                        }
                    }
                    else
                    {
                        if (floats[1] == 3 && floats[2] == 3)
                        {
                            bools[3] = true;
                            manager.toggleWin(true);
                            manager.lowerTimer(3f);
                            audioPlay(gameObjects[5].GetComponent<AudioSource>(), 0.75f);
                            return;
                        }
                        floats[0] = Mathf.Clamp(floats[0] - deltaTime * 1.5f, 0f, 1.5f);

                        floats[1] = Mathf.Clamp(floats[1] - deltaTime * 1.3f, 0f, 3f);
                        floats[2] = Mathf.Clamp(floats[2] - deltaTime * 1.3f, 0f, 3f);

                        bools[0] = false;
                        bools[1] = false;
                    }

                    if (!bools[0] && floats[1] != 3) floats[1] = Mathf.Clamp(floats[1] - deltaTime * 1.3f, 0f, 3f); // difference between 1.5 in hands lower and 1.3 here is intentional
                    if (!bools[1] && floats[2] != 3) floats[2] = Mathf.Clamp(floats[2] - deltaTime * 1.3f, 0f, 3f);

                    transforms[1].localPosition = new Vector2(Mathf.Lerp(-150f, 0f, curves[0].Evaluate(floats[1] / 3f)), 0);
                    transforms[2].localPosition = new Vector2(Mathf.Lerp(150f, 0f, curves[0].Evaluate(floats[2] / 3f)), 0);

                    gameObjects[1].GetComponent<CanvasGroup>().alpha = 1f - (floats[1] / 3.1f);
                    gameObjects[2].GetComponent<CanvasGroup>().alpha = 1f - (floats[2] / 3.1f);

                    transforms[0].localPosition = Vector2.up * Mathf.Lerp(-170f, 0f, curves[0].Evaluate(floats[0] / 1.5f));
                    transforms[0].GetComponent<AudioSource>().volume = curves[0].Evaluate(floats[0] / 1.5f);
                    gameObjects[4].GetComponent<CanvasGroup>().alpha = curves[0].Evaluate(1f - (floats[0] / 0.75f));

                    gameObjects[0].GetComponent<CanvasGroup>().alpha = curves[0].Evaluate(floats[0] / 1.5f);
                    gameObjects[1].GetComponent<EventTrigger>().enabled = (floats[0] != 0);
                    gameObjects[2].GetComponent<EventTrigger>().enabled = (floats[0] != 0);
                }

                break;
            case mcg.Lag:

                // int 0: last index spawned

                // float 0: progress
                // float 1: pitch & volume lower time

                // bool 0: has Player pressed
                // bool 1: has Player won

                // transforms 0-2: Pongons

                // GO 0: music
                // GO 1: stop sign
                // GO 2: Player indicator
                // GO 3: Player Pongon
                // GO 4: victory animation
                // GO 5: Pongon outline

                if (floats[1] != -1)
                {
                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);
                    gameObjects[0].GetComponent<AudioSource>().pitch = floats[1] * 0.5f;
                    break;
                }

                floats[0] = Mathf.Min(floats[0] + deltaTime, 6.531f);

                gameObjects[5].SetActive(floats[0] >= 1.224f && floats[0] <= 1.663f);

                for (int i = 0; i < transforms.Length; i++)
                {
                    Vector2 locPos = transforms[i].localPosition;
                    locPos.y = curves[i].Evaluate(floats[0]);
                    transforms[i].localPosition = locPos;
                }

                gameObjects[1].SetActive(floats[0] < 1.633f);

                if (floats[0] >= 3.265f && !gameObjects[4].activeInHierarchy)
                {
                    if (bools[1])
                    {
                        bools[1] = false;
                        gameObjects[4].GetComponentInChildren<Animator>().speed = gameSpeed;
                        gameObjects[4].SetActive(true);
                    } // victory animation
                    else
                    {
                        floats[1] = 2f;
                        manager.toggleWin(false);
                        manager.lowerTimer(3);
                    } // Failure (too late)
                }

                // window of error: 0.2 sec. (0.1 sec. before & 0.1 sec. after)
                // if passes that threshold, do the ending based on Player input

                break;
            case mcg.Dance:

                // int 0: current index
                // int 1: next move (up, down, left, right)

                // float 0: time until next arrow
                // float 1: animation return time

                // bool 0: is note
                // bool 1: finished
                // bool 2: misinput measure

                // GO 0: Pongon
                // GO 1: arrow
                // GO 2: Shibbi container
                // GO 3: Pongon victory
                // GO 4: arrow interior (regular arrow, 1 is technically the outline)
                // GO 5: explosion

                // GO 9: crowd behind
                // GO 10: crowd behind
                // GO 11: crowd cheering sound (success)

                // idle: (freeball it)
                // up: Wriggle T
                // down: Dedede crouch
                // left: Wriggle leg up pose
                // right: Wriggle side pose in the intro
                // failure: eating shit

                if (bools[1]) break;

                if (floats[1] != 0)
                {
                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);

                    if (floats[1] == 0)
                    {
                        for (int i = 0; i < gameObjects[0].transform.childCount; i++)
                        {
                            gameObjects[0].transform.GetChild(i).gameObject.SetActive(false);
                        }
                        gameObjects[0].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                for (floats[0] -= deltaTime; floats[0] < 0; floats[0] += 1f)
                {
                    if (!bools[0])
                    {
                        bools[0] = true;
                        ints[1] = Random.Range(0, 4);
                        switch (ints[1])
                        {
                            case 0:
                                gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                                gameObjects[4].GetComponent<Image>().color = new Color32(255, 224, 0, 255);
                                break; // up
                            case 1:
                                gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 180);
                                gameObjects[4].GetComponent<Image>().color = new Color32(96, 255, 0, 255);
                                break; // down
                            case 2:
                                gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 90);
                                gameObjects[4].GetComponent<Image>().color = new Color32(0, 224, 255, 255);
                                break; // left
                            case 3:
                                gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 270);
                                gameObjects[4].GetComponent<Image>().color = new Color32(255, 0, 128, 255);
                                break; // right
                        }
                        gameObjects[1].SetActive(true);
                    }
                    else
                    {
                        print("Failure (late)");
                        manager.toggleWin(false);
                        manager.lowerTimer(3);
                        bools[1] = true;

                        for (int i = 0; i < gameObjects[0].transform.childCount; i++)
                        {
                            gameObjects[0].transform.GetChild(i).gameObject.SetActive(false);
                        }
                        gameObjects[0].transform.GetChild(1).gameObject.SetActive(true);
                        gameObjects[5].SetActive(true);

                        gameObjects[1].SetActive(false);

                        gameObjects[2].transform.GetChild(0).gameObject.SetActive(false);
                        gameObjects[2].transform.GetChild(2).gameObject.SetActive(true);

                        gameObjects[0].GetComponent<Animator>().speed = 0;

                        return;
                    } // failure
                }

                float angle = Mathf.Lerp(365f, 270f, floats[0]);

                float sinVal = Mathf.Sin((angle * Mathf.PI) / 180f);
                float cosVal = Mathf.Cos((angle * Mathf.PI) / 180f);
                gameObjects[1].transform.localPosition = Vector2.up * -220f + new Vector2(sinVal, cosVal) * 284f;

                gameObjects[1].GetComponent<Image>().enabled = (floats[0] <= 0.25f);

                break;
            case mcg.Yap:

                // int 0: current sound index
                // int 1: progress

                // bool 0: has finished

                // float 0: anim duration (it has ~2-3 frames of closed mouth, them some time of mouth open, and the mouth closing when thr timer reaches 0)
                // float 1: antiautoclick measure / sound minimum duration
                // float 2: "subtitles by malk" time
                // float 3: time till disappear subtitles

                // transform 0: Shibbi

                // GOs 0-9: yap sounds
                // GO 10: raahh sound
                // GO 11: subtitle mask
                // GO 12: "subtitles by malk" text
                // GO 13-14: Shibbi talk sprites
                // GO 15: Shibbi final animation

                if (!bools[0])
                {
                    //print(transforms[0].localPosition);

                    floats[1] = Mathf.Max(floats[1] - deltaTime, 0f);
                    Vector2 ShibPos = transforms[0].localPosition;
                    ShibPos.y = Mathf.Lerp(33.1f, 20f, floats[1] / 0.12f);
                    transforms[0].localPosition = ShibPos;

                    floats[0] = Mathf.Max(floats[0] - deltaTime, 0f);
                    bool isOpen = (floats[0] >= 0.1f || floats[0] == 0);
                    gameObjects[13].SetActive(isOpen);
                    gameObjects[14].SetActive(!isOpen);

                    gameObjects[12].SetActive((floats[2] = Mathf.Max(floats[2] - deltaTime, 0f)) != 0);
                } // if hasn't won yet
                else
                {
                    gameObjects[11].SetActive((floats[3] = Mathf.Max(floats[3] - deltaTime, 0f)) != 0);
                }
                    break;
            case mcg.Boss0:

                // int 0: health
                // int 1: boss phase (0: survival; 1: charging (mash space))
                // int 2: power (mashing spacebar)

                // floats 0-1: Player movement vector
                // float 2: boss duration (till Charge!)
                // float 3: i-frames
                // float 4: cooldown straight
                // float 5: cooldown curved
                // float 6: cooldown giant
                // float 7: cooldown flaker
                // float 8: Mt. Fuji scroll time
                // float 9: intro Player hitbox wait time
                // float 10: Terry appear time
                // float 12: ending zoom duration (in in phase 2, out in phase 3)

                // bool 0: is alive

                // transform 0: Player
                // transform 1: Boss (Stan Luciferin)
                // transform 2: bullet parent
                // transform 3: Mt. Fuji bg
                // transform 4: Terry
                // transforms 5-6: bullet parent zoom ref points
                // transform 7: power text thing but parent idgaf
                // transform 8: charge ball

                // GO 0: straight
                // GO 1: curved
                // GO 2: giant
                // GO 3: flaker
                // GO 4: star item
                // GOs 5-9: health icons
                // GO 10: intro Player hitbox
                // GO 11: boss status text
                // GO 12: power thingy
                // GO 13: touhou item sound
                // GO 14: boss timer
                // GO 15: Pongon hurt sound
                // GO 16: Shibbi hurt sound
                // GO 17: death sound
                // GO 18: outro spellcard animation
                // GO 19: lazer
                // GO 20: hint

                // curve 0: Terry appear
                // curve 1: Terry shake
                // curve 2: zoom curve


                // endurance / survival for 30 seconds

                // 0-10: 1 bullet
                // 11-15: 2 bullets
                // 16-22: 4 bullets
                // 23-30: 8 bullets

                // 5: curvers (first gentle, then with higher curves)
                // 12: giants (first single then triple)
                // 20: spawners (first slow, then those shooting little shits)

                floats[8] = Mathf.Min(floats[8] + deltaTime, 56f);

                transforms[3].localPosition = Vector3.up * Mathf.Lerp(45f, -45f, floats[8] / 56f);

                floats[9] = Mathf.Max(floats[9] - deltaTime, 0f);
                if (floats[9] == 0) gameObjects[10].GetComponent<CanvasGroup>().alpha -= deltaTime;

                switch (ints[1])
                {
                    case 0:
                        if (!bools[0]) break;

                        floats[3] = Mathf.Max(floats[3] - deltaTime, 0f); // i-frames

                        if (floats[3] != 0) transforms[0].GetComponent<Image>().enabled = !transforms[0].GetComponent<Image>().enabled;
                        else transforms[0].GetComponent<Image>().enabled = true;

                            floats[2] = Mathf.Min(floats[2] + deltaTime, 40f);

                        string bossTime = gameObjects[14].GetComponent<TMP_Text>().text;
                        gameObjects[14].GetComponent<TMP_Text>().text = Mathf.FloorToInt(40f - floats[2]) + "";

                        if (gameObjects[14].GetComponent<TMP_Text>().text != bossTime)
                        {
                            //print("timeChange");
                            bossTime = gameObjects[14].GetComponent<TMP_Text>().text;
                            if (bossTime == "5" || bossTime == "4" || bossTime == "3" || bossTime == "2" || bossTime == "1" || bossTime == "0")
                            {
                                // play timer sound
                                gameObjects[14].GetComponent<TMP_Text>().color = Color.yellow;
                                gameObjects[14].GetComponent<AudioSource>().Play();
                            }
                        }

                        if (floats[2] != 40)
                        {
                            float shootSpeed = Mathf.Lerp(0.8f, 0.5f, floats[2] / 40f);
                            int quantity = Mathf.FloorToInt(Mathf.Lerp(1, 8, floats[2] / 35f));
                            GameObject bullet = gameObjects[0];

                            for (floats[4] -= deltaTime; floats[4] <= 0f; floats[4] += shootSpeed)
                            {
                                spawnBullet(bullet, quantity);
                            }

                            if (floats[2] >= 5f)
                            {
                                shootSpeed = Mathf.Lerp(0.8f, 0.5f, floats[2] / 40f);
                                quantity = Mathf.FloorToInt(Mathf.Lerp(1, 3, floats[2] / 35f));
                                bullet = gameObjects[1];

                                for (floats[5] -= deltaTime; floats[5] <= 0f; floats[5] += shootSpeed)
                                {
                                    spawnBullet(bullet, quantity);
                                }
                            }
                            if (floats[2] >= 12f)
                            {
                                shootSpeed = Mathf.Lerp(3f, 2.5f, floats[2] / 40f);
                                quantity = Mathf.FloorToInt(Mathf.Lerp(1, 3, floats[2] / 35f));
                                bullet = gameObjects[2];

                                for (floats[6] -= deltaTime; floats[6] <= 0f; floats[6] += shootSpeed)
                                {
                                    spawnBullet(bullet, quantity);
                                }
                            }
                            if (floats[2] >= 20f)
                            {
                                shootSpeed = Mathf.Lerp(2f, 1f, floats[2] / 40f);
                                quantity = Mathf.FloorToInt(Mathf.Lerp(1, 2, floats[2] / 35f));
                                bullet = gameObjects[3];

                                for (floats[6] -= deltaTime; floats[6] <= 0f; floats[6] += shootSpeed)
                                {
                                    spawnBullet(bullet, quantity);
                                }
                            }

                            transforms[0].GetComponent<AnimationFunctions>().setVelo(new Vector2(floats[0], floats[1]) * gameSpeed * 96f);

                            //print(transforms[0].GetComponent<Rigidbody2D>().position);
                        }
                        else
                        {
                            ints[1] = 1;

                            transforms[0].localPosition = new Vector2(0, -125);

                            GameObject[] allBullets = GameObject.FindGameObjectsWithTag("TouhouBullet");

                            for (int i = 0; i < allBullets.Length; i++)
                            {
                                // place a point item in that bullet's spot
                                GameObject star = Instantiate(gameObjects[4], allBullets[i].transform.position, Quaternion.identity);
                                star.transform.parent = transforms[2];
                                Destroy(allBullets[i]);
                            }

                            gameObjects[11].GetComponent<TMP_Text>().text = "u prolly should PAYDAY 3: Delivery...        <size=0>;</size>\n<size=32><color=red>Charge! <size=0>;</size>\n</color></size>Heist DLC\n\n\n<color=yellow>(by <color=red>mashing <b><size=16>Spacebar</b></size></color> :)</color>";
                            gameObjects[20].SetActive(true);

                            transforms[4].GetComponent<AudioSource>().Play();

                            floats[10] = 1f;

                            floats[12] = 1f;

                            // move Player to default position
                            // zoom in on Player & show Terry  \(>v<)\.
                            // Terry is charging up a hyper beam, progressing with Player's Power

                            // do the transition

                            gameObjects[14].SetActive(false);
                        }
                        break; // phase 1: evasion
                    case 1:
                        GameObject[] allStars = GameObject.FindGameObjectsWithTag("TouhouStar");

                        for (int i = 0; i < allStars.Length; i++)
                        {
                            if (i >= 48)
                            {
                                Destroy(allStars[i]);
                                continue;
                            }
                            allStars[i].transform.position += Vector3.ClampMagnitude(transforms[0].position - allStars[i].transform.position, 896f * deltaTime);
                            if (Vector3.Distance(transforms[0].position, allStars[i].transform.position) <= 3f && ints[2] < 48)
                            {
                                ints[2]++;
                                Destroy(allStars[i]);
                                gameObjects[12].GetComponentInChildren<TMP_Text>().text = "Shibbi and Pongon's\n    ower Level: " + ints[2] + "/96";
                                gameObjects[13].GetComponent<AudioSource>().Play();
                                // play item acquisition sound
                            }
                        }

                        floats[12] = Mathf.Max(floats[12] - deltaTime, 0f);
                        transforms[2].position = Vector3.Lerp(transforms[5].position, transforms[6].position, curves[2].Evaluate(floats[12]));
                        transforms[2].localScale = Vector3.Lerp(transforms[5].localScale, transforms[6].localScale, curves[2].Evaluate(floats[12]));


                        floats[10] = Mathf.Max(floats[10] - deltaTime, 0f);
                        Vector2 tPos = transforms[4].localPosition;
                        tPos.y = Mathf.Lerp(-124f, -284f, curves[0].Evaluate(floats[10]));
                        transforms[4].localPosition = tPos;

                        transforms[4].GetChild(0).localPosition = new Vector2(Random.Range(-7f, 7f), Random.Range(-14, 0f)) * curves[1].Evaluate((float)ints[2] / 96f);
                        transforms[7].localPosition = new Vector2(-14f, -86f) + new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * curves[1].Evaluate((float)ints[2] / 96f);

                        transforms[8].GetComponent<RectTransform>().sizeDelta = new Vector2(Random.Range(64f, 96f), Random.Range(64f, 96f)) * curves[1].Evaluate((float)ints[2] / 96f);
                        transforms[8].localRotation = Quaternion.Euler(0, 0, Random.Range(-360f, 360f));
                        transforms[8].GetComponent<CanvasGroup>().alpha = curves[1].Evaluate((float)ints[2] / 96f) + Random.Range(-0.1f, 0.05f);


                        gameObjects[12].GetComponent<CanvasGroup>().alpha += deltaTime * 2.5f;

                        // hold power above a threshold for 6 seconds
                        // then win & do the shooting anim
                        break; // phase 2: charging
                    case 2:
                        floats[12] = Mathf.Max(floats[12] - deltaTime * 2f, 0f);
                        transforms[2].position = Vector3.Lerp(transforms[6].position, transforms[5].position, curves[2].Evaluate(floats[12]));
                        transforms[2].localScale = Vector3.Lerp(transforms[6].localScale, transforms[5].localScale, curves[2].Evaluate(floats[12]));

                        gameObjects[19].transform.localPosition = new Vector2(Random.Range(-16f, 16f), -124f + Random.Range(-16f, 16f));
                        gameObjects[19].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-7f, 7f));
                        transforms[1].localRotation = Quaternion.Euler(0, 0, Random.Range(-360f, 360f));
                        break; // stage 3: outro
                }

                break;
        }
    }
    public void startMG()
    {
        gameSpeed = manager.gameSpeed;

        Physics2D.gravity = Vector2.down * 9.81f;

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
                manager.dontKill = true;
                gameObjects[0].GetComponent<AudioSource>().Play();
                break;
            case mcg.Stream:

                gameObjects[12].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[13].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[14].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[15].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[16].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[17].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[18].GetComponent<AudioSource>().pitch = gameSpeed;

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

                gameObjects[15].GetComponent<AudioSource>().Play();

                break;
            case mcg.Quiz:
                gameObjects[4].GetComponent<TMP_Text>().text = "Microgame " + (manager.currentMicrogameIndex + 1) + ":\nWhat is Polygon Donut NOT often called?";

                //string[] randomWrong = new string[] { "Jeanice", "Hollow Knight: Silksong", "Impala 64", "Minecraft Pocket Edition", "Shibbi", "Jorjor Well", "Dolygon Ponut", "Gaster", "Marlok", "Gorn", "Khrobaron", "Dedede", "K. Rool", "Gianni Matragrano", "Thermonuclear Reactor", "Wriggle Nightbug" }; // immediately swap ??? to a different randomly chosen one

                string[] normalAnswers = new string[] { "Polygon Donut", "Pongon", "Mowmow", "pongondonute" };
                string[] wrongAnswers = new string[] { "Poiygon Donut", "Pungon", "Moumow", "pongondonate" };

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
                                if (Random.Range(0, 16) == 3) finalAnswers[i] = "<size=20>Thermonuclear Reactor";
                                else finalAnswers[i] = wrongAnswers[a];
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

                gameObjects[9].GetComponent<AudioSource>().pitch = gameSpeed;
                break;
            case mcg.Shake:
                bools = new bool[2];

                floats = new float[3];
                floats[1] = 0.7f;
                break;
            case mcg.Drive:

                floats = new float[10];
                floats[2] = 0.5f;

                bools = new bool[5];

                for (int i = 0; i < transforms[3].childCount; i++)
                {
                    transforms[3].GetChild(i).localPosition += Vector3.right * Random.Range(-1f, 1f) + transforms[3].GetChild(i).up * Random.Range(-0.5f, 0f);
                    transforms[3].GetChild(i).localScale = Vector3.one * Random.Range(0.85f, 1.2f);
                    transforms[6].GetChild(i).localPosition = transforms[3].GetChild(i).localPosition;
                    transforms[6].GetChild(i).localScale = transforms[3].GetChild(i).localScale;
                }

                print("<color=red><b>SET SPEED AND START ALL ANIMATORS (interior)");

                gameObjects[0].GetComponent<Animator>().speed = gameSpeed;
                gameObjects[0].GetComponent<Animator>().enabled = true;

                float horPongonPos = -4f;
                if (Random.Range(0, 2) == 0)
                {
                    floats[5] = 0.7f;
                }
                else
                {
                    horPongonPos = 4f;
                    floats[5] = 0.3f;
                }

                gameObjects[2].transform.localPosition = new Vector3(-10.01f, -horPongonPos, 0f);
                gameObjects[3].transform.localPosition = new Vector3(-10.01f, -horPongonPos, 0f);

                gameObjects[5].transform.localRotation = gameObjects[4].transform.localRotation;

                break;
            case mcg.LetIn:

                //Random.seed = System.DateTime.Now.Millisecond;

                ints = new int[5];
                ints[1] = Random.Range(0, 2) + 1; // 1: let in; 2: don't let in (if on press current selected option == correct choice (0 will always just show the interrogation text))
                ints[4] = Random.Range(0, 2) + 1;
                //print(new string[] { "Don't let him in if he'll eat all my food, like the last time...", "I invited Pongon for dinner, I'll let him in only if he's hungry..." }[ints[4] - 1]);
                gameObjects[10].SetActive(ints[4] - 1 == 0);
                gameObjects[11].SetActive(ints[4] - 1 == 1);

                floats = new float[1];
                bools = new bool[3];

                strings = new string[2];
                string[] intro = new string[] { "Are you guys going Trick or Treating?", "Hi Shibbi!", "Hello!", "Hi there!", "Hey!", "Hey! Listen!" };
                strings[0] = intro[Random.Range(0, intro.Length)];

                string[] interText = default;
                if (ints[1] == 1)interText = new string[] { "I bought batteries from the store, like you asked!",
                "oops i sneezed on my pineapple",
                "Shibbi I know this is unrelated but I need your help right now.",
                "Would you like to sign my petition on letting non-hungry individuals like me into your house?",
                "Pongon, Pongon, you can call me Pongon. Lime hair, cool cow, please open the door now~",
                "I'm old!",
                "I'm hungry... Not! I'm perpetually aging instead :)",
                "Enough! My ship sails in the morning. I just wonder what you're up to!",
                "Shibbi you done did cook up!... A nice outfit I heard! Can I see it??",
                "I'll steal the frags and destroy you in Unreal. Signed, Polygon Donut.",
                "Pongon Donute, Room Service. Here to be a stereotypical catgirl maid and just look cute doing nothing.",
                "Shibbi gamer in the flesh... Or rather behind a locked door. Wanna play some board games?",
                "Shibbi. Listen carefully. You need my help, and I need your help.",
                "The Moon haunts me.",
                "What a horrible night to have a curse.",
                "The Donut jester does not play, but gently knocks the door. In the court of the Dancing Queeeeeeeeeeeeeeeeen, aaa aaaaahh...",
                "I just bought this game from Gamerhalt! It's called: GD Colon Gets Killed Behind a Convenience Store in Super Tokyo at 3:46 AM, Reincarnates as a Fumo Plushie of Himself and Kills The Supreme God of Hyperdeath!",
                "I was just wondering if you wanted to HANG out with me and play with touys and fill our brains with youtube SHORTS and play Marlok the Wizard for the PC?",
                "im SOOOOO bored",
                "I have some silly toys and board games to review! Wanna help?"}; // not food
                else interText = new string[] { "I bought cashews from the store, but i ate them on the way and i'm still hungry!",
                "oops i sneezed on my pineapple... haha jk, I'm hungry!",
                "Shibbi I know this is unrelated but I need your food right now.",
                "Would you like to sign my petition on letting hungry individuals like me into your house?",
                "Pongon, Pongon, you can call me Pongon. Lime hair, peckish, you should let me in, please~",
                "I'm old... Not! I'm hungry instead :)",
                "I'm very hungry!",
                "Enough! My ship sails in the morning. I wonder what's for dinner...",
                "Shibbi you done did cook up!... A stew, I smell?.. Can I have some??",
                "I'll eat all the food and destroy your fridge. Signed, Polygon Donut.",
                "Pongon Donute, Dish Cleaning Service. Here to clean the dinner from your plates.",
                "Shibbi's dinner in the flesh... Or rather behind a locked door.",
                "Shibbi. Listen carefully. You need my help, and I need your dinner.",
                "The hunger haunts me...",
                "What a horrible night to have a hunger.",
                "The Donut jester does not play, but gently knocks the door. In the court of the Dinner Queeeeeeeeeeeeeeeeen, huu uungryy...",
                "I just bought this game from Gamerhalt! It's called: Shibbi Lets Pongon In For Dinner 3!",
                "I was just wondering if you wanted to HANG out with me and let me eat all your food and fill our bellies with DIET cola and play Marlok the Wizard for the PC?",
                "im SOOOOO hungry",
                "I have some silly toys and all your food to \"review\"! Wanna help?" }; // food
                strings[1] = interText[Random.Range(0, interText.Length)]; // interrogation text

                // strings[1] = "I just bought this game from Gamerhalt! It's called: GD Colon Gets Killed Behind a Convenience Store in Super Tokyo at 3:46 AM, Reincarnates as a Fumo Plushie of Himself and Kills The Supreme God of Hyperdeath!";

                audioPlay(gameObjects[3].GetComponentInChildren<AudioSource>());

                gameObjects[6].GetComponent<Animator>().Play(0);
                break;
            case mcg.Chase:

                //bools = new bool[1];

                floats = new float[2];
                //floats[2] = -1f;

                gameObjects[0].GetComponent<Animator>().speed = gameSpeed;

                transforms[0].GetComponentInParent<AnimationFunctions>().speed *= gameSpeed;
                transforms[0].GetComponentInParent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1f, 0f), Random.Range(-1f, 1f)) * transforms[0].GetComponentInParent<AnimationFunctions>().speed;
                transforms[0].GetComponentInParent<AnimationFunctions>().enabled = true;

                gameObjects[3].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[3].SetActive(true);
                break;
            case mcg.Pray:

                floats = new float[3];

                bools = new bool[4];

                gameObjects[0].GetComponent<Animator>().speed = gameSpeed;
                transforms[0].GetComponent<AudioSource>().pitch = gameSpeed;
                transforms[0].GetComponent<AudioSource>().Play();

                break;
            case mcg.Lag:

                floats = new float[2];
                floats[1] = -1;

                bools = new bool[2];

                gameObjects[0].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[0].GetComponent<AudioSource>().Play(); // could just not do that & move it to regular parent but whatev idc

                gameObjects[3].GetComponent<AudioSource>().pitch = 0.8f * gameSpeed;

                break;
            case mcg.Dance:

                floats = new float[2];
                floats[0] = 1f;

                bools = new bool[3];
                bools[0] = true;

                ints = new int[5];
                ints[1] = Random.Range(0, 4);

                switch (ints[1])
                {
                    case 0:
                        gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObjects[4].GetComponent<Image>().color = new Color32(255, 224, 0, 255);
                        break; // up
                    case 1:
                        gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 180);
                        gameObjects[4].GetComponent<Image>().color = new Color32(96, 255, 0, 255);
                        break; // down
                    case 2:
                        gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 90);
                        gameObjects[4].GetComponent<Image>().color = new Color32(0, 224, 255, 255);
                        break; // left
                    case 3:
                        gameObjects[1].transform.localRotation = Quaternion.Euler(0, 0, 270);
                        gameObjects[4].GetComponent<Image>().color = new Color32(255, 0, 128, 255);
                        break; // right
                }
                gameObjects[1].transform.localPosition = new Vector2(-284f, -220f);
                gameObjects[1].SetActive(true);

                gameObjects[0].GetComponent<Animator>().speed = gameSpeed;
                gameObjects[0].GetComponent<Animator>().enabled = true;

                gameObjects[6].GetComponent<Animator>().speed = gameSpeed;
                gameObjects[6].GetComponent<Animator>().enabled = true;

                gameObjects[2].transform.GetChild(1).GetComponent<Animator>().speed = gameSpeed;
                gameObjects[2].transform.GetChild(2).GetComponent<Animator>().speed = gameSpeed;

                gameObjects[9].GetComponent<Animator>().speed = gameSpeed;
                gameObjects[10].GetComponent<Animator>().speed = gameSpeed;

                gameObjects[11].GetComponent<AudioSource>().pitch = gameSpeed;

                break;
            case mcg.Yap:

                ints = new int[2];

                floats = new float[4];

                floats[2] = 3f;

                bools = new bool[1];

                for (int i = 0; i < 11; i++)
                {
                    gameObjects[i].GetComponent<AudioSource>().pitch = gameSpeed;
                }

                break;
            case mcg.Boss0:

                gameObjects[11].GetComponent<TMP_Text>().text = "you probably should metal gear...             <size=0>;\r\n<size=32><color=red>Survive!";

                GameObject[] allBullets = GameObject.FindGameObjectsWithTag("TouhouBullet");
                for (int i = 0; i < allBullets.Length; i++)
                {
                    Destroy(allBullets[i]);
                }

                gameObjects[14].GetComponent<TMP_Text>().color = Color.white;
                gameObjects[14].GetComponent<TMP_Text>().text = "40";

                transforms[2].position = transforms[6].position;
                transforms[2].localScale = transforms[6].localScale;

                gameObjects[12].GetComponentInChildren<TMP_Text>().text = "Shibbi and Pongon's\n    ower Level: 0/96";

                gameObjects[10].GetComponent<CanvasGroup>().alpha = 1;

                transforms[0].localPosition = new Vector2(0, -125);
                transforms[0].gameObject.SetActive(true);

                transforms[8].GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                transforms[8].GetComponent<CanvasGroup>().alpha = 0;

                transforms[4].localPosition = new Vector2(0, -284f);

                floats = new float[13];
                floats[9] = 3f;

                ints = new int[3];
                ints[0] = 5;

                for (int i = 0; i < 5; i++)
                {
                    gameObjects[5 + i].SetActive(true);
                }

                bools = new bool[1];
                bools[0] = true;

                gameObjects[20].SetActive(false);

                gameObjects[12].GetComponent<CanvasGroup>().alpha = 0;

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

                        audioPlay(gameObjects[8].GetComponent<AudioSource>());
                    } // success
                    else
                    {
                        bools[2] = true;
                        gameObjects[6].SetActive(true);
                        audioPlay(gameObjects[10].GetComponent<AudioSource>());
                    } // miss
                }
                break;
            case mcg.Stream:
                if (inputName == "Space")
                {
                    bools[0] = true;

                    gameObjects[12].GetComponent<AudioSource>().Play();

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
                if (bools[0]) break;
                if (inputName == "Movement")
                {
                    //print(obj.action.ReadValue<Vector2>());

                    if (bools[3] != mode && mode)
                    {
                        gameObjects[9].GetComponent<AudioSource>().Play();
                        ints[0] = (int)Mathf.Repeat(ints[0] - Mathf.Sign(obj.action.ReadValue<Vector2>().y), 4);

                        // update visual

                        for (int i = 0; i < 4; i++)
                        {
                            gameObjects[i].transform.GetChild(1).GetComponent<Image>().enabled = (i == ints[0]);
                        }
                    }

                    bools[3] = mode;
                }
                else if (inputName == "Space" && mode)
                {
                    bools[0] = true;
                    manager.toggleWin(ints[0] == ints[1]);
                    gameObjects[5].SetActive(ints[0] == ints[1]);
                    gameObjects[6].SetActive(ints[0] != ints[1]);

                    gameObjects[ints[0]].GetComponentInChildren<TMP_Text>().color = Color.red;
                    gameObjects[ints[1]].GetComponentInChildren<TMP_Text>().color = Color.green;

                    // mark selector as correct / wrong (dependent on chosen option)
                    // highlight correct answer's text

                    if (ints[0] == ints[1]) audioPlay(gameObjects[7].GetComponent<AudioSource>());
                    else audioPlay(gameObjects[8].GetComponent<AudioSource>());

                    manager.lowerTimer(3);
                }
                break;
            case mcg.Shake:
                if (bools[1] || floats[1] != 0) break;
                if (inputName == "LClick")
                {
                    bools[0] = mode;

                    gameObjects[5].SetActive(!mode);

                    gameObjects[7].GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f) * gameSpeed; // must be done this way cause randomized pitch
                    gameObjects[7].GetComponent<AudioSource>().Play();
                }
                else if (inputName == "MouseMove" && bools[0])
                {
                    float addAmount = Mathf.Abs(obj.action.ReadValue<Vector2>().y) * 0.1f;

                    if (transforms[0].position.y == transforms[1].position.y || transforms[0].position.y == transforms[2].position.y) addAmount *= 0.1f;

                    floats[0] = Mathf.Min(floats[0] + addAmount, 1250f);

                    Vector3 shakerPos = transforms[0].position + Vector3.up * obj.action.ReadValue<Vector2>().y * 0.15f;
                    shakerPos.y = Mathf.Clamp(shakerPos.y, transforms[1].position.y, transforms[2].position.y);
                    transforms[0].position = shakerPos;
                    transforms[3].position = shakerPos;
                    // move parent of both shaker parts on y with mouse input, but clamped

                    if (floats[0] == 1250f)
                    {
                        bools[1] = true;
                        manager.toggleWin(true);
                        manager.lowerTimer(3);

                        gameObjects[5].SetActive(false);

                        transforms[0].position = transforms[8].position;
                        transforms[3].position = transforms[8].position; // reset shaker to original position

                        gameObjects[2].GetComponent<Image>().enabled = true;
                        gameObjects[3].GetComponent<Image>().enabled = true;

                        audioPlay(gameObjects[6].GetComponent<AudioSource>());

                        floats[2] = 0.85f;
                    } // success
                }
                break;
            case mcg.Drive:

                if (inputName == "LClick")
                {
                    bools[0] = mode;
                    if (!mode)
                    {
                        floats[3] = 0.5f;
                        floats[4] = floats[0];
                    }
                }
                else if (inputName == "MouseMove" && bools[0])
                {
                    float xMove = obj.action.ReadValue<Vector2>().x;

                    floats[0] = Mathf.Clamp(floats[0] + xMove * 0.0006f, 0f, 1f);
                }
                else if (inputName == "Space" && mode) audioPlay(gameObjects[8].GetComponent<AudioSource>());

                // mouse for movement

                break;
            case mcg.LetIn:
                if (bools[1]) return;

                if (!mode) bools[2] = false;

                if (mode)
                {
                    if (inputName == "Movement" && !bools[2])
                    {
                        audioPlay(gameObjects[4].GetComponent<AudioSource>());

                        bools[2] = true;
                        Vector2 arrowVec = obj.ReadValue<Vector2>();
                        if (arrowVec.x != 0)
                        {
                            if (ints[0] != 0)
                            {
                                ints[0]++;
                                if (ints[0] > 2) ints[0] = 1;
                            }
                        } // horizontal
                        else
                        {
                            if (ints[0] == 0) ints[0] = 1;
                            else ints[0] = 0;
                        }// vertical

                        for (int i = 0; i < 3; i++)
                        {
                            //gameObjects[i].SetActive(i == ints[0]);
                            gameObjects[i].transform.localScale = Vector3.one;
                            gameObjects[i].GetComponentInChildren<Image>().color = new Color32(0, 0, 0, 224);
                            if (i == ints[0])
                            {
                                gameObjects[i].transform.localScale = Vector3.one * 1.1f;
                                gameObjects[i].GetComponentInChildren<Image>().color = new Color32(32, 32, 32, 240);
                            }
                        }
                    }
                    else if (inputName == "Space")
                    {
                        audioPlay(gameObjects[5].GetComponent<AudioSource>());

                        // choice dependent stuff

                        if (ints[0] == 0)
                        {
                            bools[0] = true;

                            strings[0] = strings[1];
                            gameObjects[3].GetComponent<TMP_Text>().text = default;
                            ints[2] = 0;
                            ints[3] = 0;
                            floats[0] = 0.16f / 3f;
                            audioPlay(gameObjects[3].GetComponentInChildren<AudioSource>());
                            gameObjects[6].GetComponent<Animator>().Play(0);
                        } // interrogation
                        else
                        {
                            bools[1] = true;

                            if (ints[0] == 2)
                            {
                                strings[0] = new string[] { "I guess that's a no...", "I suppose that's a no." }[Random.Range(0, 2)];
                                gameObjects[6].SetActive(false);
                                gameObjects[7].SetActive(true);
                                audioPlay(gameObjects[8].GetComponent<AudioSource>());
                                audioPlay(gameObjects[9].GetComponent<AudioSource>());
                            } // not letting in; Pongon burns
                            else
                            {
                                strings[0] = new string[] { "Neato!", "Tubular!", "Absolutely tubular!" }[Random.Range(0, 3)];
                                gameObjects[6].SetActive(false);
                                audioPlay(gameObjects[13].GetComponent<AudioSource>());
                            } // letting in

                            gameObjects[3].GetComponent<TMP_Text>().text = default;
                            ints[2] = 0;
                            ints[3] = 0;
                            floats[0] = 0.16f / 3f;
                            audioPlay(gameObjects[3].GetComponentInChildren<AudioSource>());

                            if (!bools[0])
                            {
                                print("Failure! (no interrogation)");
                                manager.toggleWin(false);
                                manager.lowerTimer(4);
                                if (ints[4] == 1 && ints[0] == 1) audioPlay(gameObjects[12].GetComponent<AudioSource>());
                            } // Failure (didn't interrogate)
                            else
                            {
                                if (ints[1] == ints[4] && ints[0] == 1 || ints[1] != ints[4] && ints[0] == 2)
                                {
                                    print("Success!");
                                    manager.toggleWin(true);
                                    manager.lowerTimer(4);
                                    if (ints[4] == 2 && ints[0] == 1) audioPlay(gameObjects[12].GetComponent<AudioSource>());
                                } // Success (Player's choice is Shibbi's choice)
                                else
                                {
                                    print("Failure!");
                                    manager.toggleWin(false);
                                    manager.lowerTimer(4);
                                    if (ints[4] == 1 && ints[0] == 1) audioPlay(gameObjects[12].GetComponent<AudioSource>());
                                } // Failure (wrong choice)
                            }
                        } // choice
                    }
                }
                break;
            case mcg.Chase:
                if (inputName == "Movement")
                {
                    gameObjects[1].SetActive(false);
                    floats[0] = obj.action.ReadValue<Vector2>().x;
                    floats[1] = obj.action.ReadValue<Vector2>().y;
                } // just move Shibbi
                break;
            case mcg.Pray:

                if (bools[3]) return;
                if (inputName == "Space") bools[2] = mode;

                break;
            case mcg.Lag:
                if (inputName == "Space" && !bools[0] && floats[0] >= 1.633f && mode)
                {
                    bools[0] = true;
                    gameObjects[2].SetActive(false);

                    if (floats[0] >= 2.757f && floats[0] <= 2.957f)
                    {
                        bools[1] = true;
                        manager.toggleWin(true);
                        print("Success!");
                        gameObjects[3].SetActive(true);
                    } // Success
                    else
                    {
                        floats[1] = 2f;
                        manager.toggleWin(false);
                        manager.lowerTimer(3);
                    } // Failure (missed)
                }
                break;
            case mcg.Dance:
                if (inputName == "Movement" && !bools[1])
                {
                    if (mode && !bools[2])
                    {
                        bools[2] = true;
                        print("YEAH");
                        Vector2 inp = obj.action.ReadValue<Vector2>();
                        bool direCheck = (Vector2.Dot(inp, Vector2.up) >= 0.9f && ints[1] == 0 || Vector2.Dot(inp, Vector2.down) >= 0.9f && ints[1] == 1 || Vector2.Dot(inp, Vector2.left) >= 0.9f && ints[1] == 2 || Vector2.Dot(inp, Vector2.right) >= 0.9f && ints[1] == 3);

                        // print(Vector2.Dot(Vector2.up, Vector2.up) + " dot");

                        if (direCheck && bools[0] && floats[0] <= 0.25f)
                        {
                            gameObjects[1].SetActive(false);
                            if (ints[4] != 3)
                            {
                                bools[0] = false;
                                print("bump...");
                                ints[4]++;
                                // bump

                                for (int i = 0; i < gameObjects[0].transform.childCount; i++)
                                {
                                    gameObjects[0].transform.GetChild(i).gameObject.SetActive(false);
                                }
                                gameObjects[0].transform.GetChild(ints[1] + 2).gameObject.SetActive(true);

                                floats[1] = 0.5f;

                                audioPlay(gameObjects[7].GetComponent<AudioSource>());
                            }
                            else
                            {
                                print("Success");
                                manager.toggleWin(true);
                                manager.lowerTimer(5); // 5 because fortnite dance

                                gameObjects[0].SetActive(false);
                                gameObjects[3].GetComponent<Animator>().speed = gameSpeed;
                                gameObjects[3].SetActive(true);

                                // swap Shibbi sprite

                                bools[1] = true;

                                gameObjects[2].transform.GetChild(0).gameObject.SetActive(false);
                                gameObjects[2].transform.GetChild(1).gameObject.SetActive(true);

                                audioPlay(gameObjects[7].GetComponent<AudioSource>());
                                audioPlay(gameObjects[8].GetComponent<AudioSource>());

                                gameObjects[9].GetComponent<Animator>().enabled = true;
                                gameObjects[10].GetComponent<Animator>().enabled = true;

                                gameObjects[11].GetComponent<AudioSource>().Play();
                                GetComponent<AudioSource>().volume = 0.5f;

                                break;
                            } // victory
                        }
                        else
                        {
                            for (int i = 0; i < gameObjects[0].transform.childCount; i++)
                            {
                                gameObjects[0].transform.GetChild(i).gameObject.SetActive(false);
                            }
                            gameObjects[0].transform.GetChild(1).gameObject.SetActive(true);
                            gameObjects[5].SetActive(true);

                            print("Failure (miss)");
                            manager.toggleWin(false);
                            manager.lowerTimer(3);
                            bools[1] = true;

                            gameObjects[1].SetActive(false);

                            gameObjects[2].transform.GetChild(0).gameObject.SetActive(false);
                            gameObjects[2].transform.GetChild(2).gameObject.SetActive(true);

                            gameObjects[0].GetComponent<Animator>().speed = 0;

                            break;
                        } // failure
                    }
                    else if (!mode) bools[2] = false;
                }
                break;
            case mcg.Yap:
                if (inputName == "Any" && mode && !bools[0])
                {
                    if (floats[1] == 0)
                    {
                        ints[1]++;

                        //print(gameObjects[11].GetComponent<RectMask2D>().padding);
                        gameObjects[11].GetComponent<RectMask2D>().padding = new Vector4(0, 0, 565f * (1f - (ints[1] / 48f)), 0);

                        if (ints[1] == 48)
                        {
                            bools[0] = true;
                            manager.toggleWin(true);
                            manager.lowerTimer(3.5f);

                            transforms[0].localPosition = new Vector2(transforms[0].localPosition.x, 33.1f);

                            gameObjects[13].SetActive(false);
                            gameObjects[14].SetActive(false);
                            gameObjects[15].GetComponent<AudioSource>().pitch = gameSpeed;
                            gameObjects[15].GetComponent<Animator>().speed = gameSpeed;
                            gameObjects[15].SetActive(true);

                            floats[3] = 0.85f;
                        } // success!
                        else
                        {
                            //print("ahh! " + mode);
                            floats[1] = 0.06f;
                            floats[0] = 0.15f;
                            int randChance = Random.Range(0, 42);
                            if (randChance == 6 || randChance == 7) gameObjects[10].GetComponent<AudioSource>().Play();
                            else gameObjects[ints[0]].GetComponent<AudioSource>().Play();
                            ints[0] = (int)Mathf.Repeat(ints[0] + 1, 10);
                        } // continue
                    }
                }
                break;
            case mcg.Boss0:
                if (inputName == "Movement" && ints[1] == 0)
                {
                    floats[0] = 0;
                    floats[1] = 0;

                    if (obj.action.ReadValue<Vector2>().x != 0) floats[0] = Mathf.Sign(obj.action.ReadValue<Vector2>().x);
                    if (obj.action.ReadValue<Vector2>().y != 0) floats[1] = Mathf.Sign(obj.action.ReadValue<Vector2>().y);

                } // Dodge
                else if (mode && inputName == "Space" && ints[1] == 1)
                {
                    if (ints[2] < 96)
                    {
                        ints[2]++;
                        gameObjects[12].GetComponentInChildren<TMP_Text>().text = "Shibbi and Pongon's\n    ower Level: " + ints[2] + "/96";
                        gameObjects[13].GetComponent<AudioSource>().Play();
                        if (ints[2] == 96)
                        {
                            gameObjects[12].GetComponentInChildren<TMP_Text>().text = "Shibbi and Pongon's\n    ower Level: FULL!/96";
                            print("Victory!");
                            gameObjects[11].GetComponent<TMP_Text>().text = "you definitely should                                <size=0>;</size>\n<size=32><color=red>Nitori Climb!   <size=0>;</size>\n</color></size>right no-wait what!?";
                            manager.toggleWin(true);
                            // run the cutscene

                            floats[12] = 1f;
                            ints[1] = 2; // stage 3: literally just outro anim lmao

                            gameObjects[12].SetActive(false);
                            gameObjects[18].SetActive(true);
                            gameObjects[19].SetActive(true);

                            gameObjects[20].SetActive(false);

                            transforms[8].gameObject.SetActive(false);

                            manager.wasBossDefeated = true;
                            manager.microgameTimer = 5; // to add time in case Player is (somehow?) running out
                            manager.lowerTimer(5);
                        } // Success!
                    }
                } // Charge
                break;
        }
    }
    public void doWin()
    {
        switch (microgameType)
        {
            case mcg.Chase:
                manager.toggleWin(true);
                manager.lowerTimer(2);
                gameObjects[2].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[2].GetComponentInChildren<Animator>().speed = gameSpeed;
                gameObjects[2].SetActive(true);
                gameObjects[3].SetActive(false);
                break;
        }
    }
    public MicrogameManager getManager()
    {
        return manager;
    }
    void spawnBullet(GameObject input, int quantity)
    {
        //print(quantity);
        float randomAngle = Random.Range(-360f, 360f);
        for (int i = 0; i < quantity; i++)
        {
            GameObject newBullets = Instantiate(input, transforms[1].position, transforms[1].rotation);
            newBullets.transform.parent = transforms[2];
            newBullets.transform.localRotation = Quaternion.Euler(0, 0, randomAngle + Mathf.Lerp(0f, 360f, (float)i / (float)quantity));
            newBullets.transform.SetAsLastSibling();
            Destroy(newBullets, 10);
            newBullets.SetActive(true);
        }
    }
    void audioPlay(AudioSource input, float pitch = 1f)
    {
        input.pitch = pitch * gameSpeed;
        input.Play();
    }
}
