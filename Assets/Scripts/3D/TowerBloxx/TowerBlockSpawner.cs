using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TowerBlockSpawner : MonoBehaviour
{
    // Line Renderer
    [SerializeField] private Transform startLinePoint;
    [SerializeField] private LineRenderer lr;
    // -------------

    // Pendulim var
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float amplitude = 30;
    private Transform startOfPendulum;
    // ------------

    // Other GameObjects
    [SerializeField] private GameObject TowerBlockPref;
    // -----------------

    // Lifter
    [SerializeField] private Transform lifter;
    public float distanceToMove;
    // ------

    [HideInInspector] public List<TowerBlock> towerBlocks = new List<TowerBlock>();

    public int health = 3;

    private void Awake()
	{
        startOfPendulum = transform.parent.transform;
        lr.positionCount = 2;
        InstantiateTowerBlock(true);
	}

    private void Update()
    {
        Debug.Log("Count: " + towerBlocks.Count);

        // Line Rendering
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, startLinePoint.position);
        // --------------

        // Pendulum simulation
        startOfPendulum.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sin(Time.time * rotationSpeed) * amplitude));
        // ------------------

        if (health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InstantiateTowerBlock(bool isFirstCube)
	{
        TowerBlock towerBlock = Instantiate(TowerBlockPref, transform.position, Quaternion.identity).GetComponent<TowerBlock>();
        towerBlock.GetComponent<Rigidbody>().useGravity = false;
        if (isFirstCube) towerBlock.isFirstCube = true;
        towerBlock.tbSpawner = this;
        towerBlock.cranePos = transform;
	}

    public IEnumerator Lift(float distance)
    {
        Debug.Log("Lift");
        float endPos = lifter.position.y + distance;
        while (Mathf.Abs(endPos - lifter.position.y) > 0.02f)
        {
            lifter.position = Vector3.MoveTowards(lifter.position, lifter.position + Vector3.up * distance, Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
