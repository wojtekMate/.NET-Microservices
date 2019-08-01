namespace Mikro.Service
{
    public class Calculate : ICalculate
    {
        public int calculate(int number)
        {
            int a = 0;
            int b = 1;
            for (int i = 0; i < number; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }

            return a;
        }
    }
}