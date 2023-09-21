namespace Models.Contracts
{
    public interface ICurriencesConverter
    {
        void UpdateCurriences(string data);

        double GetInConvertion(string isoCurrience, double qty);
    }
}
