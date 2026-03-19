using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class TagController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public void Add(string tag)
        {
            var str = tag;
            //_unitOfWork.Tags.Create();
            
        }

        public void Edit()
        {
            
        }

        public void Delete()
        {
        }
    }
}
