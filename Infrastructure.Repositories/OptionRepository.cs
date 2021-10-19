using Domain.Model;

namespace Infrastructure.Repositories
{
    public class OptionRepository : BaseRepository<Option>
    {
        public OptionRepository(DataContext context) : base(context)
        { }
    }
}
