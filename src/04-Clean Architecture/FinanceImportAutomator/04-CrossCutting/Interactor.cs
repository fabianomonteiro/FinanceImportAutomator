using System;

namespace CleanArchitectureFinanceImportAutomator._04_CrossCutting
{
    public interface IInteractor<TInput, TOutput>
    {
        TOutput Execute(TInput input);
    }

    public class VoidOutput
    {
        public static VoidOutput Empty { get; set; }

        static VoidOutput()
        {
            Empty = new VoidOutput();
        }
    }

    public static class InteractorSettings
    {
        public static ExecuteAspectBase ExecuteAspect { get; set; }
    }

    public abstract class Interactor<TInput, TOutput> : IInteractor<TInput, TOutput>
    {
        public TInput Input { get; set; }

        public TOutput Output { get; set; } 

        protected abstract TOutput ImplementExecute(TInput input);

        public virtual TOutput Execute(TInput input)
        {
            Input = input;

            try
            {
                BeforeExecute();

                DateTime start = DateTime.Now;

                InteractorSettings.ExecuteAspect?.Start(this.GetType().FullName, input);

                Output = ImplementExecute(input);

                DateTime end = DateTime.Now;

                var timeSpan = end.Subtract(start);

                InteractorSettings.ExecuteAspect?.End(this.GetType().FullName, input, Output, timeSpan);

                AfterExecute();
            }
            catch (Exception ex)
            {
                InteractorSettings.ExecuteAspect?.Error(this.GetType().FullName, input, Output, ex);

                throw;
            }
            finally
            {
                FinallyExecute();
            }

            return Output;
        }

        protected virtual void BeforeExecute() { }

        protected virtual void AfterExecute() { }

        protected virtual void FinallyExecute() { }
    }
}
