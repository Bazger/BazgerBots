namespace Bazger.Bots.Core
{
    public abstract class BotComponent
    {
        public readonly bool ToRun;

        protected BotComponent(bool toRun)
        {
            ToRun = toRun;
        }

        public abstract void Prepare(BotStateBuilder botState);
        public abstract void Process(BotStateBuilder botState);

        public virtual int GetRetriesCount()
        {
            return 3;
        }

    }
}
