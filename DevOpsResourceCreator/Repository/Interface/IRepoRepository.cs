using DevOpsResourceCreator.Model;

namespace DevOpsResourceCreator
{
    public interface IRepoRepository
    {
        string Create(DevOpsEntity devOpsModel, string repositoryName);
        string Delete(DevOpsEntity devOpsModel);
        string GetAll(DevOpsEntity devOpsModel);

        void Push(DevOpsEntity model);
        string PushPolicy(DevOpsEntity model);
        string PushPipeline(DevOpsEntity model, string templateName, string pipeline);
    }
}
