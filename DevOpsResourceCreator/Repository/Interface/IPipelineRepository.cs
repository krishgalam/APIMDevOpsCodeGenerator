using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IPipelineRepository
    {
        string Create(DevOpsEntity devOpsModel);
        string Delete(DevOpsEntity devOpsModel);
        string GetAll(DevOpsEntity devOpsModel);
    }
}