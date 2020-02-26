using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSPrototype : MonoBehaviour
{
    public Text text;
    public LocationServiceStatus service;
    public bool locationEnabled;

    private void Start()
    {
        StartCoroutine(BeginLocationChecking());
    }

    private void Update()
    {
        service = Input.location.status;
    }



    IEnumerator BeginLocationChecking()
    {
        yield return new WaitForSeconds(3f);

        text.text = "Getting Data...";

        if (!Input.location.isEnabledByUser)
        {
            print("Location data not enabled.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 10;

        while (Input.location.status != LocationServiceStatus.Running && maxWait > 0)
        {
            print ("Location data not enabled.");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            //print("Timed out");
            print("Timed Out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            while (Input.location.status == LocationServiceStatus.Running)
            {
                print("Update");
                Input.location.Start();
                text.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
                yield return new WaitForSecondsRealtime(.5f);
            }
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();

    }
}
