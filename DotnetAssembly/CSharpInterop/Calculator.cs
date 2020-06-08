namespace CSharpInterop
{
    public static class Calculator
    {
        public static int Add(int a, int b)
        {
            //System.Windows.Interactivity.EventTrigger et = new System.Windows.Interactivity.EventTrigger();
            Form1 form1 = new Form1();
            form1.Show();

            int result = 9;// a + b;
            return result;
        }
    }
}
