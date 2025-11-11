using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class microgame
{
    public string name, altName;
    public GameObject ownGO;
    public float microgameTime;

    [Header("0: Keyboard;\n1: Arrows;\n2: Space;\n3: Mouse")]
    public bool[] controls = new bool[4] { false, false, false, false };

    [Header("Microgame-specific parameters")]
    public string[] stringVars;
    public int[] intVars;
    public float[] floatVars;
    public bool[] boolVars;
    public Transform[] transforms;

    [Space]
    public bool isEPG;

    public microgame clone()
    {
        microgame output = new microgame();

        output.name = name;
        output.altName = altName;
        output.ownGO = ownGO;
        output.microgameTime = microgameTime;

        output.controls = new bool[controls.Length]; // controls
        for (int i = 0; i < controls.Length; i++)
        {
            output.controls[i] = controls[i];
        }

        output.stringVars = new string[stringVars.Length]; // stringVars
        for (int i = 0; i < stringVars.Length; i++)
        {
            output.stringVars[i] = stringVars[i];
        }
        output.intVars = new int[intVars.Length]; // intVars
        for (int i = 0; i < intVars.Length; i++)
        {
            output.intVars[i] = intVars[i];
        }
        output.floatVars = new float[floatVars.Length]; // floatVars
        for (int i = 0; i < floatVars.Length; i++)
        {
            output.floatVars[i] = floatVars[i];
        }
        output.boolVars = new bool[boolVars.Length]; // boolVars
        for (int i = 0; i < boolVars.Length; i++)
        {
            output.boolVars[i] = boolVars[i];
        }
        output.transforms = new Transform[transforms.Length]; // transforms
        for (int i = 0; i < transforms.Length; i++)
        {
            output.transforms[i] = transforms[i];
        }

        output.isEPG = isEPG;

        return output;
    }
}
public class MicrogameManager : MonoBehaviour
{
    public int lifes;

    [Space]
    public int currentMicrogameIndex;
    [Tooltip("swap the Evil Polygon Donut microgame with this index (so this microgame is always at this index no matter what)")]
    public int evilPGIndex;
    [Tooltip("after which microgame will the speedup occur")]
    public int[] speedUpIndexes;

    public microgame[] microgameList;
    public microgame boss;

    [Space]
    public microgame[] microgames;

    [Space]
    public bool winState;
    public float gameSpeed = 1f, microgameTimer, speedMult = 1.1f;
    private float bpmSpeed = 12f;

    public GameObject[] controlIcons;
    public TMP_Text gameName, stageCount;

    public bool devMode;

    [Space]
    public float transAnimTime, transAnimProgress, transAnimDire = -1f;
    private float lastTAP;
    public Transform bgTransform;
    public CanvasGroup[] cGroups, cGroupsFaster;
    public AnimationCurve moveCurve, scaleCurve, alphaCurve;
    public Transform[] moveLerpPoints, stCountLerpPoints;

    [Space]
    public Image timer;
    public Sprite[] timerSprites;
    private int lastTimer;

    [Space]
    public float stageUpTimer = 1.5f;
    public AnimationCurve stageUpCurve, gnScaleCurve, gnScaleCurve2, gnAlphaCurve;
    public CanvasGroup gameNameAlpha;

    [Space]
    public float resultTimer;
    public Sprite[] resSprites;
    public Image resImage;

    [Space]
    public float speedUpTimer;
    public CanvasGroup speedVis0;
    public Transform speedVis1, speedVisSpeed, speedVisUp;
    public AnimationCurve speedVisCurve, speedVisAlphaCurve;
    public Transform[] spv1Points, spvsPoints, spvuPoints;

    [Space]
    public float bossTimer;

    [Space]
    public float controlsTimer;
    public AnimationCurve cCurve;
    public Transform controlsThing;
    public Transform[] cPositions;

    private float initialWait = 2f;

    private void Awake()
    {
        if (!devMode) buildMGList();
        else
        {
            microgames = cloneMGArray(microgameList);
            playMicrogame(currentMicrogameIndex);
            transAnimProgress = transAnimTime;
        }
        bgTransform.gameObject.SetActive(true);
        bpmSpeed = 12f;
        initialWait = 2f;
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime * gameSpeed;

        if (initialWait != 0)
        {
            initialWait = Mathf.Max(initialWait - deltaTime, 0f);
            return;
        }

        if (controlsTimer != 0)
        {
            controlsTimer = Mathf.Max(controlsTimer - deltaTime, 0f);

            float cMult = 1f - (controlsTimer * 0.4f); // multiplying by 0.4 is cheaper than dividing by 2.5, and it yields the same result anyway

            controlsThing.localScale = Vector3.Lerp(cPositions[0].localScale, cPositions[1].localScale, cCurve.Evaluate(cMult));
            controlsThing.position = Vector3.Lerp(cPositions[0].position, cPositions[1].position, cCurve.Evaluate(cMult));

            if (controlsTimer == 0)
            {
                gameName.enabled = true;
                gameName.transform.localScale = Vector3.one * gnScaleCurve.Evaluate(0);
            }

            return;
        }

        if (bossTimer != 0)
        {
            bossTimer = Mathf.Max(bossTimer - deltaTime, 0f);

            if (bossTimer == 0) controlsTimer = 2.5f;

            return;
        }

        if (speedUpTimer != 0)
        {
            speedUpTimer = Mathf.Max(speedUpTimer - deltaTime, 0f);

            float suMult = 0f;
            if (speedUpTimer != 0) suMult = speedUpTimer / 3f;

            speedVis1.localScale = Vector3.Lerp(spv1Points[0].localScale, spv1Points[1].localScale, speedVisCurve.Evaluate(suMult));
            speedVisSpeed.position = Vector3.Lerp(spvsPoints[0].position, spvsPoints[1].position, speedVisCurve.Evaluate(suMult));
            speedVisUp.position = Vector3.Lerp(spvuPoints[0].position, spvuPoints[1].position, speedVisCurve.Evaluate(suMult));
            speedVis0.alpha = speedVisAlphaCurve.Evaluate(suMult);

            if (speedUpTimer == 0) controlsTimer = 2.5f;

            return;
        }

        if (resultTimer != 0)
        {
            resultTimer = Mathf.Max(resultTimer - deltaTime, 0f);

            if (resultTimer <= 1f) resVisChange(0);

            if (resultTimer == 0)
            {
                for (int i = 0; i < speedUpIndexes.Length; i++)
                {
                    if (currentMicrogameIndex == speedUpIndexes[i])
                    {
                        print("SPEED UP TIME SET");
                        speedUpTimer = 3f;
                        bpmSpeed += 1f;
                        gameSpeed = bpmSpeed / 12f;
                        return;
                    }
                }

                // do the controls time
                controlsTimer = 2.5f;
            }
            return;
        }

        if (stageUpTimer != 0)
        {
            float stagePerc = 1.5f - (stageUpTimer = Mathf.Max(stageUpTimer - deltaTime, 0f));

            stageCount.transform.localScale = Vector3.one * stageUpCurve.Evaluate(stagePerc);
            gameName.transform.localScale = Vector3.one * gnScaleCurve.Evaluate(stagePerc);

            if (stagePerc >= 0.5f && stageCount.text != (currentMicrogameIndex + 1) + "") stageCount.text = (currentMicrogameIndex + 1) + "";
            return;
        }

        if (lastTAP != transAnimProgress)
        {
            lastTAP = transAnimProgress;

            float animPerc = 1f - (transAnimProgress / transAnimTime);
            bgTransform.position = Vector3.Lerp(moveLerpPoints[0].position, moveLerpPoints[1].position, moveCurve.Evaluate(animPerc));
            bgTransform.localScale = Vector3.one * scaleCurve.Evaluate(animPerc);

            stageCount.transform.position = Vector3.Lerp(stCountLerpPoints[0].position, stCountLerpPoints[1].position, moveCurve.Evaluate(animPerc));

            for (int i = 0; i < cGroups.Length; i++)
            {
                cGroups[i].alpha = alphaCurve.Evaluate(animPerc);
            }
            for (int i = 0; i < cGroupsFaster.Length; i++)
            {
                cGroupsFaster[i].alpha = alphaCurve.Evaluate(animPerc * 2f);
            }

            transAnimProgress = Mathf.Clamp(transAnimProgress - deltaTime * transAnimDire, 0f, transAnimTime);

            gameName.transform.localScale = Vector3.one * gnScaleCurve2.Evaluate(animPerc);
            gameNameAlpha.alpha = gnAlphaCurve.Evaluate(animPerc);

            if (transAnimProgress == 0 && transAnimDire == 1f)
            {
                lastTAP = transAnimProgress;
                //print("ENDED!");

                if (microgames[currentMicrogameIndex].ownGO) microgames[currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().startMG();
                bgTransform.gameObject.SetActive(false);
                gameName.enabled = false;
            }
            else if (transAnimProgress == transAnimTime && transAnimDire == -1f)
            {
                timer.enabled = false;
                print("ended; boot the results (take away a life if lost);\n" +
                    "update P&S to idle sprite / panicked (on speed up / boss);\n" +
                    "speed up / boss;\n" +
                    "controls;\n" +
                    "stage number and name");
                // result, speed up / boss stage, controls, stage number and stage name

                transAnimDire = 1f;
                stageUpTimer = 1.5f;
                playMicrogame(currentMicrogameIndex);

                resultTimer = 1.5f;
            }
            return;
        }

        if (microgames[currentMicrogameIndex].ownGO && microgameTimer != 0)
        {
            microgameTimer = Mathf.Max(microgameTimer - deltaTime * gameSpeed, 0f);

            timerVis();

            if (microgameTimer == 0f)
            {
                print("boot back to main scene!");
                if (currentMicrogameIndex < microgames.Length) currentMicrogameIndex++; // stage will move up unless Player is at the Boss Stage
                transAnimDire = -1f;
                transAnimProgress = Mathf.Clamp(transAnimProgress - deltaTime * transAnimDire, 0f, transAnimTime);
                lastTAP = -1f;
                bgTransform.gameObject.SetActive(true);

                if (winState) resVisChange(1);
                else resVisChange(2);
                // do vis based on win status
            }
        }
    }
    void resVisChange(int index)
    {
        // 0 - default
        // 1 - win
        // 2 - loss
        // 3 - faceless (1/666 chance)

        resImage.sprite = resSprites[index];
        if (index != 0 && Random.Range(0, 666) == 9) resImage.sprite = resSprites[3];
    }
    void timerVis()
    {
        if (Mathf.CeilToInt(microgameTimer * 2f) != lastTimer)
        {
            lastTimer = Mathf.CeilToInt(microgameTimer * 2f);

            if (lastTimer < timerSprites.Length)
            {
                timer.enabled = true;
                timer.sprite = timerSprites[timerSprites.Length - lastTimer - 1];
            }
            else timer.enabled = false;

        }
    }
    public void toggleWin(bool input)
    {
        winState = input;

        if (winState)
        {

        } // on toggle win
        else
        {

        } // on toggle failure
    }
    public void playMicrogame(int index)
    {
        winState = false;
        for (int i = 0; i < microgames.Length; i++)
        {
            if (microgames[i].ownGO) microgames[i].ownGO.SetActive(i == index);
        }

        switch (microgames[index].name)
        {
            case "Let in!":
                if (Random.Range(0, 2) == 1)
                {
                    microgames[index].name = microgames[index].altName;
                }
                break; // choose randomly the type (let / dont let in)
            case "Shake!":

                int randChoice = Random.Range(0, 2);
                microgames[index].ownGO.GetComponent<MicrogameScript>().gameObjects[randChoice].SetActive(true); // set character as active
                microgames[index].ownGO.GetComponent<MicrogameScript>().gameObjects[2 + randChoice].SetActive(true); // set drink outcome as active
                break; // choose shake target randomly (Pongon / Shibbi)
        }

        for (int i = 0; i < microgames[index].controls.Length; i++)
        {
            controlIcons[i].SetActive(microgames[index].controls[i]);
        }
        controlIcons[controlIcons.Length - 1].SetActive(microgames[index].isEPG);

        doMicrogameText(microgames[index].name);

        microgameTimer = microgames[index].microgameTime;
    }
    public void doMicrogameText(string input)
    {
        gameName.text = input;
    }
    void buildMGList()
    {
        microgames = shuffleMGs(shuffleMGs(cloneMGArray(microgameList))); // shuffled twice :)

        int epgMicrogameIndex = -1;
        for (int i = 0; i < microgames.Length; i++)
        {
            if (microgames[i].isEPG)
            {
                epgMicrogameIndex = i;
                break;
            }
        }
        microgame epgM = microgames[epgMicrogameIndex];
        microgames[epgMicrogameIndex] = microgames[evilPGIndex];
        microgames[evilPGIndex] = epgM;
    }
    microgame[] shuffleMGs(microgame[] input)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < input.Length; t++)
        {
            microgame tmp = input[t];
            int r = Random.Range(t, input.Length);
            input[t] = input[r];
            input[r] = tmp;
        }
        return input;
    }
    microgame[] cloneMGArray(microgame[] input)
    {
        microgame[] output = new microgame[input.Length];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = microgameList[i].clone();
        }
        return output;
    }
    public void handleInput(InputAction.CallbackContext obj)
    {
        if (transAnimProgress == 0) microgames[currentMicrogameIndex].ownGO.GetComponent<MicrogameScript>().handleInput(obj);
    }
    public void lowerTimer(float amount)
    {
        microgameTimer = Mathf.Min(microgameTimer, amount); // changed because what the hell was that even
    }
}
