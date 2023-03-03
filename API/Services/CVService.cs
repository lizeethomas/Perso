using MyWebsite.Models;
using MyWebsite.Repositories;

namespace MyWebsite.Services
{
    public class CVService
    {
        private readonly CVRepository _cvRepository;

        public CVService(CVRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public List<CV> GetCVs()
        {
            List<CV> list = _cvRepository.FindAll();
            return list;
        }
    }
}
