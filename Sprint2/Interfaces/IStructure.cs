using Microsoft.Xna.Framework;



namespace Sprint5
{
    public interface IStructure: IGameObject
    {
        IUseability UState { get; set; }

        void Broken(GameTime gametime);
    }
}
