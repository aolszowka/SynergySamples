namespace CSharpInterop
{
    public static class Calculator
    {
        public static int Add( int a, int b )
        {
            System.Windows.Interactivity.EventTrigger et = new System.Windows.Interactivity.EventTrigger();
            int result = a + b;
            return result;
        }
    }
}
