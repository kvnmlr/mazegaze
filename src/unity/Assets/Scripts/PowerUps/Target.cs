using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PowerUp {

    int numGame;

    public override IEnumerator performPowerUp(Player p)
    {
        p.points++;
        yield return new WaitForSeconds(1);
    }

    void Start()
    {  
        numGame = 0;
        this.type = PowerUpManager.PowerUpTypes.Target;
    }

    void Update()
    {
        
    }

    public Vector3 TransformpositionRandom()
    {
        float xSize = MazeGenerator.Instance.xSize;
        float ySize = MazeGenerator.Instance.ySize;
        float rdx = Random.Range(-(xSize - 1) / 2, (xSize + 1) / 2);
        float rdz = Random.Range(-(ySize - 1) / 2, (ySize + 1) / 2);
        return new Vector3(rdx + 0.5f, 0.5f, rdz);
    }

   
    
}
