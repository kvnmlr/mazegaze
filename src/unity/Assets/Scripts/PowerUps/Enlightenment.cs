using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightenment : PowerUp {

    public override IEnumerator performPowerUp(Player p)
    {
        int rangeX = MazeGenerator.Instance.xSize / 7;
        int rangeY = MazeGenerator.Instance.ySize / 7;

        int[] pos = findCell();

        GameObject[][] board = MazeGenerator.Instance.toMatrix();

        float timeToDarknessOld = p.timeToDarkness;
        p.timeToDarkness = 3;

        for(int deltaRangeX = rangeX * (-1); deltaRangeX <= rangeX; ++deltaRangeX)
        {
            for (int deltaRangeY = rangeY * (-1); deltaRangeY <= rangeY; ++deltaRangeY)
            {
                if (deltaRangeX + pos[1] < 0 || deltaRangeY + pos[0] < 0 || deltaRangeX + pos[1] >= board[0].Length || deltaRangeY + pos[0] >= board.Length)
                {
                    continue;
                }

                Cell c = board[pos[0]+deltaRangeY][pos[1]+deltaRangeX].GetComponent<Cell>();
                if (c == null)
                {
                    continue;
                }
                c.spawnLight(p);
            }
        }
        yield return new WaitForSeconds(3);
        p.timeToDarkness = timeToDarknessOld;
        gameObject.SetActive(false);

    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Enlightenment;
    }

    void Update()
    {

    }
}
