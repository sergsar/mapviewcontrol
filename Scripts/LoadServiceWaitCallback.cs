using System;
using System.Collections;

namespace MapViewScripts
{
    public delegate void LoadServiceWaitCallback(Func<float, IEnumerator> wait);
}