using PolicyPro360.Models;
namespace PolicyPro360.ViewModels
{
    public class CreatePolicyViewModel
    {
        public Policy Policy { get; set; }
        public List<PolicyAttribute> Attributes { get; set; } = new List<PolicyAttribute>();
        public IFormFile BrochureUrl { get; set; }
    }

}
