using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{

    public Transform target;
    public Vector3 targetposoffset = new Vector3(0, 3.4f, 0);
    public float looksmooth = 100f;
    public float distancefromtarget = 5;
    public float zoomsmooth = 100;
    public float maxzoom = -2;
    public float minzoom = -15;

    public float xrot = -20;
    public float yrot = -180;
    public float maxXrot = 25;
    public float minXrot = -85;
    public float vorbitsmooth = 150;
    public float horbitsmooth = 150;


    public string ORBIT_HORIZONTAL_SNAP = "ORBITHORIZONTALSNAP";
    public string ORBIT_HORIZONTAL = "ORBITHORIZONTAL";
    public string ORBIT_VERTICAL = "ORBITVERTICAL";
    public string ZOOM = "Mouse ScrollWheel";


    Vector3 targetpos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    float vorbitinput, horbitinput, zoominput, horbitsnapinput;

    // Use this for initialization
    void Start()
    {
        targetposoffset = transform.position;
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTOtaget();
        lookATTARGET();
        getinput();
        orbit();
        ZOOMinout();
    }



    void MoveTOtaget()
    {
        targetpos = target.position + targetposoffset;
        destination = Quaternion.Euler(xrot, yrot + target.eulerAngles.y, 0) * Vector3.forward * distancefromtarget;
        destination += targetposoffset;
        transform.position = destination;
    }


    void lookATTARGET()
    {
        Quaternion targetrotation = Quaternion.LookRotation(targetpos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, looksmooth * Time.deltaTime);
    }

    void getinput()
    {
        vorbitinput = Input.GetAxis(ORBIT_HORIZONTAL);
        horbitinput = Input.GetAxis(ORBIT_HORIZONTAL);
        horbitsnapinput = Input.GetAxis(ORBIT_HORIZONTAL_SNAP);
        zoominput = Input.GetAxis(ZOOM);

    }


    void orbit()
    {
        if (horbitinput > 0)
        {
            yrot = -100;
        }

        xrot += -vorbitinput * vorbitsmooth * Time.deltaTime;
        yrot += -horbitinput * horbitsmooth * Time.deltaTime;

        if (xrot > maxXrot)
            xrot = maxXrot;

        if (xrot < minXrot)
            xrot = minXrot;
        
    }

    void ZOOMinout()
    {
        
    }
}
