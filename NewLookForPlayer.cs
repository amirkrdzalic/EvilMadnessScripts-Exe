using UnityEngine;
using System.Collections;

public class NewLookForPlayer : MonoBehaviour {

    public Rigidbody player;

	// Use this for initialization
	void Start () {

        player = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector2 v = new Vector2(-Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).z) - new Vector2(-player.transform.position.x, player.transform.position.z);

        float angle = Mathf.Atan2(v.y, v.x);

        player.transform.rotation = Quaternion.AngleAxis((angle * Mathf.Rad2Deg) - 90, new Vector3(0, 1, 0));
	}
}