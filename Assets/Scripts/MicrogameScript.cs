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
                // GO 7: ...?
                // GO 8: parry sound
                // GO 8: explosion sound
                // GO 8: fail sound

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
                break;
            case mcg.LetIn:
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
                                gameObjects[12].GetComponentInChildren<TMP_Text>().text = "Shibbi and Pongon's\n    ower Level: " + ints[2] + "/128";
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
                bools = new bool[2];

                floats = new float[3];
                floats[1] = 0.7f;
                break;
            case mcg.Drive:
                break;
            case mcg.LetIn:
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
                break;
            case mcg.Lag:

                floats = new float[2];
                floats[1] = -1;

                bools = new bool[2];

                gameObjects[0].GetComponent<AudioSource>().pitch = gameSpeed;
                gameObjects[0].GetComponent<AudioSource>().Play();

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

                gameObjects[12].SetActive(false);

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
                if (bools[0]) break;
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
                    bools[0] = true;
                    manager.toggleWin(ints[0] == ints[1]);
                    gameObjects[5].SetActive(ints[0] == ints[1]);
                    gameObjects[6].SetActive(ints[0] != ints[1]);

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

                    gameObjects[7].GetComponent<AudioSource>().pitch = gameSpeed + Random.Range(-0.2f, 0.2f); // must be
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
                break;
            case mcg.LetIn:
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
    void audioPlay(AudioSource input)
    {
        input.pitch = gameSpeed;
        input.Play();
    }
}
