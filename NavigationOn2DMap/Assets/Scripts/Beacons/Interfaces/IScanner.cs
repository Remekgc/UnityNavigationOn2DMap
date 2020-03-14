using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public interface IScanner
{
    // Functioncs
    void EnableScan();
    void DisableScan();
    IEnumerator Scan();
    void UpdateBeaconList(string scanResult);

}
