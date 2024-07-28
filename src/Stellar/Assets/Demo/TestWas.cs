using Cysharp.Threading.Tasks;
using Plugins.Stellar.Runtime;

namespace Demo
{
    public class TestWas : WebApplicationServer
    {
        public TestWas(string baseUrl) : base(baseUrl)
        {
        }

        public async UniTask<ResponseDto<string>> GetHello()
        {
            return await Get<string>();
        }
    }
}