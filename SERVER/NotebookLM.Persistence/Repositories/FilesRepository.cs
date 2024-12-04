using NotebookLM.Persistence.Data;
namespace NotebookLM.Persistence.Repositories;

public class FilesRepository : GenericRepository<Domain.Entities.File>
{

    public FilesRepository(NotebookLMDbContext dbContext): base(dbContext)
    {
    }

    public async Task<Domain.Entities.File> AddFileAsync(Domain.Entities.File file)
    {
        await _dbContext.Set<Domain.Entities.File>().AddAsync(file);
        await _dbContext.SaveChangesAsync();
        return file;
    }
}
