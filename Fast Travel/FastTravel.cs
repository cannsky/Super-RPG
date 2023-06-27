using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravel
{
    public GameObject fastTravelPoint;
    
    public void TravelToFastTravelPoint(){
        Player.player.gameObject.transform.position = fastTravelPoint.transform.position;
    }
}
