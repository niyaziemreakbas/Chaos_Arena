//using FurtleGame.Singleton;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UpgradeFlowController : SingletonMonoBehaviour<UpgradeFlowController>
//{
//    public IEnumerator PerformOneUpgrade(Owner owner)
//    {
//        // 1) Kartlar� �ret
//        var cards = UpgradeManager.Instance.ReturnRandomUpgradeList(owner, 3);
//        if (cards == null || cards.Count == 0)
//            yield break;

//        // 2) Selector�dan se�im iste
//        UpgradeCardData chosen = null;
//        yield return owner.Selector.Select(owner, cards, c => chosen = c);

//        if (chosen == null)
//            yield break;

//        // 3) Uygula (HandleCardUpgrades zaten Owner.OnUpgradePerformedFunc() �a��r�yor)
//        UpgradeManager.Instance.HandleCardUpgrades(chosen, owner);
//    }
//}
