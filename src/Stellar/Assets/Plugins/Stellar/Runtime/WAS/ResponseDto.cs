
namespace Plugins.Stellar.Runtime
{
    public sealed record ResponseDto<T>
    {
        public bool Success;
        public string Message;
        public T Data;
    }
}