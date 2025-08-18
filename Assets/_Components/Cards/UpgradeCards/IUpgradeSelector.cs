using System.Collections.Generic;
using System;
using System.Collections;

public interface IUpgradeSelector
{
    // Bir seçim turu için kartlar verilir; seçilen kart callback ile döndürülür.
    IEnumerator Select(Owner owner, List<UpgradeCardData> cards, Action<UpgradeCardData> onSelected);
}
