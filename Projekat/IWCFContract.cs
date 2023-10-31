using System.ServiceModel;

namespace Projekat
{
    [ServiceContract]
    public interface IWCFContract
    {
        [OperationContract]
        void TestCommunication();
    }
}
