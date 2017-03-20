public class Duck
{
    public string Name;
    public string RegularPrice;
    public string RegularPriceColor;
    public double RegularPriceSize;
    public bool IsRegularPriceCrossed;
    public string CampaignPrice;
    public string CampaignPriceColor;
    public double CampaignPriceSize;
    public bool IsCampaignPriceBold;

    

    public Duck()
    {
        Name = "";
        RegularPrice = "";
        CampaignPrice = "";
        RegularPriceColor = "";
        RegularPriceSize = 0;
        CampaignPriceColor = "";
        CampaignPriceSize = 0;
        IsRegularPriceCrossed = false;
        IsCampaignPriceBold = false;
    }
}
