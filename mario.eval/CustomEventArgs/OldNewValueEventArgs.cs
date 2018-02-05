namespace Mario.Eval.CustomEventArgs
{
    using System;

    public class OldNewValueEventArgs : EventArgs

    {
        public OldNewValueEventArgs(int oldValue, int newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public int NewValue { get; private set; }

        public int OldValue { get; private set; }
    }
}