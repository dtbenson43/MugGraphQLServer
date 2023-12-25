namespace Mug.Mutation
{
    public partial class Mutation
    {
        public UpdateZigBobRegistryPayload UpdateZugBobRegistry(ZigBob zigBob)
        {
            zigBob.Blomf = "wow";
            var result = new UpdateZigBobRegistryPayload();
            return result;
        }
    }

    public class UpdateZigBobRegistryPayload
    {
        public string Status { get; set; } = "failed";
    }

    public class ZigBob
    {
        public string? Blomf {  get; set; }
    }
}
