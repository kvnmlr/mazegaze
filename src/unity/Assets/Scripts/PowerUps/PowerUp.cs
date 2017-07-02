using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public Cell cell { get; set; }
    public PowerUpManager.PowerUpTypes type { get; set; }
    public abstract IEnumerator performPowerUp(Player p);

    void Start()
    {
        this.cell = transform.parent.gameObject.GetComponent<Cell>();
    }

    public void activate(Player p)
    {
        Debug.Log(p.name + " collected " + type);
        StartCoroutine(performPowerUp(p));
    }

    public void OnTriggerEnter(Collider other)
    {
        this.cell = transform.parent.gameObject.GetComponent<Cell>();
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player p = other.gameObject.GetComponent<Player>();

            if (p == null)
            {
                return;
            }
            activate(p);
            Destroy(gameObject.GetComponent<SphereCollider>());
            Destroy(gameObject.GetComponent<MeshRenderer>());
        }
    }

    public int[] findCell()
    {
        GameObject[][] board = MazeGenerator.Instance.toMatrix(MazeGenerator.Instance.cells);

        // find current cell
        GameObject gCell = cell.gameObject;
        if (gCell.GetComponent<Cell>() == null)
        {
            throw new System.Exception("Power up is not in a cell");
        }

        int posX = 0;
        int posY = 0;

        for (int row = 0; row < board.Length; ++row)
        {
            for (int column = 0; column < board[row].Length; ++column)
            {
                if (board[row][column].Equals(gCell))
                {
                    posX = column;
                    posY = row;
                }
            }
        }

        Debug.Log("PowerUp is at Cell " + posY + ", " + posX);

        int[] res = new int[2];
        res[0] = posY;
        res[1] = posX;
        return res;

    }
}
