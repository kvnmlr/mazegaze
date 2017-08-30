using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public Cell cell { get; set; }
    public PowerUpManager.PowerUpTypes type { get; set; }
    public abstract IEnumerator performPowerUp(Player p);

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 100, Space.World);


        //adjust this to change speed
        float speed = 5f;
        //adjust this to change how high it goes
        float height = 0.2f;

        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.localPosition;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        transform.localPosition = new Vector3(pos.x, newY, pos.z) * height;
    }

    void Start()
    {
        this.cell = transform.parent.gameObject.GetComponent<Cell>();
    }

    public void activate(Player p)
    {
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
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            activate(p);
            PowerUpManager.Instance.activePowerUps--;
        }
    }

    public int[] findCell()
    {
        GameObject[][] board = MazeGenerator.Instance.toMatrix();

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

        int[] res = new int[2];
        res[0] = posY;
        res[1] = posX;
        return res;

    }
}
