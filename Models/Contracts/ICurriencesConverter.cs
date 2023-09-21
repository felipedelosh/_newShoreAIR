namespace Models.Contracts
{
    public interface ICurriencesConverter
    {
        void updateCurriences(string data);

        double GetInConvertion(string isoCurrience, double qty);
    }
}
