using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IProjectRepository
    {
        string Create(DevOpsEntity devOpsModel);
        string Delete(DevOpsEntity devOpsModel);
        string GetAll(DevOpsEntity model);
        string GetServiceEndPoints(DevOpsEntity model);
    }
}