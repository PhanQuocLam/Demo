using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    List<Orb> orbs;
    Door lockedDoor;
    float gameTime;
    bool gameIsWin;
    //public int orbNum;
    public int deathNumer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (gameIsWin)
            return;
        //orbNum = instance.orbs.Count;
        gameTime += Time.deltaTime;
        UIManager.UpdateTime(gameTime);
    }
    public static void PlayerWin()
    {
        instance.gameIsWin = true;
        UIManager.GameWin();
    }
    public static void PlayerDied()
    {
        instance.deathNumer++;
        instance.Invoke("RestartScene", 1.5f);
        UIManager.UpdateDeathUI(instance.deathNumer);
    }
    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);
        if (instance.orbs.Count == 0)
        {
            instance.lockedDoor.Open();
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
            return;
        if (!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }
    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
