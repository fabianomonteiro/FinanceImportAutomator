namespace FinanceImportAutomator._04_CrossCutting
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

                Output = ImplementExecute(input);

                AfterExecute();
            }
            catch
            {
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
