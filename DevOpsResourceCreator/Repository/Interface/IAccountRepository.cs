using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IAccountRepository
    {
        DevOpsAccountRoot GetAll(DevOpsEntity model);
    }
}