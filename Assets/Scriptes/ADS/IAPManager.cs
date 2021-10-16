using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private const string RemoveAds = "snakes.way.rush.NoAds";

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == RemoveAds)
        {
            Singleton<AdSettings>.Instance.RemoveAds();
        }
        else
        {
            Debug.LogError("Cant't find " + product.definition.id);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError(product.definition.id + " failed because " + failureReason);
    }
}
