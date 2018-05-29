using FluffySpoon.Publisher.Remote;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.GitHub
{
    class GitHubSourceControlSystem : IGitHubSourceControlSystem
    {
        private readonly string _username;

        private readonly IGitHubClient _client;
        private readonly IGitHubSourceControlRepositoryFactory _repositoryFactory;

        public GitHubSourceControlSystem(
            string username,
            IGitHubClient client,
            IGitHubSourceControlRepositoryFactory repositoryFactory)
        {
            _username = username;
            _client = client;
            _repositoryFactory = repositoryFactory;
        }

        public async Task<int> GetRevisionOfRepository(IRemoteSourceControlRepository repository)
        {
            var githubRepository = (GitHubSourceControlRepository)repository;
            var commits = await _client.Repository.Commit.GetAll(
              githubRepository.Owner,
              githubRepository.Name);

            return commits.Count;
        }

        public async Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetCurrentUserRepositoriesAsync()
        {
            var repositories = await _client.Repository.GetAllForUser(_username);
            return repositories
              .Select(Map)
              .ToArray();
        }

        private GitHubSourceControlRepository Map(
          Repository githubClientRepository)
        {
            var repository = _repositoryFactory.Create();
            repository.System = this;
            repository.Name = githubClientRepository.Name;
            repository.Owner = githubClientRepository.Owner.Login;
            repository.PublicUrl = githubClientRepository.HtmlUrl;
            repository.Summary = githubClientRepository.Description;
            repository.GitHubClientRepository = githubClientRepository;

            return repository;
        }
    }
}
