using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api ;

public class BannerAds : MonoBehaviour
{
    /*
    private BannerView bannerView;
    public string adUnitId = "ca-app-pub-8095700878802270/1628307634";
    public AdPosition adPosition;
    private void Start()
    {
        
        
        bannerView = new BannerView(adUnitId, AdSize.Banner, adPosition);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }
*/
    private BannerView bannerView;
    public string adUnitId = "ca-app-pub-8095700878802270/1628307634";
    public AdPosition adPosition;
    private void Start()
    {
        bannerView = new BannerView(adUnitId, AdSize.Banner, adPosition);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }
    
}
