using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IDevOpsRepository
    {
        void Create(DevOpsEntity devOpsModel);
        string Delete(DevOpsEntity devOpsModel);
        string GetAll(DevOpsEntity model);
    }
}