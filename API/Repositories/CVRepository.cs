using MyWebsite.Models;
using MyWebsite.Tools;

namespace MyWebsite.Repositories
{
    public class CVRepository : BaseRepository<CV>
    {
        public CVRepository(DataDbContext dataContext) : base(dataContext)
        {
        }

        public override List<CV> FindAll()
        {
            return _dataContext.CVs.ToList();
        }

        public override CV FindById(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Save(CV element)
        {
            throw new NotImplementedException();
        }

        public override List<CV> SearchAll(Func<CV, bool> SearchMethod)
        {
            throw new NotImplementedException();
        }

        public override CV SearchOne(Func<CV, bool> SearchMethod)
        {
            throw new NotImplementedException();
        }
    }
}
