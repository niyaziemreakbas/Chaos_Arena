using System.Collections.Generic;
using System;
using System.Collections;

public interface IUpgradeSelector
{
    // Bir se�im turu i�in kartlar verilir; se�ilen kart callback ile d�nd�r�l�r.
    IEnumerator Select(Owner owner, List<UpgradeCardData> cards, Action<UpgradeCardData> onSelected);
}
