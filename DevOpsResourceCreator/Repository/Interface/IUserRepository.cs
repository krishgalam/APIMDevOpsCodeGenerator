using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IUserRepository
    {
        string Create(DevOpsEntity devOpsModel);
        string GetAll(DevOpsEntity devOpsModel);
    }
}
