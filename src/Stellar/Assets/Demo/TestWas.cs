using Cysharp.Threading.Tasks;
using Plugins.Stellar.Runtime;

namespace Demo
{
    public class TestWas : WAS
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