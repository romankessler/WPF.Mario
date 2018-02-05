namespace Mario.Eval.UserControls
{
    using System.ComponentModel;
    using System.Windows.Controls;

    public abstract class ControlBase : Control
    {
        protected bool IsTemplateApplied { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            EvaluateControls();
            WireUpControlEvents();
            IsTemplateApplied = true;

            ApplyTemplateForChilds();
            OnTemplateApplied();
        }

        protected virtual void ApplyTemplateForChilds()
        {
        }

        protected abstract void EvaluateControls();

        protected abstract void OnTemplateApplied();

        protected abstract void WireUpControlEvents();
    }
}