
namespace LeaseExtension.World.Contract
{
    public interface ITrack
    {
        public float StartX { get; set; }
        public float EndX { get; set; }
        public float Speed { get; set; }

        public bool IsFree { get; }

        public void Construct(IConstructionBlank blank);

        public void Move();

        public void WipeOut();
    }
}
