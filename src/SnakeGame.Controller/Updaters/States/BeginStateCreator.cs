namespace SnakeGame.Controller.Updaters.States
{
    public class BeginStateCreator
    {
        public virtual IState Create()
        {
            return new InStartMenuState();
        }
    }
}
