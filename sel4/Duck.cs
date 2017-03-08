using System;
using System.Drawing;

public class Duck
{
    public string Name;
    public string RegularPrice;
    public string CampaignPrice;
    public string RegularPriceColor;
    public Size RegularPriceSize;
    public string CampaignPriceColor;
    public Size CampaignPriceSize;

    public Duck(string name, string regularPrice, string campaignPrice, string regularPriceAttributes, string campaignPriceAttribute)
	{
        Name = name;
        RegularPrice = regularPrice;
        CampaignPrice = campaignPrice;
        RegularPriceColor = regularPriceAttributes;
        CampaignPriceColor = campaignPriceAttribute;

    }

    public Duck()
    {
        Name = "";
        RegularPrice = "";
        CampaignPrice = "";
        RegularPriceColor = "";
        RegularPriceSize = new Size();
        CampaignPriceColor = "";
        CampaignPriceSize = new Size();
    }
}
