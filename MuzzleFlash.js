var muzzle : Rigidbody;


  
function Start()
{

}

function Update()
{
	if (Input.GetButtonDown ("Fire1"))
	{
		clone = Instantiate(muzzle, transform.position, transform.rotation);
		
		
		Destroy (clone.gameObject, 0.05);
	}
}