using TMPro;
using UnityEngine;

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

    public GameObject[] controlIcons;
    public TMP_Text gameName;

    public bool devMode;
    public int devIndex;

    private void Awake()
    {
        if (!devMode) buildMGList();
        else
        {
            microgames = cloneMGArray(microgameList);
            playMicrogame(devIndex);
        }
    }
    public void toggleWin(bool input)
    {
        winState = input;
    }
    public void playMicrogame(int index)
    {
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
                break; // choose shake target randomly (Pongon / Shibbi)
        }

        doMicrogameText(microgames[index].name);
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
}
