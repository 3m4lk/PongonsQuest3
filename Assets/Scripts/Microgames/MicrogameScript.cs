using UnityEngine;

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
    public MicrogameManager manager;

    public mcg microgameType;
    public void startMG()
    {
        switch (microgameType)
        {
            case mcg.Null:
                break;
            case mcg.Parry:
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
}
