using System;

public class Duck
{
    public string Name;
    public string RegularPrice;
    public string CampaignPrice;
    public string RegularPriceAttributes;
    public string CampaignPriceAttribute;

    public Duck(string name, string regularPrice, string campaignPrice, string regularPriceAttributes, string campaignPriceAttribute)
	{
        Name = name;
        RegularPrice = regularPrice;
        CampaignPrice = campaignPrice;
        RegularPriceAttributes = regularPriceAttributes;
        CampaignPriceAttribute = campaignPriceAttribute;

    }
}
