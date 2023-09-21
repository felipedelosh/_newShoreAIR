namespace Models.Contracts
{
    public interface IGetAPIData
    {
        string GetHTTPServiceVr0(string url);

        string GetHTTPServiceVr1(string url);

        string GetHTTPServiceVr2(string url);

        string GetDicHTTPServiceCurriences(string url);
    }
}
